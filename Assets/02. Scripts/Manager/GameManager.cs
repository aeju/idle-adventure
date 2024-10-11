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
            playerController.Respawn(); // 플레이어 리스폰
            ScreenManager.Instance.HideDeathScreen();
            ScreenManager.Instance.DeactivateIdleModeCanvas();  // ScreenManager - idleTimer 초기화
            
            //EnemyManager.Instance.ResetAndRespawnAllEnemies(); // 모든 적 재생성
        }
        else
        {
            Debug.LogError("PlayerController reference is missing in GameManager!");
        }
    }
    
}
