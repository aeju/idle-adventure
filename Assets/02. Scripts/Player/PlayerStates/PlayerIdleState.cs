using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerIdleState : IPlayerState
{
    private PlayerController _playerController;
    
    public void Enter(PlayerController playerController)
    {
        if (!_playerController)
            _playerController = playerController;
        
        _playerController.isAlive = true;
        _playerController.anim.SetBool("isMove", false);
    }
    
    public void Handle(PlayerController playerController)
    {
        // isMonsterDetected가 false일 때 탐지 로직 실행
        if (!_playerController.isMonsterDetected)
        {
            _playerController.StartCoroutine(_playerController.DetectMonstersPeriodically());
        }
        else // 주변 몬스터 목록 있으면, 공격 
        {
            _playerController.AutoAttack();
        }
    }

    public void Exit(PlayerController playerController)
    {
        
    }
}
