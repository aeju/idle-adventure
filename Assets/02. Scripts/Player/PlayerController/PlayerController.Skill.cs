using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerController.Skill
// Attack, Skill 로직
public partial class PlayerController : MonoBehaviour
{
    [Header("# 스킬 쿨다운")]
    // 쿨타임
    public float skillCooldown = 3f; 
    public float lastSkillTime; // start에서 초기화
    public bool isSkillOnCooldown = false; // 스킬이 쿨다운 중인지 확인 (false -> 스킬 실행 o)

    [Header("# 스킬 시간 간격")]
    // 자동공격 시간 간격
    public float hitCooldown = 2f;
    public float lastHitTime;

    // 일반 공격 (z)
    public void PlayerAttack()
    {
        if (Time.time >= lastHitTime + hitCooldown)
        {
            anim.SetTrigger("AttackTrigger");
            lastHitTime = Time.time; // 공격 쿨타임 업데이트
        }
    }

    // 치명타 공격 (x, 쿨타임 o)
    public void PlayerSkill()
    {
        if (!isSkillOnCooldown && Time.time >= lastHitTime + hitCooldown) // 쿨다운 false = 스킬 실행 o
        {
            anim.SetTrigger("SkillTrigger");
            StartCoroutine(SkillCoroutine());
            lastHitTime = Time.time; // 공격 쿨타임 업데이트
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
        lastSkillTime = Time.time; // 현재 시간

        // 스킬 쿨타임 슬라이더
        {
            // 슬라이더 초기화 
            if (cooldownSlider != null) 
            {
                cooldownSlider.value = 0;
                cooldownSlider.maxValue = skillCooldown;
            }

            // 쿨타임 동안 슬라이더 업데이트 (lastSkillTime + skillCooldown)
            while (Time.time < lastSkillTime + skillCooldown)
            {
                if (cooldownSlider != null)
                {
                    cooldownSlider.value = Time.time - lastSkillTime;
                }
                yield return null; // 다음 프레임까지 기다리도록 보장
            }
            isSkillOnCooldown = false; // 쿨타임 종료 -> 스킬 다시 사용 가능

            if (cooldownSlider != null)
            {
                cooldownSlider.value = cooldownSlider.maxValue;
            }
        }
    }
}
