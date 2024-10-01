using System;
using UnityEngine;
using UnityEngine.UI;

// 프로젝트 전반에서 재사용할 수 있는 범용적인 메서드
public static class Utilities
{
    /// <summary>
    /// 현재 플레이어 hp(%)를 hp 슬라이더의 value에 반영
    /// </summary>
    public static void HPSliderUpdate(Slider hpSlider, int currentHP, int maxHP)
    {
        if (hpSlider != null)
        {
            hpSlider.value = (float)currentHP / maxHP;
            // Debug.Log($"HP Updated: {currentHP} / {maxHP}");
        }

        else
        {
            Debug.LogError("HP Slider not assigned");
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
    
    /// <summary>
    /// 현재 UTC 시간을 반환
    /// </summary>
    public static DateTime GetCurrentTimeUTC() // 현재 UTC 시간 반환 
    {
        return DateTime.UtcNow;
    }
    
    /// <summary>
    /// 현재 한국 표준시(KST)를 반환 (KST : UTC + 9)
    /// </summary>
    public static DateTime GetCurrentTimeKST() // 현재 한국 표준시 반환 
    {
        return DateTime.UtcNow.AddHours(9); // UTC + 9 = KST
    }
    
    /// <summary>
    /// 시간을 HH:mm 형식으로 포맷하여 반환 (60분 이상 : 시간 표시)
    /// </summary>
    public static string FormatTimeHHMM(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);

        if (hours > 0)
        {
            return $"{hours}시간 {minutes}분";
        }
        else
        {
            return $"{minutes}분";
        }
    }
    
    /// <summary>
    // /// 시간을 HH:mm:ss 형식으로 포맷하여 반환
    // /// </summary>
    public static string FormatTimeHHMMSS(float time)
    { 
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }

    public static void CreateDamageText(GameObject prefab, Transform parent, int damage, Vector3 position, bool isFlipX)
    {
        if (prefab == null)
        {
            Debug.LogError("Damage text prefab is null");
            return;
        }
        
        GameObject damageText = GameObject.Instantiate(prefab, position, Quaternion.identity, parent);
        DamageText damageTextInstance = damageText.GetComponent<DamageText>();
        if (damageTextInstance != null)
        {
            damageTextInstance.damage = damage;
            damageTextInstance.InitializeDamageText(isFlipX);
        }
    }
}
