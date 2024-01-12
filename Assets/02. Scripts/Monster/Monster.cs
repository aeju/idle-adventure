using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 수정1 : Vector2 -> Vector3로 변경
// 수정2 : 플레이어 앞에서 따라가기 멈추기
// 최소 거리도 필요? 
public class Monster : MonoBehaviour
{
    public float speed;
    public GameObject player;

    private bool isLive = true;

    private Rigidbody rigid;
    
    public float followDistance = 8.0f; // 플레이어를 따라갈 최대 거리 
    public float stopDistance = 1.0f; // 목표 지점에 도달했을 때 멈출 거리 
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    
    void OnEnable() // 스크립트가 활성화 될 때, 호출되는 이벤트 함수 
    {
        player = GameObject.FindGameObjectWithTag("player"); // awake에서 작성해야? 
    }

    // 플레이어의 현재 위치를 계속 추적 + 해당 위치로 몬스터가 이동
    void FixedUpdate()
    {
        if (!isLive || player == null)
            return;
        
        // 방향 계산 (플레이어 위치 - 나의 위치)
        Vector3 dirVec = player.transform.position - transform.position;
        
        // 플레이어와의 거리가 followDistance 이내이면 몬스터가 이동
        if (dirVec.magnitude < followDistance)
        {
            // 목표 지점에 너무 가까우면 이동 멈춤
            if (dirVec.magnitude > stopDistance)
            {
                Vector3 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
                rigid.MovePosition(rigid.position + nextVec);
            }
            else
            {
                rigid.velocity = Vector3.zero;
            }
        }
        else
        {
            rigid.velocity = Vector3.zero; // 플레이어가 최대 거리 밖에 있으면 이동 멈춤
        }
        
        /*
        // 목표 지점에 가까워지면 이동 멈춤
        if (dirVec.magnitude < stopDistance)
        {
            rigid.velocity = Vector3.zero;
            //return;
        }
        
        // 플레이어와의 거리가 followDistance 이내 -> 몬스터가 이동
        else if (dirVec.magnitude < followDistance)
        {
            Vector3 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }

        else
        {
            rigid.velocity = Vector3.zero; // 최대 거리 밖 : 이동 멈추기 
        }
        */
    }

    void LateUpdate()
    {
        if (!isLive || player == null)
            return;
        
        // 목표의 x축 값과 자신의 x축 값을 비교하여 작으면 true가 되도록 설정
        if (player.transform.position.x < transform.position.x)
            FlipX();
    }
    
    void FlipX()
    {
        // 플레이어가 몬스터의 왼쪽에 있고, 몬스터가 오른쪽을 바라보고 있는 경우
        if (player.transform.position.x < transform.position.x && transform.localScale.x > 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1; // X축 스케일을 반전
            transform.localScale = localScale;
        }
        // 플레이어가 몬스터의 오른쪽에 있고, 몬스터가 왼쪽을 바라보고 있는 경우
        else if (player.transform.position.x > transform.position.x && transform.localScale.x < 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1; // X축 스케일을 반전
            transform.localScale = localScale;
        }
        
        /*
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // X축 스케일을 반전
        transform.localScale = localScale;
        */
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followDistance);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
