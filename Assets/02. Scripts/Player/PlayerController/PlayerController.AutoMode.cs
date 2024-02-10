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
        // 필요 변수: 스킬 간격 시간 <= 현재 시간
        // 필요 플래그: 스킬 사용 가능 -> 2초! 
        if (nearestMonster != null)
        {
            Debug.Log(isSkillOnCooldown);

            //if (!isSkillOnCooldown)
            if (!isSkillOnCooldown && Time.time >= lastHitTime + hitCooldown)
            {
                PlayerSkill();
                Debug.Log("skill " + isSkillOnCooldown);
            }
            
            else if (Time.time >= lastHitTime + hitCooldown)
            {
                PlayerAttack();
                Debug.Log("attack" + isSkillOnCooldown);
            }
        }
    }
}
