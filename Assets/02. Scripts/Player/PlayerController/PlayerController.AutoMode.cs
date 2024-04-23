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
        /*
        if (!isFighting)
        {
            MoveTowardsNearestEnemy();
        }

        else
            return;
            */
    }
    
    public IEnumerator AutoModeDetectMonstersPeriodically()
    {
        //if (!isMoving)
        {
            MoveTowardsNearestEnemy();
            yield return new WaitForSeconds(1f);
        }
    }

    public void MoveTowardsNearestEnemy()
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
        else
        {
            anim.SetBool("isMove", false);
        }
    }

    public IEnumerator DetectMonstersPeriodically()
    {
        AutoDetect();
        yield return new WaitForSeconds(detectPeriod);
    }

    public void AutoDetect()
    {
        // 스킬 몬스터 목록 (더 넓음)
        List<GameObject> aroundMonsters = GetmonstersInRange(skillMonsterMaxCount);
        isMonsterDetected = true;
    }

    public void AutoAttack()
    {
        if (Time.time >= lastHitTime + hitCooldown)
        {
            List<GameObject> skillMonsters = GetmonstersInRange(skillMonsterMaxCount);
            List<GameObject> attackMonsters = GetMonstersInFront(attackMonsterMaxCount);

            // 스킬 쿨타임이 아니고, 스킬 사용 가능한 몬스터가 있다면
            if (!isSkillOnCooldown && skillMonsters.Count > 0)
            {
                isMonsterDetected = true;

                Debug.Log("[AutoMode]1. Skill");
                // 스킬 공격 or 거리 확인 및 이동
                if (CheckDistance(skillMonsters))
                {
                    GameObject nearestMonster = skillMonsters
                        .OrderBy(m => (m.transform.position - transform.position).sqrMagnitude)
                        .FirstOrDefault();
                    FlipTowardsNearestMonster(nearestMonster);
                    isArrived = true;

                    PlayerSkill(skillMonsters);
                    isMonsterDetected = false;
                }
                else
                {
                    Debug.Log("[AutoMode] Moving to skill target");
                    return;
                }
            }
            else if (attackMonsters.Count > 0) // 스킬을 사용할 수 없다면 기본 공격 수행
            {
                Debug.Log("[AutoMode]2. Attack");
                // 기본 공격 or 거리 확인 및 이동
                if (CheckDistance(attackMonsters))
                {
                    GameObject nearestMonster = attackMonsters
                        .OrderBy(m => (m.transform.position - transform.position).sqrMagnitude)
                        .FirstOrDefault();
                    FlipTowardsNearestMonster(nearestMonster);
                    isArrived = true;

                    PlayerAttack(attackMonsters);
                    isMonsterDetected = false;
                }
            }
            else
            {
                Debug.Log("[AutoMode]No monsters within range");
                return;
            }
            isArrived = false;
        }
    }

    private void FlipTowardsNearestMonster(GameObject nearestMonster)
    {
        if (nearestMonster != null)
        {
            // 몬스터의 위치를 기반으로 방향을 결정하고 뒤집기
            float direction = Mathf.Sign(nearestMonster.transform.position.x - transform.position.x);
            FlipPlayer(direction);
        }
    }

    private bool CheckDistance(List<GameObject> monsters)
    {
        // 가장 가까운 몬스터와의 거리 측정
        GameObject nearestMonster = monsters
            .OrderBy(m => (m.transform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();
        if (nearestMonster == null)
        {
            Debug.Log("[AutoMode] No nearest monster found");
            return false;
        }

        float distance = Vector3.Distance(transform.position, nearestMonster.transform.position);
        Debug.Log($"[CheckDistance] Nearest Monster Distance: {distance}");

        if (distance > detectionRadius / 2)
        {
            // 목표까지 이동
            MoveTowardsTarget(nearestMonster.transform.position);
            return false; // 아직 충분히 가깝지 않음
        }

        return true; // 공격
    }

    private void MoveTowardsTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        rigid.MovePosition(transform.position + direction * playerStats.movement_Speed * Time.deltaTime);
        anim.SetBool("isMove", true); // 애니메이션 변경
        FlipPlayer(direction.x); // 방향에 따라 플레이어 방향 전환
        Invoke("AutoModeOn", 0.1f); // 자동 모드 다시 실행 (=몬스터 다시 검색)
    }
}