using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private PlayerController _playerController;

    public void Enter(PlayerController playerController)
    {
        _playerController = playerController;

        if (_playerController.nearestMonster != null)
        {
            if (Input.GetKeyUp(KeyCode.Z))
            {
                _playerController.PlayerAttack();
            }
            
            else if (Input.GetKeyUp(KeyCode.X))
            {
                _playerController.PlayerSkill();
            }
        }
    }
    
    public void Handle(PlayerController playerController)
    {
        /*
        if (!_playerController)
            _playerController = playerController;
        
        _playerController.monsterLayerMask = LayerMask.GetMask("Enemy");
        
        if (_playerController.nearestMonster != null)
        {
            // 일반 공격
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _playerController.PlayerAttack();
            }

            // 스킬 공격
            if (Input.GetKeyDown(KeyCode.X))
            {
                _playerController.PlayerSkill();
            }
        }
        */
    }

    public void Exit(PlayerController playerController)
    {
        
    }
    
    
}
