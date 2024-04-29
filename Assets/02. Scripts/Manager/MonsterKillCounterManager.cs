using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UniRx;

// 1. 시간(초) 재기
// 2. 리셋 버튼 : 시간 리셋
// (이후, 절전모드일 때 몬스터 카운트)
public class MonsterKillCounterManager : Singleton<MonsterKillCounterManager>
{
    [SerializeField] private TextMeshProUGUI totalMonsterCounterText;
    
    [SerializeField] private TextMeshProUGUI IdleModeMonsterCounterText;
    
    [SerializeField] private TextMeshProUGUI timeCountText;
    [SerializeField] private Button resetBtn; 
    
    private const string TimerId = "GamePlayTimer";
    
    // 몬스터 처치 수
    public int TotalMonsterCounter { get; private set; } // 게임 진행
    public int IdleModeMonsterCounter { get; private set; } // 절전 모드
    
    void Start()
    {
        EnemyFSM.OnEnemyDeath += IncreaseTotalMonsterCounter; // 죽음 이벤트 구독
        
        TotalMonsterCounter = 0; // 몬스터 카운터 초기화
        IdleModeMonsterCounter = 0; // 절전모드 몬스터 카운터 초기화
        TimeManager.Instance.ResetTimer(TimerId); // 시간 초기화
        
        UpdateDefaultMonsterCounterUI(); // 몬스터 마릿수 UI 업데이트 
        UpdateIdleMonsterCounterUI();
        UpdateTimeUI(); // 시간 UI 업데이트
        
        resetBtn.OnClickAsObservable().Subscribe(_ =>
        {
            ResetTimeAndCount(); 
        }).AddTo(this);
    }
    
    void OnDestroy()
    {
        EnemyFSM.OnEnemyDeath -= IncreaseTotalMonsterCounter; // 이벤트 구독 해제
    }
    
    void Update()
    {
        TimeManager.Instance.UpdateTimer(TimerId, Time.deltaTime);
        UpdateTimeUI(); // 매 프레임 시간 UI 업데이트
    }

    private void IncreaseTotalMonsterCounter()
    {
        TotalMonsterCounter++;
        
        // 절전 모드일 때만 IdleModeMonsterCounter 증가
        if (ScreenManager.Instance.IsIdleModeActive)
        {
            IdleModeMonsterCounter++;
            Debug.Log("Idle Mode Monster ++");
            UpdateIdleMonsterCounterUI();
        }
        
        UpdateDefaultMonsterCounterUI();
    }
    
    private void UpdateDefaultMonsterCounterUI()
    {
        totalMonsterCounterText.text = $"{TotalMonsterCounter}";
    }
    
    private void UpdateIdleMonsterCounterUI()
    {
        IdleModeMonsterCounterText.text = $"{IdleModeMonsterCounter}";
    }

    private void UpdateTimeUI()
    {
        timeCountText.text = Utilities.FormatTimeHHMMSS(TimeManager.Instance.GetTime(TimerId));
    }
    
    void ResetTimeAndCount()
    {
        TotalMonsterCounter = 0; // 몬스터 카운터를 0으로 초기화
        TimeManager.Instance.ResetTimer(TimerId);
        UpdateDefaultMonsterCounterUI(); 
    }

    public void ResetIdleMonsterCounter()
    {
        IdleModeMonsterCounter = 0;
        UpdateIdleMonsterCounterUI();
    }
}
