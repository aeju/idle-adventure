using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 조건 : 환경설정 패널이 한 번도 열리지 않은 경우
// 메뉴 패널이 열리면 -> 메뉴 버튼 - Off / 환경 설정 버튼 - On
public class MenuBtnRedDot : MonoBehaviour
{
    public GameObject menuBtnRedDot;
    
    void Start()
    {
        if (!PlayerPrefs.HasKey(OptionManager.MenuOpenedKey))
        {
            menuBtnRedDot.SetActive(true);
        }
    }

    public void SetActive(bool active)
    {
        menuBtnRedDot.SetActive(active);
    }
}
