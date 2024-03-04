using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Utilities
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
    
    // K(Kilo 킬로) : 1,000
    // M(Mega 메가) : 1,000,000
    // G(Giga 기가) : 1,000,000,000
    // T(Tera 테라) : 1,000,000,000,000
    // P(Peta 페타) : 1,000,000,000,000,000
    // E(Exa 엑사) : 1,000,000,000,000,000,000
    // Z(Zetta 제타) : 1,000,000,000,000,000,000,000
    // Y(Yotta 요타) : 1,000,000,000,000,000,000,000,000
    // 각 단위 : 1000의 거듭제곱
    // 코인 단위 축약 : 4자리 미만 -> 축약할 필요 x
    public static string FormatNumberUnit(int number)
    {
        if (number >= 1000)
        {
            return (number / 1000f).ToString("0.#") + "K"; // 킬로
        }

        else if (number >= 1000000)
        {
            return (number / 1000000f).ToString("0.#") + "M"; // 메가
        }

        else if (number >= 1000000000)
        {
            return (number / 1000000000f).ToString("0.#") + "G"; // 기가
        }

        else if (number >= 1000000000000)
        {
            return (number / 1000000000000f).ToString("0.#") + "T"; // 테라
        }

        else if (number >= 1000000000000000)
        {
            return (number / 1000000000000000f).ToString("0.#") + "P"; // 페타
        }

        else if (number >= 1000000000000000000)
        {
            return (number / 1000000000000000000f).ToString("0.#") + "E"; // 엑사
        }

        else
        {
            return number.ToString(); // 1000이하 : 축약 x
        }
    }
}
