using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

// ScreenManager - ActivateIdleModeScreen() 호출
public class IdleModeBtn : MonoBehaviour
{
    public ScreenManager screenManager; 
    public Button idleModeButton;       

    void Start()
    {
        if (idleModeButton != null && screenManager != null)
        {
            idleModeButton.OnClickAsObservable().Subscribe(_ => 
                {
                    Debug.Log("Click Btn");
                    screenManager.ActivateIdleModeScreen();
                })
                .AddTo(this);
        }
        else
        {
            Debug.LogError("Button or ScreenManager is null");
        }
    }
}
