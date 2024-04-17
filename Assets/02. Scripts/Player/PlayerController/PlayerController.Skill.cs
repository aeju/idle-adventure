using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerController.Skill
// Attack, Skill 로직
public partial class PlayerController : MonoBehaviour
{
    [Header("# 스킬 쿨다운")]
    // 쿨타임
    [SerializeField] private float skillCooldown = 3f; 
    [SerializeField] private float lastSkillTime; // start에서 초기화
    [SerializeField] public bool isSkillOnCooldown = false; // 스킬이 쿨다운 중인지 확인 (false -> 스킬 실행 o)

    [Header("# 스킬 시간 간격")]
    // 자동공격 시간 간격
    [SerializeField] private float hitCooldown = 2f;
    [SerializeField] private float lastHitTime;
    
    public List<GameObject> _attackMonsters; // 어택 적용 몬스터 목록 저장
    public List<GameObject> _skillMonsters; // 스킬 적용 몬스터 목록 저장
    
    // 일반 공격 (z)
    public void PlayerAttack(List<GameObject> monsters)
    {
        if (Time.time >= lastHitTime + hitCooldown)
        {
            _attackMonsters = monsters; // 어택 적용 몬스터 목록 저장
            anim.SetTrigger("AttackTrigger");
            lastHitTime = Time.time; // 공격 쿨타임 업데이트
        }
    }
    
    // 치명타 공격 (x, 쿨타임 o)
    public void PlayerSkill(List<GameObject> monsters)
    {
        if (!isSkillOnCooldown && Time.time >= lastHitTime + hitCooldown) // 쿨다운 false = 스킬 실행 o
        {
            _skillMonsters = monsters; // 스킬 적용 몬스터 목록 저장
            anim.SetTrigger("SkillTrigger"); // 애니메이션 트리거
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
