using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public bool AutoModeActive;

    // 자동 공격부터 구현!
    public void AutoModeOn()
    {
        // 1. 거리 측정 + 이동 (작업 중)

        // 2. 자동 공격 (우선순위: skill > attack) 
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
