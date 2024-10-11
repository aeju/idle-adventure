using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;

    void Start()
    {
        if (_playerController == null)
        {
            Debug.LogError("No PlayerController");
        }
    }

    // 상태 전환
    void Update()
    {
        if (_playerController.isAlive)
        {
            if (_playerController.autoModeActive)
            {
                _playerController.AutoPlayer();
            }
            else  // AutoModeActive 아닐 때
            {
#if UNITY_EDITOR // 컴퓨터에서의 키보드 입력 처리                
                if (Input.GetKeyDown(KeyCode.Z) && _playerController.isMoving == false)
                {
                    _playerController.AttackPlayer();
                }
                else if (Input.GetKeyDown(KeyCode.X) && !_playerController.isSkillOnCooldown 
                                                     && _playerController.isMoving == false)
                {
                    _playerController.SkillPlayer();
                }
#endif
                 JudgeMovement(); // idle, move 판단 (이동 입력 유무)
            }
        }
        else if (!_playerController.isAlive && _playerController.isRespawnRequested)
        {
            _playerController.Respawn();
            _playerController.IdlePlayer();
        }
    }

    void JudgeMovement()
    {
        Vector3 combinedInput = Vector3.zero;
        bool isJoystickActive = false; 

        // 조이스틱 : 모바일, 에디터
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        isJoystickActive = _playerController.joystick.isDragging; 
        if (isJoystickActive) // 조이스틱
        {
            Vector2 joystickInput = _playerController.joystick.GetInputDirection();
            combinedInput = new Vector3(joystickInput.x, 0, joystickInput.y); // 조이스틱 입력값
        }
#endif
        
        // 키보드 : 유니티 에디터
#if UNITY_EDITOR
        if (!isJoystickActive)
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            combinedInput = new Vector3(horizontalInput, 0, verticalInput); // 키보드 입력값
        }
#endif
        
        // 입력값이 있는 경우, 플레이어 이동
        if (combinedInput != Vector3.zero)
        {
            Vector3 normalizedInput = combinedInput.normalized;
            _playerController.MovePlayer(); 
        }
        else
        {
            _playerController.IdlePlayer();
        }
    }
}
