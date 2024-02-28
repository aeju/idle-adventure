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
        okBtn.OnClickAsObservable().Subscribe(_ =>
        {
            UIPanel.SetActive(false);
        }).AddTo(this);

    }
}
