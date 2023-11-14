using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class MainUI : MonoBehaviour
{
    public Button Xbutton;

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

    private void Awake()
    {
        Hero_Page.SetActive(false);
        Inventory_Page.SetActive(false);
        Strengthen_Page.SetActive(false);
        Contents_Page.SetActive(false);
        Mission_Page.SetActive(false);
        Shop_Page.SetActive(false);
    }

        private void Start()
    {
        Xbutton.OnClickAsObservable().Subscribe(_ =>
        {
            // 모든 창 닫기게 
        }).AddTo(this);


        // 버튼 누르면, 해당 페이지 UI 켜지게
        Hero_Btn.OnClickAsObservable().Subscribe(_ =>
        {
            Hero_Page.SetActive(true); 
        }).AddTo(this);

        Inventory_Btn.OnClickAsObservable().Subscribe(_ =>
        {
            Inventory_Page.SetActive(true);
        }).AddTo(this);

        Strengthen_Btn.OnClickAsObservable().Subscribe(_ =>
        {
            Strengthen_Page.SetActive(true);
        }).AddTo(this);

        Contents_Btn.OnClickAsObservable().Subscribe(_ =>
        {
            Contents_Page.SetActive(true);
        }).AddTo(this);

        Mission_Btn.OnClickAsObservable().Subscribe(_ =>
        {
            Mission_Page.SetActive(true);
        }).AddTo(this);

        Shop_Btn.OnClickAsObservable().Subscribe(_ =>
        {
            Shop_Page.SetActive(true); 
        }).AddTo(this);
    }
}
