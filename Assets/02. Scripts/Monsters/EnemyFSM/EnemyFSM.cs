using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// 필요 1: Idle상태 -> 주변 배회
// 필요 2: damaged 애니메이션
public partial class EnemyFSM : MonoBehaviour
{
    [SerializeField] public MonsterStats monsterStats;
    
    [SerializeField] private UserInfoManager userInfo;
    [SerializeField] private ResourceManager resourceInfo;
    
    [SerializeField] private GameObject hudDamageText;

    // 에너미 상태 상수
    enum EnemyState
    {
        Idle,
        Wander,
        Chase,
        Attack,
        Return,
        Damaged,
        Die
    }
    
    // 에너미 상태 변수
    private EnemyState m_State;
    private Rigidbody rigid;
    
    // 플레이어 발견 범위
    [SerializeField] float findDistance = 8f;
    // 공격 가능 범위
    [SerializeField] float attackDistance = 2f;
    // 배회 가능 최대 거리
    [SerializeField] float wanderDistance = 3f;
    
    // 캐릭터 컨트롤러 컴포넌트
    private CharacterController cc;

    // 플레이어 
    public PlayerController target;

    // 일정한 시간 간격으로 공격 -> 누적 시간, 공격 딜레이 시간
    private float currentTime = 0; // 누적 시간
    private float attackDelay = 1f; // 공격 딜레이 시간
    [SerializeField] private float wanderDelay = 3f; // 배회 방향 바꾸는 시간 딜레이 
    [SerializeField] private float dieDelay = 1.5f; // 죽음 딜레이 시간
    
    public bool flipX;
    
    // 초기 위치 저장용 변수
    private Vector3 originPos;
    // 이동 가능 범위
    [SerializeField] float moveDistance = 20f;
    
    // 배회 시간과 다음 이동 목적지를 위한 변수
    [SerializeField] private float wanderTime = 5f; // 배회 상태를 유지할 시간
    private Vector3 wanderPosition; // 다음 이동 목적지
    
    [SerializeField] Slider hpSlider;
    
    // 애니메이션 
    private Animator anim;
    private SkeletonMecanim skeletonMecanim;
    
    [SerializeField] GameObject dropItem;
    
    // HP 이벤트
    // public event 

    void Awake()
    {
        target = FindObjectOfType<PlayerController>();
        
        // 몬스터 - 캐릭터 컨트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        skeletonMecanim = GetComponent<SkeletonMecanim>();
        
        userInfo = UserInfoManager.Instance;
        resourceInfo = ResourceManager.Instance;
        
        monsterStats.currentHP = monsterStats.maxHP;  // HP 최대치로 초기화
    }

    void OnEnable()
    {
        m_State = EnemyState.Idle; // 최초의 에너미 상태 : Idle
        originPos = transform.position; // 자신의 초기 위치 저장
        HPSliderUpdate(hpSlider, monsterStats.currentHP, monsterStats.maxHP); // HP 바 업데이트
        currentTime = 0; // 타이머 리셋
    }

