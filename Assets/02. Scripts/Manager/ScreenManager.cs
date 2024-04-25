using UnityEngine;

// On: 정해진 시간 동안 입력 x, Idle Mode Btn
// Off: 잠금해제 버튼
public class ScreenManager : Singleton<ScreenManager>
{
    [SerializeField] private float idleTime = 30.0f; // 방치 시간 : 30초 
    [SerializeField] private float currentTime;
    
    [SerializeField] private Canvas idleModeCanvas; // idle Mode 표시 Canvas
    [SerializeField] private CountTime countTime;
    
    [SerializeField] private bool isIdleModeActive = false;
    
    private void Start()
    {
        idleModeCanvas.enabled = false;
        ResetIdleModeTimer(); // 타이머 초기화
    }

    private void Update()
    {
        if (!isIdleModeActive)
        {
            // 입력값이 있으면, 절전모드 타이머 초기화
            if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                ResetIdleModeTimer();
            }

            else 
            {
                currentTime += Time.deltaTime; // 입력이 없을 경우 타이머 증가

                // 지정된 방치 시간이 초과되면 검은 화면을 활성화
                if (currentTime >= idleTime && !idleModeCanvas.enabled)
                {
                    Debug.Log("Idle Mode Screen On");
                    idleModeCanvas.enabled = true; // idle Mode 잠금화면 활성화
                    countTime.IdleModeOn();
                }
            }
        }
    }
    
    // IdleModeBtn에서 호출
    public void ActivateIdleModeScreen()
    {
        if (!idleModeCanvas.enabled)
        {
            idleModeCanvas.enabled = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep; // 화면 꺼짐 방지 
            countTime.IdleModeOn();
        }
    }
    
    public void DeactivateIdleModeCanvas()
    {
        if (idleModeCanvas.enabled)
        {
            idleModeCanvas.enabled = false;
            Screen.sleepTimeout = SleepTimeout.SystemSetting; // 화면 꺼짐 방지 해제 (기기 설정 따르도록)
            countTime.IdleModeOff();
        }
    }
    
    public void ResetIdleModeTimer()
    {
        currentTime = 0;
    }
}
