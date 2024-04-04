using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MainUI : MonoBehaviour
{
    [SerializeField] private GameObject X_btn_off;
    [SerializeField] private GameObject X_btn_on;
    [SerializeField] private Button X_btn;

    [SerializeField] private Button Hero_Btn;
    [SerializeField] private Button Inventory_Btn;
    [SerializeField] private Button Strengthen_Btn;
    [SerializeField] private Button CoinBuff_Btn;
    [SerializeField] private Button ExpBuff_Btn;
    [SerializeField] private Button Shop_Btn;

    [SerializeField] private GameObject Hero_Page;
    [SerializeField] private GameObject Inventory_Page;
    [SerializeField] private GameObject Strengthen_Page;
    [SerializeField] private GameObject CoinBuff_Page;
    [SerializeField] private GameObject ExpBuff_Page;
    [SerializeField] private GameObject Shop_Page;

    private GameObject currentOpenPage = null; // 현재 열려있는 페이지
    
    private List<GameObject> pages; // X버튼 누를 때! 
    
    private Dictionary<Button, GameObject> buttonPage; // 버튼-페이지 딕셔너리
    
    // 페이지 닫기 : X버튼 / ABB키
    private void Start()
    {
        XbtnClose(); // 1. X 버튼: 비활성화
        
        pages = new List<GameObject>
        {
            Hero_Page,
            Inventory_Page,
            Strengthen_Page,
            CoinBuff_Page,
            ExpBuff_Page,
            Shop_Page
        };

        foreach (var page in pages) // 2. 모든 페이지 닫기
        {
            page.SetActive(false);
        }
        
        // 3. 버튼 - 페이지 매핑 (구독 목적)
        buttonPage = new Dictionary<Button, GameObject>
        {
            { Hero_Btn, Hero_Page },
            { Inventory_Btn, Inventory_Page },
            { Strengthen_Btn, Strengthen_Page },
            { CoinBuff_Btn, CoinBuff_Page },
            { ExpBuff_Btn, ExpBuff_Page },
            { Shop_Btn, Shop_Page }
        };
        
        foreach (var btn in buttonPage) 
        {
            btn.Key.OnClickAsObservable().Subscribe(_ =>
            {
                OpenPage(btn.Value);
            }).AddTo(this);
        } 
        
        // 1) X버튼 눌러서 닫기
        X_btn.OnClickAsObservable().Subscribe(_ =>
        {
            CloseAllPages(); // 무슨 창이든 닫기게
        }).AddTo(this);
        
        // 버프 활성화에 대한 구독
        BuffManager.Instance.OnBuffActivated += ClosePagesOnBuffActivate;
    }
    
    void OnDestroy()
    {
        // 버프 활성화 구독 취소
        if (BuffManager.Instance != null)
        {
            BuffManager.Instance.OnBuffActivated -= ClosePagesOnBuffActivate;
        }
    }
    
    // 버프 활성화 시 실행
    private void ClosePagesOnBuffActivate(Buff buff)
    {
        // 버프 종류에 따라 열린 페이지 닫기
        switch (buff.buffType)
        {
            case BuffType.Coin:
                // 코인 버프 활성화 시 CoinBuff_Page 닫기
                if (currentOpenPage == CoinBuff_Page)
                {
                    CloseAllPages();
                }
                break;
            case BuffType.Exp:
                // 경험치 버프 활성화 시 ExpBuff_Page 닫기
                if (currentOpenPage == ExpBuff_Page)
                {
                    CloseAllPages();
                }
                break;
        }
    }
    
    private void OpenPage(GameObject page)
    {
        CloseAllPages();
        page.SetActive(true);
        currentOpenPage = page; // 현재 열린 페이지 업데이트
        XbtnOpen();
    }
    
    private void CloseAllPages()
    {
        if (currentOpenPage != null) // 페이지가 열려있는 경우에만 
        {
            currentOpenPage.SetActive(false); // 현재 열려있는 페이지 닫기
            currentOpenPage = null;
            XbtnClose();
        }
    }
    
    // 2) ABB키 -> 페이지 닫기
    private void Update()
    {
        // 어떤 페이지가 열려있고, 뒤로가기를 누를 때 
        if (currentOpenPage != null 
            && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllPages();
        }
    }
    
    private void XbtnOpen()
    {
        X_btn_off.SetActive(false);
        X_btn_on.SetActive(true);
    }
    
    private void XbtnClose()
    {
        X_btn_off.SetActive(true);
        X_btn_on.SetActive(false);
    }
}
