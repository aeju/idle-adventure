using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Button restartButton;
    [SerializeField] private PlayerController playerController;

    void Awake()
    {
        base.Awake();
        
        playerController = FindObjectOfType<PlayerController>();
    }
    
    void Start()
    {
        if (restartButton != null)
        {
            restartButton.OnClickAsObservable().Subscribe(_ =>
            {
                Restart(); 
            }).AddTo(this);
        }
        else
        {
            Debug.LogError("restartButton reference is missing in GameManager!");
        }
    }
    
    private void Restart()
    {
        if (playerController != null)
        {
            playerController.isRespawnRequested = true; // 플레이어 리스폰 플래그
            ScreenManager.Instance.HideDeathScreen();
            ScreenManager.Instance.DeactivateIdleModeCanvas();  // ScreenManager - idleTimer 초기화
            
            EnemyManager.Instance.ResetAllMonsters(); // 모든 몬스터, 오브젝트풀 반환
            
            MonsterKillCounterManager.Instance.ResetTimeAndCount(); // 인게임 몬스터 카운트 초기화
            MonsterKillCounterManager.Instance.ResetIdleMonsterCounter(); // 절전모드 몬스터 카운트 초기화
        }
        else
        {
            Debug.LogError("PlayerController reference is missing in GameManager!");
        }
    }
    
}
