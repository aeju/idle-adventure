using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public float autoAttackRange = 0.1f; // 자동 공격 범위
    public float terrainRadius = 100f;

    // 자동 이동! (자동 공격 : 기본 상태에서도 o) 
    public void AutoModeOn()
    {
        
    }
    
    public IEnumerator AutoModeDetectMonstersPeriodically()
    {
        {
            AutoMoveTowardsNearestEnemy();
            yield return new WaitForSeconds(1f);
        }
    }

    public void AutoMoveTowardsNearestEnemy()
    {
        Debug.Log("[AutoMove]1. 실행");
        isMoving = true;
        List<Point> nearbyEnemies = QuadtreeManager.Instance.QueryNearbyEnemies(transform.position, terrainRadius);

        if (nearbyEnemies.Count > 0)
        {
            Point targetMonster = nearbyEnemies
                .OrderBy(enemy => Vector3.Distance(transform.position, new Vector3(enemy.x, 0, enemy.z)))
                .FirstOrDefault();
            
            Debug.Log("[AutoMove]2. 타겟몬스터 위치" + targetMonster.x + targetMonster.z);

            if (targetMonster != null)
            {
                Vector3 targetPosition = new Vector3(targetMonster.x, transform.position.y, targetMonster.z);
                FlipTowardsNearestMonster(targetPosition.x);
                MoveToTarget(targetPosition);
            }
        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }
    
    private void FlipTowardsNearestMonster(float targetX)
    {
        if ((targetX > transform.position.x && !isFlipX) || (targetX < transform.position.x && isFlipX))
        {
            FlipPlayer(targetX);
        }
    }
    
    private void MoveToTarget(Vector3 targetPosition)
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        
        if (distanceToTarget > autoAttackRange) // 자동 공격 범위보다 멀 때
        {
            Vector3 moveDirection =
                (targetPosition - transform.position).normalized * playerStats.movement_Speed;
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
}