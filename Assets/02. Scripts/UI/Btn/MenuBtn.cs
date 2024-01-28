using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;

// 처음 : menuPanel 비활성화
// 메뉴 아이콘 클릭 : menuPanel 활성화
// menuPanel이 활성화된 상태에서, Xbtn 터치하거나 그 외 영역 터치 -> menuPanel 비활성화 
// 문제점 : menu Panel 내 버튼(idleModeBtn) 클릭 -> menu Panel 비활성화
public class MenuBtn : MonoBehaviour
{
    public Button menuBtn; // 메뉴 버튼
    public Button closeBtn; // 클로즈 버튼 
    public GameObject menuPanel; // 메뉴 패널 
    public GameObject settingBtnRedDot;

    public MenuBtnRedDot menuBtnRedDot;
    
    void Start()
    {
        menuPanel.SetActive(false);
        
        menuBtn.OnClickAsObservable().Subscribe(_ =>
        {
            ToggleMenuPanel();
            //MenuBtnRedDotOff();
        }).AddTo(this);
        
        closeBtn.OnClickAsObservable().Subscribe(_ =>
        {
            ToggleMenuPanel();
        }).AddTo(this);
        
        // 터치 감지 
        this.UpdateAsObservable()
            .Where(_ => menuPanel.activeSelf) // 메뉴 패널이 활성화된 상황에서
            .Where(_ => Input.GetMouseButtonDown(0)) // 터치가 입력될 때만 구독 내부 코드 실행 
            .Subscribe(_ =>
            {
                // UI 중복 확인: 현재 터치가 UI 요소 위에 있는지 확인 
                if (!EventSystem.current.IsPointerOverGameObject()) 
                // 터치가 메뉴 패널 밖에 있는 경우, 메뉴 패널을 비활성화 
                if (!RectTransformUtility.RectangleContainsScreenPoint( // 터치 -> 메뉴 패널의 RectTransform 범위 내에 있는지 확인 
                        menuPanel.GetComponent<RectTransform>(), 
                        Input.mousePosition, // 터치 현재 위치
                        Camera.main)) // 스크린 좌표 -> 월드 좌표로 전환 
                {
                    ToggleMenuPanel();
                }
            }).AddTo(this);
    }
    
    public void ToggleMenuPanel() // SettingBtn에서 호출
    {
        menuPanel.SetActive(!menuPanel.activeSelf); //상태 전환 

        if (menuBtnRedDot != null)
        {
            menuBtnRedDot.RedDotOff();
        }
    }
    
    private void MenuBtnRedDotOff()
    {
        // MenuBtnRedDot의 DisableRedDot 메소드 호출
        if (menuBtnRedDot != null)
        {
            menuBtnRedDot.RedDotOff();
        }
    }
}
