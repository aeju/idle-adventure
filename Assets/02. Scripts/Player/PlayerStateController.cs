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

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 combinedInput = new Vector3(horizontalInput, 0, verticalInput);

        if (_playerController.isAlive)
        {
            if (combinedInput == Vector3.zero)
            {
                _playerController.IdlePlayer();
                Debug.Log("IdleState");
            }

            if (combinedInput != Vector3.zero)
            {
                _playerController.MovePlayer();
                Debug.Log("MoveState");
            }

            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
            {
                _playerController.AttackPlayer();
            }
            
            
            if (_playerController.playerStats.currentHP <= 0) 
            {
                _playerController.DiePlayer();
                
                _playerController.anim.SetTrigger("isDead"); // 애니메이션 -> 죽음
                _playerController.isAlive = false;
            }
        }
    }
}
