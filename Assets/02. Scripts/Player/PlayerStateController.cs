using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerController _playerController;

    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    // 상태 전환
    void Update()
    {
        if (_playerController.isAlive)
        {
            if (_playerController.AutoModeActive)
            {
                _playerController.AutoPlayer();
            }
            else 
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    _playerController.AttackPlayer();
                }
                else if (Input.GetKeyDown(KeyCode.X) && !_playerController.isSkillOnCooldown)
                {
                    _playerController.SkillPlayer();
                }
                else // idle, move 판단
                {
                    JudgeMovement(); 
                }
            }
        }
    }

    void JudgeMovement()
    {
        Vector3 combinedInput;
        bool isJoystickActive = _playerController.joystick.isDragging; 

        if (isJoystickActive) // 조이스틱
        {
            Vector2 joystickInput = _playerController.joystick.GetInputDirection();
            combinedInput = new Vector3(joystickInput.x, 0, joystickInput.y);
        }
        else // 키보드
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            combinedInput = new Vector3(horizontalInput, 0, verticalInput);
        }
        
        if (combinedInput != Vector3.zero)
        {
            _playerController.MovePlayer();
        }
        else
        {
            _playerController.IdlePlayer();
        }
    }
}
