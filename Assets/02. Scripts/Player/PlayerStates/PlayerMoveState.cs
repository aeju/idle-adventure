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
        
        if (_playerController.playerStats == null)
        {
            Debug.LogError("PlayerStats is null");
        }
    }
    
    public void Handle(PlayerController playerController)
    {
        _playerController = playerController;
        PlayerMove();
    }

    public void PlayerMove()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        
        // 키보드 + 조이스틱 입력을 위한 새로운 변수
        Vector3 combinedInput = new Vector3(horizontalInput, 0, verticalInput);

        /*
        if (_playerController.joystick.isDragging) // 조이스틱값 들어올 때만
        {
            // 조이스틱 입력값
            Vector2 joystickInput = _playerController.joystick.GetInputDirection();
            combinedInput = new Vector3(joystickInput.x, 0, joystickInput.y);
            Debug.Log(combinedInput);
        }
        */
        //else // 키보드
        // 

        Vector3 moveVelocity = combinedInput.normalized * _playerController.playerStats.movement_Speed * Time.deltaTime;
        //transform.position += moveVelocity;
        _playerController.transform.position += moveVelocity;
        
        // 애니메이션
        bool isMoving = (horizontalInput != 0 || verticalInput != 0);
        //bool isMoving = _playerController.joystick.isDragging ? _playerController.joystick.GetInputDirection() != Vector2.zero : (horizontalInput != 0 || verticalInput != 0);
        
        if (isMoving)
        {
            float xDirectionInput = horizontalInput; 
            
            if (xDirectionInput > 0)
            {
                //_playerController.transform.localScale = new Vector3(-2f, 2f, -1f);
                _playerController.ponpo.localScale = new Vector3(-2f, 2f, -1f);
                
                //_playerController.transform.localScale = new Vector3(-1f, 1f, -1f);
                
                //_playerController.flipExclude.localScale = new Vector3(-0.02f, 0.02f, 1);
                //_playerController.flipExclude.localScale = new Vector3(-_playerController.flipExclude.localScale.x, _playerController.flipExclude.localScale.y, -1f);
                
                //_playerController.flipX = false;
                //ExcludeFlip();
            }
            else
            {
                _playerController.ponpo.localScale = new Vector3(2f, 2f, -1f);
                //_playerController.transform.localScale = new Vector3(1f, 1f, -1f);
                _playerController.flipX = true;
            }
            _playerController.anim.SetBool("isMove", true);
            //_playerController.SliderRight();
            //_playerController.transform.localScale = xDirectionInput > 0 ? new Vector3(-2f, 2f, -1f) : new Vector3(2f, 2f, -1f);
        }

        /*
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
        
        else
        {
            _playerController.anim.SetBool("isMove", false);
        }
        /*
    }
    
    // 사용 x
    void AdjustTerrain()
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
    }
    public void Exit(PlayerController playerController)
    {
        _playerController.anim.SetBool("isMove", false);
    }

    /*
    public void ExcludeFlip()
    {
        if (_playerController.flipExclude)
        {
            _playerController.flipExclude.localScale = new Vector3(-_playerController.flipExclude.localScale.x, _playerController.flipExclude.localScale.y, -1f);
            //_playerController.flipExclude.localScale = new Vector3(-_playerController.flipExclude.localScale.x, 1, 1);
        }
    }
    */
}
