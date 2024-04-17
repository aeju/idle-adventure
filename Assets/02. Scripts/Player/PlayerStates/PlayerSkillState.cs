using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : IPlayerState
{
    private PlayerController _playerController;

    public void Enter(PlayerController playerController)
    {
        _playerController = playerController;
        
        // 스킬 몬스터 탐지
        var skillMonsters = _playerController.GetmonstersInRange(_playerController.skillMonsterMaxCount);
        // 몬스터가 범위 내에 있을 때만 상태 변환
        if (skillMonsters.Count > 0)
        {
            _playerController.PlayerSkill();
        }
        else
            return;
    }
    
    public void Handle(PlayerController playerController)
    {
        
    }

    public void Exit(PlayerController playerController)
    {
        
    }
}
