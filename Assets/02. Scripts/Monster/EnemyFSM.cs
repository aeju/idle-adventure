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
    //public GameObject player;

    void Start()
    {
        m_State = EnemyState.Idle; // 최초의 에너미 상태 : Idle
        
        // 플레이어의 트랜스폼 컴포넌트 받아오기
        player = GameObject.FindGameObjectWithTag("player").transform; 
        //player = GameObject.FindGameObjectWithTag("player"); 
        //player = GameObject.Find("Player").transform;
        
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

    // 공격 범위 안에 들어왔을 때: 상태 전환
    // 공격 범위 안에 들어오지 않았을 때: 이동
    void Move()
    {
        // 만일, 플레이어와의 거리가 공격 범위 밖이라면, 플레이어를 향해 이동
        if (Vector3.Distance(transform.position, player.position) > attackDistance)
        //if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
        {
            // 이동 방향 설정
            Vector3 dir = (player.position - transform.position).normalized;

            cc.Move(dir * moveSpeed * Time.deltaTime);
            
            //Vector3 dir = (player.transform.position - transform.position).normalized;

            //rigid.MovePosition(rigid.position + moveDirection * speed * Time.fixedDeltaTime);
            // 이동
            //rigid.MovePosition(rigid.position + dir * moveSpeed * Time.fixedDeltaTime);
            //rigid.MovePosition(rigid.position + dir * moveSpeed * Time.deltaTime);
            //rigid.MovePosition(rigid.position + dir * moveSpeed * Time.deltaTime);

        }
        
        // 그렇지 않다면, 현재 상태를 공격으로 전환
        else
        {
            m_State = EnemyState.Attack;
            print("상태 전환: Move -> Attack");
        }
    }

    void Attack()
    {
        
    }

    void Damaged()
    {
        
    }

    void Die()
    {
        
    }
}
