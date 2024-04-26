using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 고친 것 
public partial class PlayerController : MonoBehaviour
{
    [SerializeField] private float autoAttackRange = 2f; // 자동 공격 범위
    [SerializeField] private float terrainRadius = 100f;
    private HashSet<GameObject> monstersInRange = new HashSet<GameObject>();

    // 자동 이동! (자동 공격 : 기본 상태에서도 o) 
    public void AutoModeOn()
    {
        
    }
    
    public IEnumerator AutoModeDetectMonstersPeriodically()
    { 
        AutoMoveTowardsNearestEnemy();
        yield return new WaitForSeconds(1f);
    }
    
    public void AutoMoveTowardsNearestEnemy()
    {
        isMoving = true;
        List<Point> nearbyEnemies = QuadtreeManager.Instance.QueryNearbyEnemies(transform.position, terrainRadius);

        if (nearbyEnemies.Count > 0)
        {
            Point targetMonster = nearbyEnemies
                .OrderBy(enemy => Vector3.Distance(transform.position, new Vector3(enemy.x, 0, enemy.z)))
                .FirstOrDefault();

            // 몬스터 위치 업데이트
            Debug.Log("[AutoMove]2. 타겟몬스터 위치" + targetMonster.x + targetMonster.z);

            if (targetMonster != null)
            {
                Debug.Log("[AutoMove]업데이트 + 2. 타겟몬스터 위치" + targetMonster.x + targetMonster.z);
                UpdateNearestMonsterPosition(targetMonster);
                
                Vector3 targetPosition = new Vector3(targetMonster.x, transform.position.y, targetMonster.z);
                FlipTowardsNearestMonster(targetPosition.x);
                MoveToTarget(targetPosition); // 가까워진 몬스터 있으면, 이동 중단 
            }
        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }
    
    public float flipCooldown = 0.5f; // 뒤집기 쿨다운 시간 (0.5초)
    public float lastFlipTime = 0; // 마지막 뒤집기 시간
    private void FlipTowardsNearestMonster(float targetX)
    {
        // 현재 시간이 마지막 뒤집기 시간 + 쿨다운보다 크거나 같은지 확인
        //if (Time.time < lastFlipTime + flipCooldown)
            //return; // 쿨다운 중이면 실행 취소
        
        if ((targetX > transform.position.x && !isFlipX) || (targetX < transform.position.x && isFlipX))
        {
            FlipPlayer(targetX);
            //lastFlipTime = Time.time; // 마지막 뒤집기 시간 업데이트
        }
    }
    
    private void MoveToTarget(Vector3 initialTargetPosition)
    {
        GameObject nearestMonster = GetNearestMonsterInTriggerRange();
        Vector3 targetPosition = nearestMonster != null ? nearestMonster.transform.position : initialTargetPosition;

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        
        // 공격 범위보다 멀 때 
        if (distanceToTarget > autoAttackRange) // 자동 공격 범위보다 멀 때
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized * playerStats.movement_Speed;
            Vector3 newPosition = transform.position + moveDirection * Time.deltaTime; // 이동
            rigid.MovePosition(newPosition);
            anim.SetBool("isMove", true);
        }
        else // 자동 공격 범위 안에 들어왔을 때
        {
            isMoving = false;
            isArrived = true;
                    
            anim.SetBool("isMove", false);
            AutoAttack(); // 자동 공격
        }
    }
    
    // 트리거 범위 내의 가장 가까운 몬스터를 찾음
    // 하고 싶은 것 : 트리거 범위 내에 몬스터가 없으면, 초기 목표 위치로 계속 이동 
    private GameObject GetNearestMonsterInTriggerRange()
    {
        GameObject nearestMonster = null;
        float minDistance = float.MaxValue;

        foreach (GameObject monster in monstersInRange)
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestMonster = monster;
            }
        }

        return nearestMonster;
    }
    
    // 몬스터가 트리거 범위 내로 들어오면
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster")) 
        {
            monstersInRange.Add(other.gameObject);
            AutoAttackCheck();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster")) 
        {
            monstersInRange.Remove(other.gameObject);
        }
    }

    void AutoAttackCheck()
    {
        foreach (GameObject monster in monstersInRange)
        {
            float distance = Vector3.Distance(transform.position, monster.transform.position);
            if (distance <= autoAttackRange)
            {
                AutoAttack(); // 자동 공격 실행
                break; // 가장 가까운 몬스터에 대해 공격을 실행한 후 중단
            }
        }
    }
    
    public void UpdateNearestMonsterPosition(Point nearestMonsterPoint) 
    {
        GameObject nearestMonster = GameObject.Find(nearestMonsterPoint.monsterName);
        
        if (nearestMonster != null) 
        {
            Vector3 currentMonsterPosition = nearestMonster.transform.position;
            QuadtreeManager.Instance.UpdateMonsterPosition(nearestMonsterPoint.monsterName, currentMonsterPosition);
            MoveTowardsTarget(currentMonsterPosition); // 다시 이동
        }
    }
}