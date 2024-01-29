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
    public MenuBtn menuBtn;

    void Start()
    {
        settingPanel.SetActive(false);

        // 세팅 버튼을 누르면
        // 1. 세팅 패널 활성화
        // 2. 메뉴 패널 닫히고, 메뉴 버튼 활성화 (= menu 패널 닫기)
        settingBtn.OnClickAsObservable().Subscribe(_ =>
        {
            settingPanel.SetActive(true);
            
            // 세팅 패널이 열릴 때 키값 설정 -> 더 이상 레드닷 뜨지 않음
            PlayerPrefs.SetInt(OptionManager.MenuOpenedKey, 1);
            PlayerPrefs.Save();

            menuBtn.Close();
            
        }).AddTo(this);

        // X 버튼
        closeBtn.OnClickAsObservable().Subscribe(_ =>
        {
            settingPanel.SetActive(false); 
            
        }).AddTo(this);

        // 뒤로가기 키 입력 감지 (= Update)
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Escape)) // 뒤로가기 키가 눌렸을 때
            .Subscribe(_ =>
            {
                if (settingPanel.activeSelf)
                {
                    settingPanel.SetActive(false);
                }
            }).AddTo(this);
    }
}
