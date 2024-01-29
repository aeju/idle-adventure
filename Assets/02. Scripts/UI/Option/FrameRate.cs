using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 하나의 토글 on -> 나머지 : off
// 프레임 : 30, 45, 60
public class FrameRate : MonoBehaviour
{
    public Toggle toggle30;
    public Toggle toggle45;
    public Toggle toggle60;

    // 상단바 - fps 디스플레이 변경 이벤트
    public static event Action<int> OnFrameRateChanged;
    
    private void Start()
    {
        // 기본 FPS 설정
        SetFrameRate(OptionManager.Instance.GetInt(OptionManager.FrameRateKey, 60));

        // 각 토글에 토글 이벤트 리스너 할당
        toggle30.onValueChanged.AddListener(delegate { OnToggleChanged(); });
        toggle45.onValueChanged.AddListener(delegate { OnToggleChanged(); });
        toggle60.onValueChanged.AddListener(delegate { OnToggleChanged(); });

        // 저장된 프레임 레이트에 따라 초기 토글 상태 설정
        SetInitialToggleState();
    }
    
    // 토글이 변경될 때 호출
    private void OnToggleChanged()
    {
        if (toggle30.isOn)
        {
            SetFrameRate(30);
        }
        else if (toggle45.isOn)
        {
            SetFrameRate(45);
        }
        else if (toggle60.isOn)
        {
            SetFrameRate(60);
        }
    }

    // 시작 시 저장된 프레임 레이트에 따라 토글 상태 초기화 
    private void SetInitialToggleState()
    {
        int savedFrameRate = OptionManager.Instance.GetInt(OptionManager.FrameRateKey, 60);

        toggle30.isOn = savedFrameRate == 30;
        toggle45.isOn = savedFrameRate == 45;
        toggle60.isOn = savedFrameRate == 60;
    }
    
    private void SetFrameRate(int frameRate)
    {
        Application.targetFrameRate = frameRate; // 프레임 레이트 설정
        OptionManager.Instance.SetInt(OptionManager.FrameRateKey, frameRate);
    }
}
