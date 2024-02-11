using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

// 닫힘: X 버튼, 뒤로가기
public class MainUI : MonoBehaviour
{
    [SerializeField] private GameObject X_btn_off;
    [SerializeField] private GameObject X_btn_on;
    [SerializeField] private Button X_btn;

    [SerializeField] private Button Hero_Btn;
    [SerializeField] private Button Inventory_Btn;
    [SerializeField] private Button Strengthen_Btn;
    [SerializeField] private Button Contents_Btn;
    [SerializeField] private Button Mission_Btn;
    [SerializeField] private Button Shop_Btn;

    [SerializeField] private GameObject Hero_Page;
    [SerializeField] private GameObject Inventory_Page;
    [SerializeField] private GameObject Strengthen_Page;
    [SerializeField] private GameObject Contents_Page;
    [SerializeField] private GameObject Mission_Page;
    [SerializeField] private GameObject Shop_Page;

    private GameObject currentOpenPage = null; // 현재 열려있는 페이지

    // X버튼 누를 때! 
    private List<GameObject> pages;
    
    // 버튼-페이지 딕셔너리
    private Dictionary<Button, GameObject> buttonPage;
    
    // 1. X버튼 비활성화
    // 2. 모든 페이지 비활성화
    // 3. 버튼 - 페이지 연결
    private void Start()
    {
        XbtnClose(); // 1. 초기 X 버튼: 비활성화
        
        pages = new List<GameObject>
        {
            Hero_Page,
            Inventory_Page,
            Strengthen_Page,
            Contents_Page,
            Mission_Page,
            Shop_Page
        };

        foreach (var page in pages) // 2. 모든 페이지 닫기
        {
            page.SetActive(false);
        }
        
        // 3. 버튼 - 페이지 매핑, 구독 목적
        buttonPage = new Dictionary<Button, GameObject>
        {
            { Hero_Btn, Hero_Page },
            { Inventory_Btn, Inventory_Page },
            { Strengthen_Btn, Strengthen_Page },
            { Contents_Btn, Contents_Page },
            { Mission_Btn, Mission_Page },
            { Shop_Btn, Shop_Page }
        };
        
        foreach (var btn in buttonPage) 
        {
            btn.Key.OnClickAsObservable().Subscribe(_ =>
            {
                OpenPage(btn.Value);
            }).AddTo(this);
        }
        
        X_btn.OnClickAsObservable().Subscribe(_ =>
        {
            CloseAllPages(); // 무슨 창이든 닫기게
        }).AddTo(this);
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
        // 페이지가 열려있는 경우에만 
        if (currentOpenPage != null)
        {
            currentOpenPage.SetActive(false);
            currentOpenPage = null;
            XbtnClose();
        }
    }
    
    // 뒤로가기 = 페이지 닫기
    private void Update()
    {
        // 어떤 페이지가 열려있고, 뒤로가기를 누를 때 
        if (currentOpenPage != null && Input.GetKeyDown(KeyCode.Escape))
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
