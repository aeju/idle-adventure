using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

// ScreenManager - ActivateIdleModeScreen() 호출
public class IdleModeBtn : MonoBehaviour
{
    [SerializeField] private ScreenManager screenManager;
    [SerializeField] private Button idleModeButton;

    void Start()
    {
        // 버튼과 스크린 매니저가 모두 설정되어 있는지 확인
        if (idleModeButton == null || screenManager == null)
        {
            // 누락된 컴포넌트를 식별하여, 적절한 에러 메시지를 출력
            string missingComponent = idleModeButton == null ? "IdleModeButton" : "ScreenManager";
            Debug.LogError("Component missing: " + missingComponent + " is null.");
            return; // 초기화 중단
        }

        idleModeButton.OnClickAsObservable().Subscribe(_ => 
        { 
            screenManager.ActivateIdleModeScreen();
        }).AddTo(this);
    }
}