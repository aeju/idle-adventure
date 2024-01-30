using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class AwakenBtn : MonoBehaviour
{
    private Button awakenBtn;
    
    void Start()
    {
        awakenBtn = GetComponent<Button>();
        
        if (awakenBtn == null)
        {
            Debug.LogError("Awaken button is not assigned.");
            return;
        }
        
        awakenBtn.OnClickAsObservable()
            .Subscribe(_ =>
            {
                ScreenManager.Instance?.DeactivateIdleModeCanvas();
            })
            .AddTo(this);
    }
}
