using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateContext
{
    private IPlayerState CurrentState
    {
        get;
        set;
    }

    private readonly PlayerController _playerController;

    public PlayerStateContext(PlayerController playerController)
    {
        _playerController = playerController;
    }

    public void Transition(IPlayerState newState)
    {
        // 현재 상태가 있다면, 현재 상태의 Exit 메서드를 호출
        CurrentState?.Exit(_playerController);
        // 새 상태로 전환
        CurrentState = newState;
        // 새 상태의 Enter 메서드를 호출
        CurrentState.Enter(_playerController);
        // 새 상태의 Handle 메서드를 호출하여 상태별 로직 실행
        CurrentState.Handle(_playerController);
    }
}
