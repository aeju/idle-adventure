using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : MonoBehaviour, IPlayerState
{
    private PlayerController _playerController;

    public void Handle(PlayerController playerController)
    {
        if (!_playerController)
        {
            _playerController = playerController;
        }
    }

    void Update()
    {
        PlayerMove();
    }
    
    public void PlayerMove()
    {
        // terrain raycast
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;

        float UIDist = 0.75f;
        
        /*
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, _playerController.terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + _playerController.groundDist + UIDist;
                transform.position = movePos;
            }
        }
        */
        
        _playerController.anim.SetBool("isMove", false);
        
        float horizontalInput = 0f;
        float verticalInput = 0f;
        
        // 키보드 + 조이스틱 입력을 위한 새로운 변수
        Vector3 combinedInput = Vector3.zero;;

        if (_playerController.joystick.isDragging) // 조이스틱값 들어올 때만
        {
            // 조이스틱 입력값
            Vector2 joystickInput = _playerController.joystick.GetInputDirection();
            combinedInput = new Vector3(joystickInput.x, 0, joystickInput.y);
            Debug.Log(combinedInput);
        }
        else // 키보드
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            combinedInput = new Vector3(horizontalInput, 0, verticalInput);
        }
        
        Vector3 moveVelocity = combinedInput.normalized * _playerController.playerStats.movement_Speed * Time.deltaTime;
        transform.position += moveVelocity;
        
        // 애니메이션
        bool isMoving = _playerController.joystick.isDragging ? _playerController.joystick.GetInputDirection() != Vector2.zero : (horizontalInput != 0 || verticalInput != 0);
        
        
        if (isMoving)
        {
            // isDragging : true -> joystick 입력값 / false -> 키보드 입력값
            float xDirectionInput = _playerController.joystick.isDragging ? _playerController.joystick.GetInputDirection().x : horizontalInput; 
            
            if (xDirectionInput > 0)
            {
                transform.localScale = new Vector3(-2f, 2f, -1f);
            }
            else
            {
                transform.localScale = new Vector3(2f, 2f, -1f);
            }
            _playerController.anim.SetBool("isMove", true);
        }
    }
}
