using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class OKBtn : MonoBehaviour
{
    private Button okBtn;
    [SerializeField] private GameObject UIPanel;
    
    void Start()
    {
        okBtn = GetComponent<Button>();
        
        // 1. 확인 버튼을 눌렀을 때, 비활성화
        okBtn.OnClickAsObservable().Subscribe(_ =>
        {
            UIPanel.SetActive(false);
        }).AddTo(this);
    }

    void OnEnable()
    {
        // 2. 3초가 지나면, 자동으로 비활성화
        Observable.Timer(System.TimeSpan.FromSeconds(3)).Subscribe(_ =>
        {
            UIPanel.SetActive(false);
        }).AddTo(this);
    }
}
