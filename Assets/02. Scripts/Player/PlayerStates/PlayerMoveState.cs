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
    }
    
    public void Handle(PlayerController playerController)
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        // 키보드 + 조이스틱 입력
        Vector2 joystickInput = _playerController.joystick.GetInputDirection(); // 조이스틱 입력값 
        
        float horizontalInput = joystickInput.x + Input.GetAxisRaw("Horizontal");
        float verticalInput = joystickInput.y + Input.GetAxisRaw("Vertical");
        Vector3 combinedInput = new Vector3(horizontalInput, 0, verticalInput);
        
        Vector3 moveVelocity = combinedInput.normalized * _playerController.playerStats.movement_Speed * Time.deltaTime;
        _playerController.transform.position += moveVelocity;

        // 애니메이션
        bool isMoving = combinedInput != Vector3.zero;
        _playerController.anim.SetBool("isMove", isMoving);
        
        if (isMoving)
        {
            if (horizontalInput > 0)
            {
                _playerController.ponpo.localScale = new Vector3(-2f, 2f, -1f);
            }
            else if (horizontalInput < 0)
            {
                _playerController.ponpo.localScale = new Vector3(2f, 2f, -1f);
                _playerController.flipX = true;
            }
            _playerController.anim.SetBool("isMove", true);
        }
    }
    
    public void Exit(PlayerController playerController)
    {
        _playerController.anim.SetBool("isMove", false);
    }
}
