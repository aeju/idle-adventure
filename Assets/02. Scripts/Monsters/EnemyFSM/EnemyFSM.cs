using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

// 필요 1: Idle상태 -> 주변 배회
// 필요 2: damaged 애니메이션
public partial class EnemyFSM : MonoBehaviour
{
    public MonsterStats monsterStats;
    
    protected UserInfoManager userInfo;
    protected ResourceManager resourceInfo;
    
    public GameObject hudDamageText;

    // 에너미 상태 상수
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }
    
    // 에너미 상태 변수
    private EnemyState m_State;
    private Rigidbody rigid;
    
    // 플레이어 발견 범위
    public float findDistance = 8f;
    // 공격 가능 범위
    public float attackDistance = 2f;
    
    // 캐릭터 컨트롤러 컴포넌트
    private CharacterController cc;

    // 플레이어 
    public PlayerController target;
    
    // 일정한 시간 간격으로 공격 -> 누적 시간, 공격 딜레이 시간
    private float currentTime = 0; // 누적 시간
    private float attackDelay = 2f; // 공격 딜레이 시간
    
    public bool flipX;

    // 초기 위치 저장용 변수
    private Vector3 originPos;
    // 이동 가능 범위
    public float moveDistance = 20f;
    
    public Slider hpSlider;
    
    // 애니메이션 
    private Animator anim;
    private SkeletonMecanim skeletonMecanim;
    
    public GameObject dropItem;

    void Start()
    {
        m_State = EnemyState.Idle; // 최초의 에너미 상태 : Idle
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("player");
        if (playerObject != null)
        {
            target = playerObject.GetComponent<PlayerController>();
        }
        
        // 캐릭터 컨트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        skeletonMecanim = GetComponent<SkeletonMecanim>();
        
        userInfo = UserInfoManager.Instance;
        resourceInfo = ResourceManager.Instance;
        
        originPos = transform.position; // 자신의 초기 위치 저장
        
        monsterStats.currentHP = monsterStats.maxHP;  // 현재 체력 = 최대 체력으로 초기화
        HPSliderUpdate();
    }

    void Update()
    {
        if (target == null)
        {
            m_State = EnemyState.Idle;
            return;
        }
        
        // 현재 상태를 체크해 해당 상태별로 정해진 기능을 수행하게 함
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged: // 매 프레임마다 반복x, 1회만 실행
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

    void Idle()
    {
        // 만일, 플레이어와의 거리가 액션 시작 범위 이내라면 Move 상태로 전환
        if (Vector3.Distance(transform.position, target.transform.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환: Idle -> Move");
            
            // 이동 애니메이션으로 전환
            anim.SetTrigger("IdleToMove");
        }
    }

    // 1) 공격 범위 안에 들어오지 않았을 때: 이동
    // 2) 공격 범위 안에 들어왔을 때: 상태 전환(공격)
    void Move()
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
            if (target.currentHP > 0)
            {
                // 일정한 시간마다 플레이어를 공격
                currentTime += Time.deltaTime; // 경과 시간 누적
                if (currentTime > attackDelay) // 경과 시간 > 공격 딜레이 시간
                {
                    print("공격, PlayerHP: " + target.GetComponent<PlayerController>().currentHP);
                    currentTime = 0; // 경과 시간 초기화
                    anim.SetTrigger("StartAttack"); // 공격 애니메이션 플레이
                }
            }
        }
        
        // 그렇지 않다면, 현재 상태를 이동(Move)으로 전환(재추격 실시)
        // 공격 중이라도, 플레이어가 공격 범위를 넘어가면 이동 상태로 변환 (경과 시간 초기화!)
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환: Attack -> Move");
            currentTime = 0;
            
            anim.SetTrigger("AttackToMove");
        }
    }
    
    // 플레이어의 스크립트의 데미지 처리 함수 실행
    public void AttackAction()
    {
        target.GetComponent<PlayerController>().PlayerDamaged(monsterStats.attack);
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
        HPSliderUpdate();
        StartCoroutine(DamageProcess()); // 피격 상태를 처리하기 위한 코루틴
    }

    // 데미지 처리용 코루틴 함수 (피격 모션이 이뤄질 시간이 경과 -> 현재 상태를 다시 이동 상태로 전환)
    IEnumerator DamageProcess()
    {
        // 피격 모션 시간 만큼 기다린다.
        yield return new WaitForSeconds(0.5f);
        
        // 현재 상태를 이동 상태로 전환
        m_State = EnemyState.Move;
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
        HPSliderUpdate();
        
        // 캐릭터 컨트롤러 컴포넌트를 비활성화
        cc.enabled = false;
        
        // 2초 동안 기다린 후, 자기 자신을 제거 (나중에 손 봐야함!)
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}