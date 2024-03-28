using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : Singleton<OptionManager>
{
    public const string FrameRateKey = "FrameRate";
    public const string StatusBarActiveKey = "StatusBarActive";
    public const string MenuOpenedKey = "MenuOpend";

    // 설정 값 불러오기
    public int GetInt(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }

    // 설정 값 저장
    public void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
        PlayerPrefs.Save();
    }
}
