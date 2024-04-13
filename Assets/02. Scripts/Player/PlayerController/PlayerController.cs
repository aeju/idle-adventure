using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    public GameObject hudDamageText;

    public bool flipX;

    public Transform ponpo;
    public Rigidbody rigid;
    
    [SerializeField] private float detectionRadius = 5f; // 탐지 반경 설정

    // 상태: 필요에 따라 인스턴스화, 상태 컨텍스트(PlayerController)를 통해 관리
    void Start()
    {
        ponpo = transform.GetChild(0);
        anim = ponpo.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        if (!joystick)
        {
            Debug.LogError("No joystick");
        }
        
        InitializeStates(); // State 패턴 초기 설정
        PlayerInit();
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
    
    // 상태 객체와 상태 관리자 인스턴스를 초기화
    private void InitializeStates()
    {
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
        float detectionRadius = 5f; 
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, monsterLayerMask);

        nearestMonster = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
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
            Vector3 damagePosition = transform.position + new Vector3(offsetDirection * 1.0f, 2.0f, 0);
            GameObject damageText = Instantiate(hudDamageText, damagePosition, Quaternion.identity, transform.root); // 자식으로 생성
            
            damageText.GetComponent<DamageText>().damage = hitPower;
        }
    }
    
    // FlipX 기준으로 스프라이트 방향 전환
    public void FlipPlayer(float horizontalInput)
    {
        if (horizontalInput < 0 && flipX || horizontalInput > 0 && !flipX)
        {
            Vector3 theScale = ponpo.localScale; 
            theScale.x *= -1; // ponpo의 localScale x 값을 반전시켜 방향 전환
            ponpo.localScale = theScale;
            
            flipX = !flipX; // flipX 상태 업데이트
        }
    }
    
    // 문제 : 10마리 탐지 주기 -> 이동이 멈췄을 때로 제한
    void Update()
    {
        /*
        List<GameObject> skillmonsters = monstersInRange();
        if (skillmonsters.Count > 0)
        {
            Debug.Log($"Detected {skillmonsters.Count} monsters in range:");
            
            foreach (GameObject monster in skillmonsters)
            {
                Debug.Log($"Detected List: {monster.name}");
            }
        }
        */
        
        List<GameObject> attackmonsters = monstersInRange();
        if (attackmonsters.Count > 0)
        {
            Debug.Log($"Detected {attackmonsters.Count} monsters in range:");
            
            foreach (GameObject monster in attackmonsters)
            {
                Debug.Log($"Detected List: {monster.name}");
            }
        }
    }
    
    // 일단은 Update에서 -> 추후, 이동 완료 플래그(isReached) 후 실행!  
    // 지정된 범위 내에서 모든 몬스터를 찾아 리스트로 반환하는 메서드
    public List<GameObject> monstersInRange()
    {
        List<GameObject> skillMonsters = new List<GameObject>();

        // 현재 위치에서 detectionRadius 내의 모든 콜라이더를 검색
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, monsterLayerMask);
        
        // 거리에 따라 몬스터 리스트를 정렬 (sqrtMagnitude : 두 오브젝트 단순 거리 비교)
        skillMonsters = colliders
            .Select(collider => collider.gameObject) // 검색된 콜라이더에서 게임 오브젝트 추출
            .Where(gameObject => gameObject != this.gameObject) // 플레이어 자신은 제외
            .OrderBy(gameObject => (transform.position - gameObject.transform.position).sqrMagnitude) // 거리에 따라 정렬
            .Take(10) // 최대 10마리의 몬스터만 반환
            .ToList();
        
        return skillMonsters;
    }
    
    // 플레이어가 바라보는 앞 방향으로만 몬스터를 탐지하는 메서드
    public List<GameObject> GetMonstersInFront()
    {
        List<GameObject> attackMonsters = new List<GameObject>();
        
        // 플레이어의 바라보는 방향 계산
        Vector3 forward = flipX ? transform.right : -transform.right;
        Vector3 center = transform.position + forward * (detectionRadius / 2);
        
        Collider[] colliders = Physics.OverlapSphere(center, detectionRadius / 2, monsterLayerMask);
        
        attackMonsters = colliders
            .Select(collider => collider.gameObject)
            .Where(gameObject => gameObject != this.gameObject) // 플레이어 자신은 제외
            .OrderBy(gameObject => (transform.position - gameObject.transform.position).sqrMagnitude) // 거리에 따라 정렬
            .Take(5) // 최대 5마리까지
            .ToList();

        return attackMonsters;
    }

    
    void OnDrawGizmos()
    {
        // skill 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // 현재 위치를 중심으로 하는 구

        // attack 범위
        Gizmos.color = Color.magenta;
        
        Vector3 forward = flipX ? transform.right : -transform.right;
        Vector3 center = transform.position + forward * (detectionRadius / 2);
        Gizmos.DrawWireSphere(center, detectionRadius / 2);
    }
}
