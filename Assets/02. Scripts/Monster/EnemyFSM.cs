using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 필요 1: Idle상태 -> 주변 배회
// 필요 2: Return상태 -> Player 근처 -> 다시 추적 상태
// 필요 3: 방향에 따라 이미지 뒤집기
public class EnemyFSM : MonoBehaviour
{
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
    
    // 이동 속도
    public float moveSpeed = 5f;
    
    // 캐릭터 컨트롤러 컴포넌트
    private CharacterController cc;
    
    // 플레이어 위치
    public Transform player;
    
    // 일정한 시간 간격으로 공격 -> 누적 시간, 공격 딜레이 시간
    private float currentTime = 0; // 누적 시간
    private float attackDelay = 2f; // 공격 딜레이 시간
    
    // 공격력
    public int attackPower = 3;
    
    // 초기 위치 저장용 변수
    private Vector3 originPos;
    // 이동 가능 범위
    public float moveDistance = 20f;
    
    // 체력
    public int currentHP = 15;

    void Start()
    {
        m_State = EnemyState.Idle; // 최초의 에너미 상태 : Idle
        
        // 플레이어의 트랜스폼 컴포넌트 받아오기
        player = GameObject.FindGameObjectWithTag("player").transform;

        // 캐릭터 컨트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();
        
        // 자신의 초기 위치 저장하기
        originPos = transform.position;
    }

    void Update()
    {
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
            case EnemyState.Damaged:
                // Damaged(); 매 프레임마다 반복x, 1회만 실행
                break;
            case EnemyState.Die:
                // Die();
                break;
        }
    }

    void Idle()
    {
        // 만일, 플레이어와의 거리가 액션 시작 범위 이내라면 Move 상태로 전환
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        //if (Vector3.Distance(transform.position, player.transform.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환: Idle -> Move");
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
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {
            // 이동 방향 설정
            Vector3 dir = (player.position - transform.position).normalized;

            // 이동
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        
        // 그렇지 않다면, 현재 상태를 공격으로 전환
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");
            
            // 누적 시간을 공격 딜레이 시간만큼 미리 진행시켜 놓기 (공격 상태로 전환됐을 때 기다렸다가 공격 시작하는 문제)
            currentTime = attackDelay;
        }
    }

    // 1) 플레이어가 공격 범위 안에 있을 때: 공격
    // 2) 플레이어가 공격 범위 밖에 있을 때: 상태 전환(이동)
    void Attack()
    {
        // 만일, 플레이어가 공격 범위 이내에 있다면 플레이어를 공격
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            // 일정한 시간마다 플레이어를 공격
            currentTime += Time.deltaTime; // 경과 시간 누적
            if (currentTime > attackDelay) // 경과 시간 > 공격 딜레이 시간
            {
                player.GetComponent<PlayerController>().PlayerDamaged(attackPower);
                print("공격, PlayerHP: " + player.GetComponent<PlayerController>().currentHP);
                currentTime = 0; // 경과 시간 초기화
            }
        }
        
        // 그렇지 않다면, 현재 상태를 이동(Move)으로 전환(재추격 실시)
        // 공격 중이라도, 플레이어가 공격 범위를 넘어가면 이동 상태로 변환 (경과 시간 초기화!)
        else
        {
            m_State = EnemyState.Move;
            print("상태 전환: Attack -> Move");
            currentTime = 0;
        }
    }

    void Return()
    {
        // 만일 초기 위치에서 거리가 0.1f이상이라면, 초기 위치 쪽으로 이동
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
            Vector3 dir = (originPos - transform.position).normalized;
            cc.Move(dir * moveSpeed * Time.deltaTime);
        }
        // 그렇지 않다면, 자신의 위치를 초기 위치로 조정 + 현재 상태를 대기로 전환
        else
        {
            transform.position = originPos;
            m_State = EnemyState.Idle;
            print("상태 전환: Return -> Idle");
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
        currentHP -= hitPower;
        
        // 에너미의 체력이 0보다 크면 피격 상태로 전환
        if (currentHP > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any state -> Damaged");
            Damaged();;
        }
        
        // 그렇지 않다면 죽음 상태로 전환
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any state -> Die");
            Die();
        }
    }
    
    void Damaged()
    {
        // 피격 상태를 처리하기 위한 코루틴 실행
        StartCoroutine(DamageProcess());
    }

    // 데미지 처리용 코루틴 함수
    // 피격 모션이 이뤄질 시간이 경과 -> 현재 상태를 다시 이동 상태로 전환
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
        // 캐릭터 컨트롤러 컴포넌트를 비활성화
        cc.enabled = false;
        
        // 2초 동안 기다린 후, 자기 자신을 제거 (나중에 손 봐야함!)
        yield return new WaitForSeconds(2f);
        print("Die");
        Destroy(gameObject);
    }
}
