using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IPlayerController
{
    //void PlayerMove();
}

// 필요 : 공격 -> Move x (state 분리)
public partial class PlayerController : MonoBehaviour, IPlayerController
{
    // State Pattern 적용을 위해 추가 
    private IPlayerState _idleState, _moveState, _autoState, _attackState, _skillState, _damagedState, _dieState;
    
    private PlayerStateContext _playerStateContext;
    
    public PlayerStats playerStats;
    
    // 애니메이션
    public Animator anim;
    
    // 상태 (생존)
    public bool isAlive = true;

    // 조이스틱
    public FullScreenJoystick joystick;
    
    // 슬라이더
    public Slider hpSlider;
    public Slider cooldownSlider;

    // 가장 가까운 몬스터 탐지
    public LayerMask monsterLayerMask; // 레이어 마스크 
    public GameObject nearestMonster;
    
    // 이동 몬스터 
    public GameObject targetMonster;
    private float timeSinceLastTargetUpdate = 0f; // 타겟 몬스터가 마지막으로 업데이트된 이후 시간
    private float targetUpdateCooldown = 10f; // 타겟 몬스터 업데이트 쿨다운 시간 (10초)
    
    public GameObject hudDamageText;

    public bool flipX;

    public Transform ponpo;
    public Rigidbody rigid;

    public float searchRadius = 50f;

    // 상태: 필요에 따라 인스턴스화, 상태 컨텍스트(PlayerController)를 통해 관리
    void Start()
    {
        ponpo = transform.GetChild(0);
        anim = ponpo.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        if (!joystick)
        {
            joystick = FindObjectOfType<FullScreenJoystick>();
        }
        
        // 상태 객체: 인스턴스화 필요 (일반 클래스 인스턴스로 생성)
        _idleState = new PlayerIdleState(); 
        _moveState = new PlayerMoveState();
        _autoState = new PlayerAutoState();
        _attackState = new PlayerAttackState();
        _skillState = new PlayerSkillState();
        _damagedState = new PlayerDamagedState();
        _dieState = new PlayerDieState();

        // 상태 관리자 인스턴스 생성 및 초기 상태로 전환
        _playerStateContext = new PlayerStateContext(this);
        _playerStateContext.Transition(_idleState);

        PlayerInit();
    }

    void Update()
    {
        timeSinceLastTargetUpdate += Time.deltaTime;
        
        // 10초가 지났을 경우, 또는 targetMonster가 null일 경우에만 타겟을 업데이트
        if (timeSinceLastTargetUpdate >= targetUpdateCooldown || targetMonster == null)
        {
            MoveTowardsNearestEnemy();
            // 타겟이 업데이트되면 타이머를 리셋
            timeSinceLastTargetUpdate = 0f;
        }
    }
    
    // targetMonster
    private void MoveTowardsNearestEnemy()
    {
        List<Point> nearbyEnemies = QuadtreeManager.Instance.QueryNearbyEnemies(transform.position, searchRadius);
        
        if (nearbyEnemies.Count > 0)
        {
            // 플레이어와 가장 가까운 몬스터 찾기
            Point targetMonster = nearbyEnemies
                .OrderBy(enemy => Vector3.Distance(transform.position, new Vector3(enemy.x, 0, enemy.z)))
                .FirstOrDefault();
        
            // 해당 몬스터로 향해 이동
            if (targetMonster != null)
            {
                // 디버그 로그로 몬스터의 프리팹 이름 출력
                Debug.Log($"[targetMonster] Moving towards target monster: {targetMonster.x}, {targetMonster.z}");
                
                Vector3 targetPosition = new Vector3(targetMonster.x, transform.position.y, targetMonster.z);
                float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
                
                if (distanceToTarget <= 0.2f)
                {
                    // 이동을 멈춤
                    anim.SetBool("isMove", false);
                }
                else
                {
                    // 목표 몬스터와의 거리가 공격 범위를 초과할 경우
                    Vector3 moveDirection = (targetPosition - transform.position).normalized * playerStats.movement_Speed;
                    Vector3 newPosition = transform.position + moveDirection * Time.deltaTime; // 현재 위치에서 목표 위치로 이동하기 위한 새 위치 계산
                    rigid.MovePosition(newPosition); // 이동

                    // 이동 중 방향 전환 처리
                    FlipPlayer(moveDirection.x);

                    // 이동 애니메이션 활성화
                    anim.SetBool("isMove", true);
                }
                
                // 몬스터 오브젝트 할당
                DetectAndAttackNearestMonster();
            }
        }
        else // 몬스터가 검색 반경 내에 없을 경우 이동 중지 애니메이션
        {
            anim.SetBool("isMove", false);
        }
    }

