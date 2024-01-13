using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    // 에너미 상태 상수
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
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

    void Start()
    {
        m_State = EnemyState.Idle; // 최초의 에너미 상태 : Idle
        
        // 플레이어의 트랜스폼 컴포넌트 받아오기
        player = GameObject.FindGameObjectWithTag("player").transform;

        // 캐릭터 컨트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();
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
            case EnemyState.Damaged:
                Damaged();
                break;
            case EnemyState.Die:
                Die();
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
        // 만일, 플레이어와의 거리가 공격 범위 밖이라면, 플레이어를 향해 이동
        if (Vector3.Distance(transform.position, player.position) > attackDistance)
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

    void Damaged()
    {
        
    }

    void Die()
    {
        
    }
}
