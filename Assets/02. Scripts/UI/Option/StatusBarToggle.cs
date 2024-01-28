using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 1 : On 
public class StatusBarToggle : MonoBehaviour
{
    [SerializeField] private GameObject statusBar;
    [SerializeField] private Toggle switchToggle;
    
    void Start()
    {
        // OptionManager에서 StatusBar 활성화 상태 불러오기 (기본값: 1)
        bool isActive = OptionManager.Instance.GetInt(OptionManager.StatusBarActiveKey, 1) == 1;
        statusBar.SetActive(isActive);

        // Toggle의 isOn 속성을 직접 설정
        switchToggle.isOn = isActive;

        // Toggle의 onValueChanged 이벤트에 HandleStatusBarToggle 메소드(상태 저장) 연결
        switchToggle.onValueChanged.AddListener(HandleStatusBarToggle);
        
        // 저장된 상태에 따라 초기 토글 상태 설정
        SetInitialToggleState();
    }

    private void HandleStatusBarToggle(bool isActive)
    {
        // StatusBar의 활성/비활성 상태 설정
        statusBar.SetActive(isActive);
        
        // 변경된 상태를 OptionManager에 저장
        OptionManager.Instance.SetInt(OptionManager.StatusBarActiveKey, isActive ? 1 : 0);
    }

    // 시작 시 저장된 상태에 따라 토글 상태 초기화
    private void SetInitialToggleState()
    {
        bool savedState = OptionManager.Instance.GetInt(OptionManager.StatusBarActiveKey, 1) == 1;
        switchToggle.isOn = savedState;
    }
}
