using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public bool AutoModeActive;

    // 범위 내에 있으면, 자동 공격부터 구현!
    // 필요 변수: Detect 거리
    // 필요 변수: 공격 가능 범위
    public void AutoModeOn()
    {
        // 1. 거리 측정 + 이동
        
        
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
