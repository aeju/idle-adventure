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

// 조건 1. !A -> B
// 조건 2. (B AND D) -> (B XOR C) 
public class MenuBtn : MonoBehaviour
{
    public Button menuBtn; // 메뉴 버튼
    public Button closeBtn; // 클로즈 버튼 
    public GameObject menuPanel; // 메뉴 패널 
    public GameObject settingPanel; // 세팅 패널

    public GameObject settingBtnRedDot; // 2. 세팅 버튼 레드닷

    public MenuBtnRedDot menuBtnRedDot; // 1. 메뉴 버튼 레드닷
    public SettingBtn settingBtn;

    void Awake()
    {
        // 만약 에디터에서 세팅 패널 비활성화여도
        Utilities.EnsureActive(settingPanel);
    }
    
    void Start()
    {
        menuPanel.SetActive(false);
        settingPanel.SetActive(false);
        
        menuBtn.OnClickAsObservable().Subscribe(_ =>
        {
            // 세팅 패널 열리지 않았을 때만 눌리게 
            if (!settingBtn.settingPanel.activeSelf)
            {
                Open(); // 세팅 패널 On
            }
        }).AddTo(this);
        
        closeBtn.OnClickAsObservable().Subscribe(_ =>
        {
            Close(); // 세팅 패널 Off
        }).AddTo(this);
    }

    public void Open()
    {
        menuBtn.gameObject.SetActive(false);
        menuPanel.SetActive(true);

        if (!PlayerPrefs.HasKey(OptionManager.MenuOpenedKey))
        {
            settingBtnRedDot.SetActive(true);
        }
        else
        {
            settingBtnRedDot.SetActive(false);
        }
    }

    // 메뉴 패널이 닫히는 경우, 메뉴 버튼 활성화 (+ 레드닷 활성화)
    public void Close()
    {
        menuPanel.SetActive(false);
        menuBtn.gameObject.SetActive(true);

        if (!PlayerPrefs.HasKey(OptionManager.MenuOpenedKey))
        {
            menuBtnRedDot.SetActive(true);
        }
        else
        {
            menuBtnRedDot.SetActive(false);
        }
    }
}
