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
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 combinedInput = new Vector3(horizontalInput, 0, verticalInput);

        if (_playerController.isAlive)
        {
            
            if (combinedInput == Vector3.zero)
            {
                _playerController.IdlePlayer();
                //Debug.Log("IdleState");
            }

            if (combinedInput != Vector3.zero)
            {
                _playerController.MovePlayer();
                //Debug.Log("MoveState");
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                _playerController.AttackPlayer();
                Debug.Log("AttackState");
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                _playerController.SkillPlayer();
                Debug.Log("SkillState");
            }
            
            if (_playerController.playerStats.currentHP <= 0) 
            {
                _playerController.DiePlayer();
            }
        }
    }
}