    void Update()
    {
        if (target == null)
        {
            m_State = EnemyState.Idle;
            return;
        }
        
        // 현재 상태를 체크, 해당 상태별로 정해진 기능 수행
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Wander:
                Wander();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged: 
                break;
            case EnemyState.Die:
                break;
        }
        FlipTowardsPlayer();
    }
    
    // 방향 전환: 플레이어에게 이동, originPos로 복귀
    // 플레이어가 왼쪽에 있다면, scalex = -1 (좌우반전)
    private void FlipTowardsPlayer()
    {
        if (m_State == EnemyState.Return) // originPos
        {
            flipX = originPos.x > transform.position.x; 
        }
        else if (m_State == EnemyState.Wander)
        {
            flipX = wanderPosition.x > transform.position.x;
        }
        else if (target != null)
        {
            flipX = target.transform.position.x > transform.position.x;
        }
        else 
        {
            flipX = skeletonMecanim.Skeleton.ScaleX > 0;
        }
        
        skeletonMecanim.Skeleton.ScaleX = flipX ? 1 : -1; // true = 오른쪽, false = 왼쪽
    }

    // 1) 플레이어 거리가 가까우면: Chase
    // 2) 플레이어가 멀리 있으면: Wander
    void Idle()
    {
        // 만일, 플레이어와의 거리가 액션 시작 범위 이내라면 Chase 상태로 전환
        if (Vector3.Distance(transform.position, target.transform.position) < findDistance)
        {
            m_State = EnemyState.Chase;
            print("상태 전환: Idle -> Move");
            
        }
        else // Wander
        {
            m_State = EnemyState.Wander;
            print("상태 전환: Idle -> Wander");
        }
        
        // 이동 애니메이션으로 전환
        anim.SetTrigger("IdleToMove");
    }
    
    // 1) 추격 범위 안에 들어오지 않았을 때: 배회
    // 2) 추격 범위 안에 들어오면: 상태 전환(추격)
    void Wander()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < findDistance)
        {
            m_State = EnemyState.Chase;
            print("상태 전환: Idle -> Move");
            
            // 이동 애니메이션으로 전환
            anim.SetTrigger("IdleToMove");
        }

        else
        {
            // 타이머가 경과하면 새로운 배회 위치를 결정
            if (currentTime <= 0f)
            {
                GetNewWanderDestination();
                currentTime = wanderTime;
            }
    
            // 현재 위치에서 목적지까지의 방향을 계산하고, 그 방향으로 이동
            Vector3 dir = (wanderPosition - transform.position).normalized;
            cc.Move(dir * monsterStats.movement_Speed * Time.deltaTime);

            // 오브젝트가 목적지에 도달하면 타이머를 리셋하여 새로운 위치를 결정할 준비
            if (Vector3.Distance(transform.position, wanderPosition) < 0.5f)
            {
                currentTime = 0;
            }
        }
    }
    
    private void GetNewWanderDestination()
    {
        // 원점을 기준으로 x축 방향으로 랜덤한 위치 결정
        float randomX = originPos.x + Random.Range(-wanderDistance, wanderDistance);
        // y축과 z축은 현재 위치를 유지
        float y = transform.position.y;
        float z = transform.position.z;
        // 새로운 배회 목적지 설정
        wanderPosition = new Vector3(randomX, y, z);
    }

    // 1) 공격 범위 안에 들어오지 않았을 때: 이동
    // 2) 공격 범위 안에 들어왔을 때: 상태 전환(공격)
    void Chase()
    {
        // 만일 현재 위치가 초기 위치에서 이동 가능 범위를 넘어간다면 (이동 가능 거리 체크)
        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            // 현재 상태: 복귀(Return)으로 전환
            m_State = EnemyState.Return;
            print("상태 전환: Move -> Return");
        }
        
        // 만일, 플레이어와의 거리가 공격 범위 밖이라면, 플레이어를 향해 이동
        else if (Vector3.Distance(transform.position, target.transform.position) > attackDistance)
        {
            // 이동 방향 설정
            Vector3 dir = (target.transform.position - transform.position).normalized;
            cc.Move(dir * monsterStats.movement_Speed * Time.deltaTime); // 이동
        }
        
        // 그렇지 않다면, 현재 상태를 공격으로 전환
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");
            
            // 누적 시간을 공격 딜레이 시간만큼 미리 진행시켜 놓기 (공격 상태로 전환됐을 때 기다렸다가 공격 시작하는 문제)
            currentTime = attackDelay;
            
            // 공격 대기 애니메이션 플레이
            anim.SetTrigger("MoveToAttackDelay");
        }
    }

    // 1) 플레이어가 공격 범위 안에 있을 때: 공격
    // 2) 플레이어가 공격 범위 밖에 있을 때: 상태 전환(이동)
    void Attack()
    {
        // 만일, 플레이어가 공격 범위 이내에 있다면 플레이어를 공격
        if (Vector3.Distance(transform.position, target.transform.position) < attackDistance)
        {
            // 플레이어 hp > 0일 때만, (생존 상태)
            if (target.playerStats.currentHP > 0)
            {
                // 일정한 시간마다 플레이어를 공격
                currentTime += Time.deltaTime; // 경과 시간 누적
                if (currentTime > attackDelay) // 경과 시간 > 공격 딜레이 시간
                {
                    print("공격, PlayerHP: " + target.GetComponent<PlayerController>().playerStats.currentHP);
                    currentTime = 0; // 경과 시간 초기화
                    anim.SetTrigger("StartAttack"); // 공격 애니메이션 플레이
                }
            }
        }
        
        // 그렇지 않다면, 현재 상태를 이동(Move)으로 전환(재추격 실시)
        // 공격 중이라도, 플레이어가 공격 범위를 넘어가면 이동 상태로 변환 (경과 시간 초기화!)
        else
        {
            m_State = EnemyState.Chase;
            print("상태 전환: Attack -> Move");
            currentTime = 0;
            
            anim.SetTrigger("AttackToMove");
        }
    }
    
    // 플레이어의 스크립트의 데미지 처리 함수 실행
    public void AttackAction()
    {
        if (target != null)
        {
            target.DamagedPlayer();
            target.ReceiveDamage(CombatCalculator.CalculateAttackDamage(monsterStats.attack, target.playerStats.defense, monsterStats.attack_multiplier, monsterStats.critical_multiplier));
            
            // 플레이어가 반대 방향 보고 있으면, 뒤집기 
            float playerToMonsterDistance = transform.position.x - target.transform.position.x;
            target.FlipPlayer(playerToMonsterDistance);
        }
    }

    void Return()
    {
        // 만일 초기 위치에서 거리가 0.1f이상이라면, 초기 위치 쪽으로 이동
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * monsterStats.movement_Speed * Time.deltaTime);
        }
        // 그렇지 않다면, 자신의 위치를 초기 위치로 조정 + 현재 상태를 대기로 전환
        else
        {
            transform.position = originPos;
            m_State = EnemyState.Idle;
            print("상태 전환: Return -> Idle");
            
            anim.SetTrigger("MoveToIdle");
        }
    }

    // 데미지 실행 함수
    // 1) 피격
    // 2) 죽음
    public void HitEnemy(int hitPower)
    {
        // 이미 피격 상태이거나, 사망 상태라면 아무런 처리도 하지 않고 함수 종료
        if (m_State == EnemyState.Damaged || m_State == EnemyState.Die)
        {
            return;
        }
        
        // 플레이어의 공격력만큼 에너미의 체력 감소
        monsterStats.currentHP -= hitPower;
        // 데미지 텍스트 생성
        CreateDamageText(hitPower);
        
        // 에너미의 체력이 0보다 크면 피격 상태로 전환
        if (monsterStats.currentHP > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any state -> Damaged");
            
            anim.SetTrigger("Damaged"); // 피격 애니메이션 플레이 
            
            Damaged();;
        }
        
        // 그렇지 않다면 죽음 상태로 전환
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any state -> Die");
            
            // 죽음 애니메이션을 플레이
            anim.SetTrigger("Die");
            Die();
        }
    }
    
    void Damaged()
    {
        HPSliderUpdate(hpSlider, monsterStats.currentHP, monsterStats.maxHP);
        StartCoroutine(DamageProcess()); // 피격 상태를 처리하기 위한 코루틴
    }

    // 데미지 처리용 코루틴 함수 (피격 모션이 이뤄질 시간이 경과 -> 현재 상태를 다시 이동 상태로 전환)
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간 만큼 기다린다.
        yield return new WaitForSeconds(0.5f);
        
        // 현재 상태를 이동 상태로 전환
        m_State = EnemyState.Chase;
        print("상태 전환: Damaged -> Move");
    }

    void Die()
    {
        // 진행 중인 피격 코루틴 중지
        StopAllCoroutines();
        
        // 죽음 상태를 처리하기 위한 코루틴 
        StartCoroutine(DieProcess());
    }

    IEnumerator DieProcess()
    {
        ItemDrop();
        HPSliderUpdate(hpSlider, monsterStats.currentHP, monsterStats.maxHP);
        
        monsterStats.currentHP = monsterStats.maxHP; // hp 초기화
        cc.enabled = false; // 캐릭터 컨트롤러 비활성화
        
        yield return new WaitForSeconds(dieDelay); // 사망 후 일정 시간 대기
        
        gameObject.SetActive(false);
        cc.enabled = true;
        EnemyManager.Instance.enemyObjectPool.Add(gameObject); // 오브젝트 풀로 반환
    }
    
    void HPSliderUpdate(Slider hpSlider, int currentHP, int maxHP)
    {
        Utilities.HPSliderUpdate(hpSlider, monsterStats.currentHP, monsterStats.maxHP);
    }
}
