using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedState : MonoBehaviour, IPlayerState
{
    private PlayerController _playerController;

    public void Enter(PlayerController playerController)
    {
        
    }
    
    public void Handle(PlayerController playerController)
    {
        if (!_playerController)
            _playerController = playerController;
    }
    
    public void Exit(PlayerController playerController)
    {
        
    }
}
