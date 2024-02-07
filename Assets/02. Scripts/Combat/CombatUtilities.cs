using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CombatUtilities
{
    /// <summary>
    /// 현재 플레이어 hp(%)를 hp 슬라이더의 value에 반영
    /// </summary>
    public static void HPSliderUpdate(Slider hpSlider, int currentHP, int maxHP)
    {
        if (hpSlider != null)
        {
            hpSlider.value = (float)currentHP / (float)maxHP;
        }
    }
}
