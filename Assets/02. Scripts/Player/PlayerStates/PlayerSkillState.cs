using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : IPlayerState
{
    private PlayerController _playerController;

    public void Enter(PlayerController playerController)
    {
        _playerController = playerController;

        if (_playerController.nearestMonster != null)
        {
            _playerController.PlayerSkill();
        }
    }
    
    public void Handle(PlayerController playerController)
    {
        
    }

    public void Exit(PlayerController playerController)
    {
        
    }
}
