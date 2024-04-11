using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public bool AutoModeActive;
    public float autoAttackRange = 0.1f; // 자동 공격 범위
    
    // 자동 공격부터 구현!
    public void AutoModeOn()
    {
        // 1. 거리 측정 + 이동 (작업 중)
        MoveTowardsNearestEnemy(); // 가장 가까운 적에게 이동
        
        
    }

    private void MoveTowardsNearestEnemy()
    {
        List<Point> nearbyEnemies = QuadtreeManager.Instance.QueryNearbyEnemies(transform.position, searchRadius);

        if (nearbyEnemies.Count > 0)
        {
            Point targetMonster = nearbyEnemies
                .OrderBy(enemy => Vector3.Distance(transform.position, new Vector3(enemy.x, 0, enemy.z)))
                .FirstOrDefault();

            if (targetMonster != null)
            {
                Vector3 targetPosition = new Vector3(targetMonster.x, transform.position.y, targetMonster.z);
                float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

                if (distanceToTarget > autoAttackRange) // 자동 공격 범위보다 멀 때
                {
                    Vector3 moveDirection = (targetPosition - transform.position).normalized * playerStats.movement_Speed;
                    Vector3 newPosition = transform.position + moveDirection * Time.deltaTime; // 이동
                    rigid.MovePosition(newPosition);
                    anim.SetBool("isMove", true);
                }
                else // 자동 공격 범위 안에 들어왔을 때
                {
                    anim.SetBool("isMove", false);
                    AutoAttack(); // 자동 공격
                }
            }
        }
        else
        {
            anim.SetBool("isMove", false);
        }
    }
    
    
    
   // (우선순위: skill > attack) 
    void AutoAttack()
    {
        if (nearestMonster != null && Time.time >= lastHitTime + hitCooldown)
        {
            Debug.Log(isSkillOnCooldown);
            
            if (!isSkillOnCooldown)
            {
                PlayerSkill();
                Debug.Log("skill " + isSkillOnCooldown);
            }
            
            else
            {
                PlayerAttack();
                Debug.Log("attack" + isSkillOnCooldown);
            }
        }
    }
}
