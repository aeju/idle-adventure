using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : IPlayerState
{
    private PlayerController _playerController;
    
    [Header("# 플레이어 죽음 ")] 
    private float deathTime;
    public float REVIVE_DELAY = 2;
    private Coroutine _reviveCoroutine;
    
    public void Enter(PlayerController playerController)
    {
        if (!_playerController)
            _playerController = playerController;
        
        _playerController.anim.SetTrigger("isDead"); // 애니메이션 -> 죽음
        _playerController.isAlive = false;
        
        deathTime = Time.time;

        // 부활 코루틴 시작
        _reviveCoroutine = _playerController.StartCoroutine(ReviveAfterDelay());
    }
    
    public void Handle(PlayerController playerController)
    {
       
    }

    public void Exit(PlayerController playerController)
    {
        
    }
    
    private IEnumerator ReviveAfterDelay()
    {
        yield return new WaitForSeconds(REVIVE_DELAY);
        
        // 부활 로직
        _playerController.isAlive = true;
        _playerController.playerStats.CurrentHP = _playerController.playerStats.maxHP;
        
        // 애니메이션 초기화 
        _playerController.anim.ResetTrigger("isDead");
        
        // Idle 상태로 전환
        _playerController.IdlePlayer();
    }
}
