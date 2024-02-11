using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

public class PlayerAutoState : IPlayerState
{
    private PlayerController _playerController;

    public void Enter(PlayerController playerController)
    {
        if (!_playerController)
            _playerController = playerController;

        if (_playerController.nearestMonster != null)
        {
            _playerController.AutoModeOn();
            
        }
    }
    
    public void Handle(PlayerController playerController)
    {
        
    }

    public void Exit(PlayerController playerController)
    {
        
    }
}
