using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BatteryManager로부터 이벤트를 수신, 각 상속받은 클래스가 구체적으로 처리하도록
public abstract class BatteryStatusTemplate : MonoBehaviour
{
    // BatteryManager 이벤트에 연결
    private void OnEnable()
    {
        if (BatteryManager.Instance != null)
        {
            BatteryManager.Instance.OnBatteryStatusChanged += HandleBatteryStatusChange;
        }
    }
    
    private void OnDisable()
    {
        if (BatteryManager.Instance != null)
        {
            BatteryManager.Instance.OnBatteryStatusChanged -= HandleBatteryStatusChange;
        }
    }
    
    // 템플릿 메소드 : 이벤트가 발생할 때 호출 (구체적인 처리 : 하위 클래스에서 정의)
    private void HandleBatteryStatusChange(int percentage, UnityEngine.BatteryStatus status)
    {
        HandleBatteryChanged(percentage, status);
    }

    // 템플릿 메소드에서 호출되는 후크 메소드 (하위 클래스에서 각자 배터리 상태 변경 처리)
    protected abstract void HandleBatteryChanged(int percentage, UnityEngine.BatteryStatus status);
}
