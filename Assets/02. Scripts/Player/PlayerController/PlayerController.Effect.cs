using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [Header("# 공격 이펙트")]
    public GameObject attackEffect;
    public GameObject skillEffect;
    public float effectDuration = 1.5f; // 이펙트 비활성화 시간
    
    /// <summary>
    /// 제일 처음, 공격 이펙트를 꺼주고 시작
    /// </summary>
    private void DeactivateEffects()
    {
        attackEffect.SetActive(false);
        skillEffect.SetActive(false);
    }
    
    // 이펙트 종류, 코루틴을 파라미터로 받는 공통 함수 (일정 시간 후 비활성화)
    private void ActivateEffect(GameObject effect, float duration)
    {
        if (effect != null)
        {
            effect.SetActive(true);
            StartCoroutine(DeactivateEffect(effect, duration));
        }
    }
    
    // 지정된 시간이 지난 후 스킬 이펙트를 비활성화하는 코루틴
    private IEnumerator DeactivateEffect(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (effect != null)
        {
            effect.SetActive(false);
        }
    }
    
    // 기본 공격(z) 이펙트
    private void CreateAttackEffect()
    {
        ActivateEffect(attackEffect, effectDuration); 
    }

    // 스킬 공격(x) 이펙트
    private void CreateSkillEffect()
    {
        ActivateEffect(skillEffect, effectDuration); 
    }
}
