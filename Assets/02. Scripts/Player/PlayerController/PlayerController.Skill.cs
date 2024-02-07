using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerController.Skill
// Attack, Skill 로직
public partial class PlayerController : MonoBehaviour
{
    // 일반 공격 (z)
    public void PlayerAttack()
    {
        anim.SetTrigger("AttackTrigger");
    }

    // 치명타 공격 (x, 쿨타임 o)
    public void PlayerSkill()
    {
        anim.SetTrigger("SkillTrigger");
        
        if (!isSkillOnCooldown)
        {
            StartCoroutine(SkillCoroutine());
        }
        else
        {
            float remainCooldown = (lastSkillTime + skillCooldown) - Time.time;
            Debug.Log($"남은 시간: {remainCooldown}");
        }
    }
    
    public IEnumerator SkillCoroutine()
    {
        isSkillOnCooldown = true;
        lastSkillTime = Time.time;

        // 스킬 쿨타임 슬라이더
        {
            if (cooldownSlider != null) // 슬라이더 초기화 
            {
                cooldownSlider.value = 0;
                cooldownSlider.maxValue = skillCooldown;
            }

            // 쿨타임 동안 슬라이더 업데이트
            while (Time.time < lastSkillTime + skillCooldown)
            {
                if (cooldownSlider != null)
                {
                    cooldownSlider.value = Time.time - lastSkillTime;
                }
                yield return null; // 다음 프레임까지 기다리도록 보장
            }
            isSkillOnCooldown = false;

            if (cooldownSlider != null)
            {
                cooldownSlider.value = cooldownSlider.maxValue;
            }
        }
    }
}
