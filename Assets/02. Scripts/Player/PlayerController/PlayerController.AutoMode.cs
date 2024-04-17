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
                // 스킬 공격
                PlayerSkill(skillMonsters);
                Debug.Log("[AutoMode]Using Skill");
            }
            else if (attackMonsters.Count > 0) // 스킬을 사용할 수 없다면 기본 공격 수행
            {
                // 기본 공격
                PlayerAttack(attackMonsters);
                Debug.Log("[AutoMode]Using Basic Attack");
            }
            else
            {
                Debug.Log("[AutoMode]No monsters within range");
            }
        }
    }
    
    
    // 자동 공격부터 구현!
    public void BeforeAutoModeOn()
    {
        // 1. 거리 측정 + 이동 (작업 중)

        // 2. 자동 공격 (우선순위: skill > attack) 
        //if (nearestMonster != null && Time.time >= lastHitTime + hitCooldown)
        if (Time.time >= lastHitTime + hitCooldown)
        {
            Debug.Log(isSkillOnCooldown);
            
            if (!isSkillOnCooldown)
            {
                //PlayerSkill();
                Debug.Log("skill " + isSkillOnCooldown);
            }
            
            else
            {
                //PlayerAttack();
                Debug.Log("attack" + isSkillOnCooldown);
            }
        }
    }
}
