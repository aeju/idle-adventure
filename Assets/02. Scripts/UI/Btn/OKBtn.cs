using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

// 1. 확인 버튼 눌렀을 때, 비활성화
// 2. 3초가 지나면, 비활성화
public class OKBtn : MonoBehaviour
{
    private Button okBtn;
    [SerializeField] private GameObject UIPanel;
    
    void Start()
    {
        okBtn = GetComponent<Button>();
        
        okBtn.OnClickAsObservable().Subscribe(_ =>
        {
            UIPanel.SetActive(false);
        }).AddTo(this);
    }

    void OnEnable()
    {
        StartCoroutine(CloseUIPanelDelay(3)); 
    }

    IEnumerator CloseUIPanelDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UIPanel.SetActive(false);
    }
}
