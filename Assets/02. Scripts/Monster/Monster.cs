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
    private bool isChasing = false; // 현재 플레이어를 추적 중인지 여부 

    private Rigidbody rigid;
    
    public float followDistance = 8.0f; // 플레이어를 따라갈 최대 거리 
    public float stopDistance = 1.0f; // 목표 지점에 도달했을 때 멈출 거리 
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    
    void OnEnable() // 스크립트가 활성화 될 때, 호출되는 이벤트 함수 
    {
        player = GameObject.FindGameObjectWithTag("player"); 
        
        StartCoroutine(CheckDistanceCoroutine());
    }
    
    IEnumerator CheckDistanceCoroutine()
    {
        while (isLive && player != null)
        {
            Vector3 dirVec = player.transform.position - transform.position;
            float distanceToPlayer = dirVec.magnitude;
            Debug.Log($"Distance to Player: {distanceToPlayer}");

            if (distanceToPlayer < followDistance && distanceToPlayer > stopDistance && !isChasing)
            {
                // 플레이어가 다시 추적 거리 내에 들어오면 추적 시작
                isChasing = true;
                Debug.Log("Resuming Chase");
            }
            else if (distanceToPlayer <= stopDistance && isChasing)
            {
                // 플레이어가 멈추는 거리 이내로 접근하면 추적 멈춤
                isChasing = false;
                Debug.Log("Reached Player, Stopping");
            }
            else if (distanceToPlayer > followDistance && isChasing)
            {
                // 플레이어가 추적 범위를 벗어나면 추적 멈춤
                isChasing = false;
                Debug.Log("Player Out of Range, Stopping");
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    // 플레이어의 현재 위치를 계속 추적 + 해당 위치로 몬스터가 이동
    void FixedUpdate()
    {
        if (!isLive || player == null)
            return;

        // 플레이어와의 거리 계산
        Vector3 dirVec = player.transform.position - transform.position;
        float distanceToPlayer = dirVec.magnitude;

        // 추적 상태일 때만 플레이어를 따라 이동
        if (isChasing)
        {
            // 플레이어가 멈추는 거리 이내로 들어오면 이동 멈춤
            if (distanceToPlayer <= stopDistance)
            {
                rigid.velocity = Vector3.zero;
            }
            else
            {
                // 플레이어를 향해 이동
                Vector3 moveDirection = dirVec.normalized;
                rigid.MovePosition(rigid.position + moveDirection * speed * Time.fixedDeltaTime);
            }
        }
        /*
        if (!isLive || player == null)
            return;
        
        // 방향 계산 (플레이어 위치 - 나의 위치)
        Vector3 dirVec = player.transform.position - transform.position;
        
        float distanceToPlayer = dirVec.magnitude;
        // 재추적 상태 및 플레이어와의 거리 로그 출력
        if (distanceToPlayer < followDistance)
        {
            if (distanceToPlayer > stopDistance)
            {
                Vector3 moveDirection = dirVec.normalized;
                rigid.MovePosition(rigid.position + moveDirection * speed * Time.fixedDeltaTime);
                if (!isChasing)
                {
                    isChasing = true;
                    Debug.Log("Resuming Chase");
                }
            }
            else
            {
                if (isChasing)
                {
                    rigid.velocity = Vector3.zero;
                    isChasing = false;
                    Debug.Log("Reached Player, Stopping");
                }
            }
        }
        else
        {
            if (isChasing)
            {
                rigid.velocity = Vector3.zero;
                isChasing = false;
                Debug.Log("Player Out of Range, Stopping");
            }
        }
        
        Debug.Log($"Is Chasing: {isChasing}, Distance to Player: {distanceToPlayer}");
        */

        
        /*
        if (distanceToPlayer < followDistance)
        {
            if (distanceToPlayer > stopDistance)
            {
                Vector3 moveDirection = dirVec.normalized;
                rigid.MovePosition(rigid.position + moveDirection * speed * Time.fixedDeltaTime);
                if (!isChasing)
                {
                    isChasing = true;
                    Debug.Log("Resuming Chase");
                }
            }
            else
            {
                if (isChasing)
                {
                    rigid.velocity = Vector3.zero;
                    isChasing = false;
                    Debug.Log("Reached Player, Stopping");
                }
            }
        }
        else
        {
            if (isChasing)
            {
                rigid.velocity = Vector3.zero;
                isChasing = false;
                Debug.Log("Player Out of Range, Stopping");
            }
        }
        */

        /*
        if (distanceToPlayer < followDistance)
        {
            if (distanceToPlayer > stopDistance)
            {
                // 플레이어를 향해 이동
                Vector3 moveDirection = dirVec.normalized;
                rigid.MovePosition(rigid.position + moveDirection * speed * Time.fixedDeltaTime);
                isChasing = true;
                Debug.Log("Chasing Player");
            }
            else if (isChasing)
            {
                // stopDistance 내에 들어오면 추적 멈춤
                rigid.velocity = Vector3.zero;
                isChasing = false;
                Debug.Log("Reached Player, Stopping");
            }
        }
        else if (isChasing)
        {
            // followDistance 밖으로 나가면 추적 멈춤
            rigid.velocity = Vector3.zero;
            isChasing = false;
            Debug.Log("Player Out of Range, Stopping");
        }
        */
        
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
        //if (player.transform.position.x < transform.position.x)
        
        // 플레이어의 위치에 따라 몬스터의 방향 전환
        // 플레이어가 몬스터의 왼쪽에 있고, 몬스터가 오른쪽을 바라보고 있는 경우
        // 플레이어가 몬스터의 오른쪽에 있고, 몬스터가 왼쪽을 바라보고 있는 경우
        if (player.transform.position.x < transform.position.x && transform.localScale.x > 0 || 
            player.transform.position.x > transform.position.x && transform.localScale.x < 0)
        {
            FlipX();
        }
    }
    
    void FlipX()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // X축 스케일을 반전
        transform.localScale = localScale;
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, followDistance);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}
