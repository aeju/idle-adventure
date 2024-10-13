using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : IPlayerState
{
    private PlayerController _playerController;

    public void Enter(PlayerController playerController)
    {
        if (!_playerController)
            _playerController = playerController;
        
        _playerController.anim.SetTrigger("isDead"); // 애니메이션 -> 죽음
        _playerController.isAlive = false;
        
        // 죽음 화면 UI 표시
        ScreenManager.Instance.ShowDeathScreen();
        SoundManager.Instance.PlaySFX("Defeat");
    }
    
    public void Handle(PlayerController playerController)
    {
       
    }

    public void Exit(PlayerController playerController)
    {
        
    }
}
