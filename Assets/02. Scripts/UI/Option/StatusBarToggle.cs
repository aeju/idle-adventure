using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarToggle : MonoBehaviour
{
    [SerializeField] private GameObject statusBar;
    [SerializeField] private Toggle switchToggle; 

    private const string StatusBarPrefKey = "StatusBarActive";

    void Start()
    {
        // PlayerPrefs에서 StatusBar 활성화 상태 불러오기 (기본값: 1)
        bool isActive = PlayerPrefs.GetInt(StatusBarPrefKey, 1) == 1;
        statusBar.SetActive(isActive);

        // Toggle의 isOn 속성을 직접 설정
        switchToggle.isOn = isActive;

        // Toggle의 onValueChanged 이벤트에 HandleStatusBarToggle 메소드(상태 저장) 연결
        switchToggle.onValueChanged.AddListener(HandleStatusBarToggle);
        
        // 저장된 상태에 따라 초기 토글 상태 설정
        SetInitialToggleState();
    }

    private void OnStatusBarToggleChanged()
    {
        bool isActive = switchToggle.isOn;
        HandleStatusBarToggle(isActive);
    }

    private void HandleStatusBarToggle(bool isActive)
    {
        // StatusBar의 활성/비활성 상태 설정
        statusBar.SetActive(isActive);
        
        // 변경된 상태를 PlayerPrefs에 저장
        PlayerPrefs.SetInt(StatusBarPrefKey, isActive ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 시작 시 저장된 상태에 따라 토글 상태 초기화
    private void SetInitialToggleState()
    {
        bool savedState = PlayerPrefs.GetInt(StatusBarPrefKey, 1) == 1;
        switchToggle.isOn = savedState;
    }
}
