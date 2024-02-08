using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedState : IPlayerState
{
    private PlayerController _playerController;

    public void Enter(PlayerController playerController)
    {
        if (!_playerController)
            _playerController = playerController;
    }

    public void Handle(PlayerController playerController)
    {
        
    }

    public void Exit(PlayerController playerController)
    {

    }
    
}
