using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public interface IPlayerController
{
    //void PlayerMove();
}

// 필요 : 공격 -> Move x (state 분리)
public partial class PlayerController : MonoBehaviour, IPlayerController
{
    private IPlayerState _idleState, _moveState, _attackState, _damagedState, _dieState;

    private PlayerStateContext _playerStateContext;
    
    // 교체 필요
    public Direction CurrentMoveDirection
    {
        get;
        private set;
    }
    
    // 이전 스크립트
    
    public PlayerStats playerStats;
    
    // 애니메이션
    public Animator anim;
    
    // 상태 (생존)
    public bool isAlive = true;
    
    // terrain 
    public LayerMask terrainLayer;
    public float groundDist;
    
    // 공격
    public int CombatPower = 10; // 전투력
    
    // 쿨타임
    public float skillCooldown = 5f;
    public float lastSkillTime = -5f;
    public bool isSkillOnCooldown = false;
    
    // 조이스틱
    public FullScreenJoystick joystick;

    // 레이어 마스크 : 가장 가까운 몬스터 탐지에 필요
    public LayerMask monsterLayerMask;
    
    [SerializeField] private GameObject nearestMonster;
    
    // 공격 이펙트
    public GameObject attackEffect;
    public GameObject skillEffect;

    void Start()
    {
        /*
        anim = GetComponent<Animator>();
        HPSliderUpdate();
        attackEffect.SetActive(false);
        skillEffect.SetActive(false);
        monsterLayerMask = LayerMask.GetMask("Enemy");
        StartCoroutine(DetectNearestMonsterCoroutine());
        */

        _playerStateContext = new PlayerStateContext(this);

        _idleState = gameObject.AddComponent<PlayerIdleState>();
        _moveState = gameObject.AddComponent<PlayerMoveState>();
        _attackState = gameObject.AddComponent<PlayerAttackState>();
        _damagedState = gameObject.AddComponent<PlayerDamagedState>();
        _dieState = gameObject.AddComponent<PlayerDieState>();
        
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
    
    public void AttackPlayer()
    {
        _playerStateContext.Transition(_attackState);
    }
    
    public void DamagedPlayer()
    {
        _playerStateContext.Transition(_damagedState);
    }
    
    public void DiePlayer()
    {
        _playerStateContext.Transition(_dieState);
    }

    public void Move(Direction direction)
    {
        CurrentMoveDirection = direction;
        _playerStateContext.Transition(_moveState);
    }
    

    /*
    public void PlayerMove()
    {
        // terrain raycast
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;

        float UIDist = 0.75f;
        
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist + UIDist;
                transform.position = movePos;
            }
        }
        
        anim.SetBool("isMove", false);
        
        float horizontalInput = 0f;
        float verticalInput = 0f;
        
        // 키보드 + 조이스틱 입력을 위한 새로운 변수
        Vector3 combinedInput = Vector3.zero;;

        if (joystick.isDragging) // 조이스틱값 들어올 때만
        {
            // 조이스틱 입력값
            Vector2 joystickInput = joystick.GetInputDirection();
            combinedInput = new Vector3(joystickInput.x, 0, joystickInput.y);
            Debug.Log(combinedInput);
        }
        else // 키보드
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            combinedInput = new Vector3(horizontalInput, 0, verticalInput);
        }
        
        Vector3 moveVelocity = combinedInput.normalized * playerStats.movement_Speed * Time.deltaTime;
        //Vector3 moveVelocity = combinedInput.normalized * movement_Speed * Time.deltaTime;
        transform.position += moveVelocity;
        
        // 애니메이션
        bool isMoving = joystick.isDragging ? joystick.GetInputDirection() != Vector2.zero : (horizontalInput != 0 || verticalInput != 0);
        
        
        if (isMoving)
        {
            // isDragging : true -> joystick 입력값 / false -> 키보드 입력값
            float xDirectionInput = joystick.isDragging ? joystick.GetInputDirection().x : horizontalInput; 
            
            if (xDirectionInput > 0)
            {
                transform.localScale = new Vector3(-2f, 2f, -1f);
            }
            else
            {
                transform.localScale = new Vector3(2f, 2f, -1f);
            }
            anim.SetBool("isMove", true);
        }
    }
    */

    
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

        if (nearestMonster != null)
        {
            Debug.Log("nearestMonster:" + nearestMonster);
        }
    }
    
    // 기본 공격 (attack02): 치명타 - 공격력의 175% (나중)
    void PlayerAttackAnim()
    {
        EnemyFSM enemyFsm = nearestMonster.GetComponent<EnemyFSM>();
        CreateAttackEffect();

        if (enemyFsm != null)
        {
            int attackDamage = CalculateAttackDamage(playerStats.attack, playerStats.attack_Multiplier, playerStats.critical_Multiplier);
            enemyFsm.HitEnemy(attackDamage); // 일반공격 
            Debug.Log("3. HitEnemy");
        }
        else
        {
            return;
        }
    }
    
    // 스킬 공격 (attack01)
    void PlayerSkillAnim()
    {
        EnemyFSM enemyFsm = nearestMonster.GetComponent<EnemyFSM>();
        CreateSkillEffect();

        if (enemyFsm != null)
        {
            int attackDamage = CalculateSkillDamage(playerStats.attack, playerStats.skill_Multiplier);
            enemyFsm.HitEnemy(attackDamage); // 스킬공격 
            Debug.Log("3. HitEnemy");
        }
        else
        {
            return;
        }
    }
    
    // 이펙트, 코루틴을 파라미터로 받는 공통 함수 (일정 시간 후 비활성화)
    void ActivateEffect(GameObject effect, float duration)
    {
        if (effect != null)
        {
            effect.SetActive(true);
            StartCoroutine(DeactivateEffect(effect, duration));
        }
    }

    // 지정된 시간이 지난 후 게임 오브젝트를 비활성화하는 코루틴
    IEnumerator DeactivateEffect(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (effect != null)
        {
            effect.SetActive(false);
        }
    }
    
    // 기본 공격 이펙트
    void CreateAttackEffect()
    {
        ActivateEffect(attackEffect, 1.5f); 
    }

    // 스킬 공격 이펙트
    void CreateSkillEffect()
    {
        ActivateEffect(skillEffect, 1.5f); 
    }

    // 피격 함수
    public void PlayerDamaged(int damage)
    {
        // 에너미의 공격력만큼 플레이어의 체력 깎기
        playerStats.currentHP -= damage;
        HPSliderUpdate();
    }
}
