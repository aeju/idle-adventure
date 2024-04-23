using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoState : IPlayerState
{
    private PlayerController _playerController;

    public void Enter(PlayerController playerController)
    {
        if (!_playerController)
            _playerController = playerController;
        // _playerController.AutoModeOn();
        
        //_playerController.autoModeActive = true;
    }
    
    public void Handle(PlayerController playerController)
    {
        Debug.Log("[AutoMove]0. Handle");
        if (!_playerController.isFighting)
        {
            Debug.Log("[AutoMove]0-1.실행");
            _playerController.StartCoroutine(_playerController.AutoModeDetectMonstersPeriodically());
            //_playerController.MoveTowardsNearestEnemy();
        }
    }

    public void Exit(PlayerController playerController)
    {
        //_playerController.autoModeActive = false;
    }
}
