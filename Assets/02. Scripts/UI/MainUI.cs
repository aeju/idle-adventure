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
    
    /*
    public GameObject X_btn_off;
    public GameObject X_btn_on;
    public Button X_btn;

    public Button Hero_Btn;
    public Button Inventory_Btn;
    public Button Strengthen_Btn;
    public Button Contents_Btn;
    public Button Mission_Btn;
    public Button Shop_Btn;

    public GameObject Hero_Page;
    public GameObject Inventory_Page;
    public GameObject Strengthen_Page;
    public GameObject Contents_Page;
    public GameObject Mission_Page;
    public GameObject Shop_Page;
    */
    
    private GameObject currentOpenPage = null; // 현재 열려있는 페이지

    // X버튼 누를 때! 
    private List<GameObject> pages;
    

    // 버튼-페이지 딕셔너리
    private Dictionary<Button, GameObject> buttonPage;

    private void Awake()
    {
        X_btn_on.SetActive(false);

        Hero_Page.SetActive(false);
        Inventory_Page.SetActive(false);
        Strengthen_Page.SetActive(false);
        Contents_Page.SetActive(false);
        Mission_Page.SetActive(false);
        Shop_Page.SetActive(false);
    }

    private void Start()
    {
        pages = new List<GameObject>
        {
            Hero_Page,
            Inventory_Page,
            Strengthen_Page,
            Contents_Page,
            Mission_Page,
            Shop_Page
        };
        
        buttonPage = new Dictionary<Button, GameObject>
        {
            { Hero_Btn, Hero_Page },
            { Inventory_Btn, Inventory_Page },
            { Strengthen_Btn, Strengthen_Page },
            { Contents_Btn, Contents_Page },
            { Mission_Btn, Mission_Page },
            { Shop_Btn, Shop_Page }
        };
        
        X_btn.OnClickAsObservable().Subscribe(_ =>
        {
            CloseAllPages(); // 무슨 창이든 닫기게
        }).AddTo(this);
        
        foreach (var btn in buttonPage)
        {
            btn.Key.OnClickAsObservable().Subscribe(_ =>
            {
                OpenPage(btn.Value);
            }).AddTo(this);
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
        // 페이지가 열려있는 경우에만 
        if (currentOpenPage != null)
        {
            currentOpenPage.SetActive(false);
            currentOpenPage = null;
            XbtnClose();
        }
    }
    
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
