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
    
    /// <summary>
    /// 숫자를 읽기 쉬운 단위로 포맷 (K, M, G, T, P, E, Z, Y)
    /// </summary>
    /// <param name="number">포맷할 숫자</param>
    /// <returns>
    /// 1000 미만일 경우 숫자를 그대로 반환
    /// 1000 이상일 경우, 적절한 단위(1000dml 거듭제곱)로 포맷하여 문자열로 반환
    /// </returns>
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
    
    /// <summary>
    /// 분을 초로 변환
    /// </summary>
    /// <param name="minutes">분</param>
    /// <returns>초로 변환된 값</returns>
    public static float MinutesToSeconds(float minutes)
    {
        return minutes * 60f;
    }
}
