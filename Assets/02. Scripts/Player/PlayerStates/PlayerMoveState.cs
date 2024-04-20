using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveState : IPlayerState
{
    private PlayerController _playerController;

    public void Enter(PlayerController playerController)
    {
        if (!_playerController)
            _playerController = playerController;
        _playerController.isMoving = true;
    }
    
    public void Handle(PlayerController playerController)
    {
        if (!_playerController.isFighting)
        {
            PlayerMove();
        }
    }

    private void PlayerMove()
    {
        // 키보드 + 조이스틱 입력
        Vector2 joystickInput = _playerController.joystick.GetInputDirection(); // 조이스틱 입력값 
        
        float horizontalInput = joystickInput.x + Input.GetAxisRaw("Horizontal");
        float verticalInput = joystickInput.y + Input.GetAxisRaw("Vertical");
        Vector3 combinedInput = new Vector3(horizontalInput, 0, verticalInput);
        
        Vector3 moveVelocity = combinedInput.normalized * _playerController.playerStats.movement_Speed * Time.deltaTime; // 이동할 벡터(속도) 계산
        Vector3 newPosition = _playerController.rigid.position + moveVelocity; // Rigidbody의 현재 위치 + 계산된 이동 벡터 = 새 위치
        _playerController.rigid.MovePosition(newPosition); // Rigidbody를 사용하여 위치를 업데이트
        
        // 애니메이션
        bool isMoving = combinedInput != Vector3.zero;
        _playerController.anim.SetBool("isMove", isMoving);
        
        if (isMoving)
        {
            _playerController.FlipPlayer(horizontalInput);
            _playerController.anim.SetBool("isMove", true);
        }
    }
    
    public void Exit(PlayerController playerController)
    {
        _playerController.anim.SetBool("isMove", false);
        _playerController.isMoving = false;
    }
}