    void PlayerInit()
    {
        // 초기 1회 필요 
        isAlive = true;
        flipX = false;
        DeactivateEffects();
        monsterLayerMask = LayerMask.GetMask("Enemy");
        StartCoroutine(DetectNearestMonsterCoroutine());
        playerStats.OnPlayerHPChanged += HandlePlayerHpChange; // 체력에 대한 이벤트 구독
        
        isSkillOnCooldown = false;
        lastSkillTime = -skillCooldown;
        lastHitTime = -hitCooldown;
    }

    public void IdlePlayer()
    {
        _playerStateContext.Transition(_idleState);
    }
    
    public void MovePlayer()
    {
        _playerStateContext.Transition(_moveState);
    }

    public void AutoPlayer()
    {
        _playerStateContext.Transition(_autoState);
    }
    
    public void AttackPlayer()
    {
        _playerStateContext.Transition(_attackState);
    }
    
    public void SkillPlayer()
    {
        _playerStateContext.Transition(_skillState);
    }
    
    public void DamagedPlayer()
    {
        _playerStateContext.Transition(_damagedState);
    }
    
    public void DiePlayer()
    {
        if (!isAlive) return;
        _playerStateContext.Transition(_dieState);
    }
    
    // EnemyFSM에서 공격할 때, 호출 (->_damageState)
    public void ReceiveDamage(int damage)
    {
        if (isAlive && playerStats.CurrentHP > 0)
        {
            playerStats.CurrentHP -= damage;
            CreateDamageText(damage);

            if (playerStats.CurrentHP <= 0)
            {
                DiePlayer();
            }
            
            else
            {
                DamagedPlayer();
            }
        }
    }

    // 체크 시간 : 3초
    public IEnumerator DetectNearestMonsterCoroutine()
    {
        while (true)
        {
            DetectAndAttackNearestMonster();
            yield return new WaitForSeconds(3f);
        }
    }
    
    void DetectAndAttackNearestMonster()
    {
        float detectionRadius = 5f; // 몬스터 탐지 반경
        
        // 주어진 반경 내에 있는 모든 콜라이더 -몬스터 레이어에 속하는- 를 배열로 반환
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, monsterLayerMask);

        nearestMonster = null; // 가장 가까운 몬스터 
        float minDistance = Mathf.Infinity; // 초반 : 무한대로 설정

        foreach (Collider collider in hitColliders)
        {
            // 플레이어와 몬스터 사이 거리 계산, 현재까지 발견된 가장 짧은 거리보다 짧은지 확인
            float distance = Vector3.Distance(transform.position, collider.transform.position);  
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestMonster = collider.gameObject;
            }
        }
    }

    void HandlePlayerHpChange(int currentHP, int maxHP)
    {
        Debug.Log($"[Player] Handling HP Change. New HP: {currentHP}/{maxHP}");
        Utilities.HPSliderUpdate(hpSlider, playerStats.CurrentHP, playerStats.maxHP);
    }
    
    void CreateDamageText(int hitPower)
    {
        if (hudDamageText != null) // 데미지 텍스트 
        {
            // flipX을 기준으로 위치 계산
            float offsetDirection = flipX ? -1.0f : 1.0f;
            //Vector3 damagePosition = transform.position + new Vector3(1.0f, 2.0f, 0);
            Vector3 damagePosition = transform.position + new Vector3(offsetDirection * 1.0f, 2.0f, 0);
            GameObject damageText = Instantiate(hudDamageText, damagePosition, Quaternion.identity, transform.root); // 자식으로 생성

            damageText.GetComponent<DamageText>().damage = hitPower;
        }
    }
    
    public void FlipPlayer(float horizontalInput)
    {
        // FlipX 기준으로 스프라이트 방향 전환
        if (horizontalInput < 0 && flipX || horizontalInput > 0 && !flipX)
        {
            // ponpo의 localScale x 값을 반전시켜 방향 전환
            Vector3 theScale = ponpo.localScale;
            theScale.x *= -1;
            ponpo.localScale = theScale;

            // flipX 상태 업데이트
            flipX = !flipX;
        }
    }
    
    void OnDrawGizmos()
    {
        // 플레이어 위치를 중심으로 하는 queryArea 시각화
        Gizmos.color = Color.cyan; // 기즈모 색상 설정
        Gizmos.DrawWireSphere(transform.position, searchRadius); // 반경을 사용하여 와이어 프레임 구체 그리기
    }
}
