using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : IPlayerState
{
    private PlayerController _playerController;
    
    private List<GameObject> attackMonsters; // 어택 적용 몬스터 목록 저장

    public void Enter(PlayerController playerController)
    {
        _playerController = playerController;
        
        // 몬스터 탐지
        attackMonsters = _playerController.GetMonstersInFront(_playerController.attackMonsterMaxCount);
        // 몬스터가 범위 내에 있을 때만 상태 변환
        if (attackMonsters.Count > 0)
        {
            _playerController.PlayerAttack(attackMonsters);
        }
        else
            return;
    }
    
    public void Handle(PlayerController playerController)
    {
        
    }

    public void Exit(PlayerController playerController)
    {
        attackMonsters = null; 
    }
}
