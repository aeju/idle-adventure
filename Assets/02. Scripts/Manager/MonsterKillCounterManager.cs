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
    
    [SerializeField] private TextMeshProUGUI timeCountText;
    [SerializeField] private Button resetBtn; 
    
    // 게임 진행, 몬스터 처치 수
    public int TotalMonsterCounter { get; private set; }

    // 절전 모드, 몬스터 처치 수
    public int IdleModeMonsterCounter { get; private set; }
    
    void Start()
    {
        EnemyFSM.OnEnemyDeath += IncreaseTotalMonsterCounter; // 죽음 이벤트 구독
        
        TotalMonsterCounter = 0; // 몬스터 카운터 초기화
        UpdateMonsterCounterUI(); // 몬스터 마릿수 UI 업데이트 
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
        UpdateTimeUI(); // 매 프레임 시간 UI 업데이트
    }

    private void IncreaseTotalMonsterCounter()
    {
        TotalMonsterCounter++;
        UpdateMonsterCounterUI();
    }
    
    private void UpdateMonsterCounterUI()
    {
        totalMonsterCounterText.text = $"{TotalMonsterCounter}";
    }

    private void UpdateTimeUI()
    {
        timeCountText.text = FormatTime(TimeManager.Instance.TimeElapsed); 
        Debug.Log("Time Update");
    }
    
    void ResetTimeAndCount()
    {
        TotalMonsterCounter = 0; // 몬스터 카운터를 0으로 초기화
        TimeManager.Instance.ResetTime();
        UpdateMonsterCounterUI(); 
    }
    
    // HH:mm:ss
    private string FormatTime(float time)
    { 
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }
}
