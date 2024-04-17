using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    // 우선 사용 : Skill
    public void AutoModeOn()
    {
        if (Time.time >= lastHitTime + hitCooldown)
        {
            List<GameObject> skillMonsters = GetmonstersInRange(skillMonsterMaxCount);
            List<GameObject> attackMonsters = GetMonstersInFront(attackMonsterMaxCount);

            // 스킬 쿨타임이 아니고, 스킬 사용 가능한 몬스터가 있다면
            if (!isSkillOnCooldown && skillMonsters.Count > 0)
            {
                Debug.Log("[AutoMode]1. Skill");
                // 스킬 공격 or 거리 확인 및 이동
                if (CheckDistance(skillMonsters))
                {
                    PlayerSkill(skillMonsters);
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
                    PlayerAttack(attackMonsters);
                    Debug.Log("[AutoMode]Using Basic Attack");
                }
            }
            else
            {
                Debug.Log("[AutoMode]No monsters within range");
                return;
            }
        }
    }
    
    private bool CheckDistance(List<GameObject> monsters)
    {
        // 가장 가까운 몬스터와의 거리 측정
        GameObject nearestMonster = monsters.OrderBy(m => (m.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
        if (nearestMonster == null)
        {
            Debug.Log("[AutoMode] No nearest monster found");
            return false;
        }
        
        float distance = Vector3.Distance(transform.position, nearestMonster.transform.position);
        Debug.Log($"[CheckDistance] Nearest Monster Distance: {distance}");
        
        if (distance > 2f)
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
        FlipPlayer(direction.x); // 방향에 따라 플레이어 방향 전환
        Invoke("AutoModeOn", 0.1f); // 자동 모드 다시 실행 (=몬스터 다시 검색)
    }
}
