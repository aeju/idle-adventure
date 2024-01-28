using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

// 처음: Close
// 오픈: 메뉴버튼 클릭, 메뉴버튼 레드닷 비활성화 
// 닫힘: X버튼, 패널 밖 영역 터치
public class SettingBtn : MonoBehaviour
{
    public Button settingBtn; // 메뉴 버튼
    public Button closeBtn; // 클로즈 버튼 
    
    public GameObject settingPanel; // 세팅 패널
    public MenuBtn menuBtnScript;

    void Start()
    {
        settingPanel.SetActive(false);
        
        settingBtn.OnClickAsObservable().Subscribe(_ =>
        {
            menuBtnScript.ToggleMenuPanel(); // 메뉴 패널 비활성화
            settingPanel.SetActive(true);
        }).AddTo(this);
        
        closeBtn.OnClickAsObservable().Subscribe(_ =>
        {
            settingPanel.SetActive(false);
        }).AddTo(this);
        
        // 터치 감지 
        this.UpdateAsObservable()
            .Where(_ => settingPanel.activeSelf) // 세팅 패널이 활성화된 상황에서
            .Where(_ => Input.GetMouseButtonDown(0)) // 터치가 입력될 때만 구독 내부 코드 실행 
            .Subscribe(_ =>
            {
                // UI 중복 확인: 현재 터치가 UI 요소 위에 있는지 확인 
                if (!EventSystem.current.IsPointerOverGameObject()) 
                    // 터치가 세팅 패널 밖에 있는 경우, 세팅 패널을 비활성화
                
                    if (!RectTransformUtility.RectangleContainsScreenPoint( // 터치 -> 메뉴 패널의 RectTransform 범위 내에 있는지 확인 
                            settingPanel.GetComponent<RectTransform>(), 
                            Input.mousePosition, // 터치 현재 위치
                            Camera.main)) // 스크린 좌표 -> 월드 좌표로 전환 
                    {
                        settingPanel.SetActive(false);
                    }
                
            }).AddTo(this);
    }

    // 세팅 패널이 활성화되어 있는지 여부
    public bool IsSettingPanelActive()
    {
        return settingPanel.activeSelf;
    }
}
