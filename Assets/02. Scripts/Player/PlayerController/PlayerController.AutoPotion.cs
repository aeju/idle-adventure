using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    private PotionManager potionManager;
    private Coroutine autoPotionCoroutine;
    
    // 자동 포션 체크 코루틴 (OnPlayerHPChanged 이벤트 구독)
    private void CheckAutoPotion(int currentHP, int maxHP)
    {
        if (autoPotionCoroutine != null)
        {
            StopCoroutine(autoPotionCoroutine);
        }
        autoPotionCoroutine = StartCoroutine(AutoPotionCheckCoroutine());
    }

    // 주기적으로 자동 포션 사용 조건 확인
    private IEnumerator AutoPotionCheckCoroutine()
    {
        while (true)
        {
            // 설정된 간격만큼 대기
            yield return new WaitForSeconds(potionManager.autoPotionCheckInterval);
            // 설정된 자동 포션 사용 조건 충족 시, 포션 사용
            if (potionManager.CheckAutoPotionConditions(playerStats))
            {
                UsePotion();
            }
        }
    }

    public void UsePotion()
    {
        int beforeHP = playerStats.CurrentHP;
        if (potionManager.UsePotion(playerStats))
        {
            PlayHealEffect();
            Debug.Log($"Potion used. HP: {beforeHP} -> {playerStats.CurrentHP}");
        }
    }
}
