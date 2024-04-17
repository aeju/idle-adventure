using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : IPlayerState
{
    private PlayerController _playerController;

    private List<GameObject> skillMonsters; // 스킬 적용 몬스터 목록 저장
    
    public void Enter(PlayerController playerController)
    {
        _playerController = playerController;
        
        // 스킬 몬스터 탐지
        skillMonsters = _playerController.GetmonstersInRange(_playerController.skillMonsterMaxCount);
        // 몬스터가 범위 내에 있을 때만 상태 변환
        if (skillMonsters.Count > 0)
        {
            _playerController.PlayerSkill(skillMonsters);
        }
        else
            return;
    }
    
    public void Handle(PlayerController playerController)
    {
        
    }

    public void Exit(PlayerController playerController)
    {
        skillMonsters = null; 
    }
}
