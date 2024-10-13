using System;
using UnityEngine;
using TMPro;

// On: 정해진 시간 동안 입력 x, Idle Mode Btn
// Off: 잠금해제 버튼
public class ScreenManager : Singleton<ScreenManager>
{
    [SerializeField] private float idleTime = 30.0f; // 방치 시간 : 30초 
    [SerializeField] private int idleFPS = 10; // 절전모드 FPS : 10 
    
    [SerializeField] private Canvas idleModeCanvas; // idle Mode 표시 Canvas
    [SerializeField] private Canvas deathCanvas; // death 표시 Canvas
    [SerializeField] private IdleModeCountTime _idleModeCountTime;
    
    [SerializeField] private bool isIdleModeActive = false;
    [SerializeField] private bool isDeathScreenActive = false;
    
    public bool IsIdleModeActive { get { return isIdleModeActive; } }
    
    private const Definitions.TimeId TimerId = Definitions.TimeId.IdleModeTimer; // 타이머 ID
    
    protected override void Awake()
    {
        base.Awake(); // Singleton의 Awake 메서드 호출
        
        // 만약 에디터에서 절전모드/죽음 캔버스 비활성화여도
        Utilities.EnsureActive(idleModeCanvas.gameObject);
        Utilities.EnsureActive(deathCanvas.gameObject);
    }
    
    private void Start()
    {
        idleModeCanvas.enabled = false;
        deathCanvas.enabled = false;
    }

    private void Update()
    {
        //if (!isIdleModeActive)
        if (!isIdleModeActive && !isDeathScreenActive)
        {
            // 입력값이 있으면, 절전모드 타이머 초기화
            if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                DeactivateIdleModeCanvas();
                Debug.Log("Input detected, resetting idle timer.");
            }

            else 
            {
                TimeManager.Instance.UpdateTimer(TimerId, Time.deltaTime);
                float currentIdleTime = TimeManager.Instance.GetTime(TimerId);

                // 입력이 없을 경우 타이머 증가 + 지정된 방치 시간이 초과되면 Idle Canvas 활성화
                if (currentIdleTime >= idleTime && !idleModeCanvas.enabled)
                {
                    Debug.Log("Idle time exceeded, activating idle mode screen.");
                    ActivateIdleModeScreen();
                }
            }
        }
        else // 절전모드 활성화 (타이머 시작)
        {
            TimeManager.Instance.UpdateTimer(TimerId, Time.deltaTime);
        }
    }
    
    // IdleModeBtn에서 호출
    public void ActivateIdleModeScreen()
    {
        if (!idleModeCanvas.enabled)
        {
            idleModeCanvas.enabled = true;
            isIdleModeActive = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep; // 화면 꺼짐 방지 
            ReduceFPS(); // FPS 낮추기
        }
    }
    
    public void DeactivateIdleModeCanvas()
    {
        if (idleModeCanvas.enabled)
        {
            idleModeCanvas.enabled = false;
            isIdleModeActive = false;
            Screen.sleepTimeout = SleepTimeout.SystemSetting; // 화면 꺼짐 방지 해제 (기기 설정 따르도록)
            MonsterKillCounterManager.Instance.ResetIdleMonsterCounter(); // 몬스터 카운터 초기화
            ResetFPS(); // FPS 되돌리기
        }
        ResetIdleTimer(); // 타이머 리셋 : 항상 수행 <- 게임 재시작
    }
    
    public void ResetIdleTimer()
    {
        TimeManager.Instance.ResetTimer(TimerId);
        Debug.Log("Idle timer has been reset.");
    }

    // 절전 모드 O : FPS 낮추기
    private void ReduceFPS()
    {
        Application.targetFrameRate = idleFPS; 
    }

    // 절전 모드 X : FPS 되돌리기
    private void ResetFPS()
    {
        // 저장된 FPS 값이 없다면, 기본 FPS를 60으로 설정
        int defaultFrameRate = 60;
        
        if (OptionManager.Instance == null)
            return;
        
        // OptionManager, FrameRateKey에 해당하는 값을 불러옴
        int originFPS = OptionManager.Instance.GetInt(OptionManager.FrameRateKey, defaultFrameRate);
        // 불러온 FPS 값으로 설정
        Application.targetFrameRate = originFPS;
    }
    
    public void ShowDeathScreen()
    {
        if (!isDeathScreenActive)
        {
            deathCanvas.enabled = true;
            isDeathScreenActive = true;
            DeactivateIdleModeCanvas();
        }
    }

    public void HideDeathScreen()
    {
        if (isDeathScreenActive)
        {
            deathCanvas.enabled = false;
            isDeathScreenActive = false;
        }
    }
}
