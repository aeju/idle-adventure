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
            if (Input.GetKeyDown(KeyCode.Z))
            {
                _playerController.AttackPlayer();
                Debug.Log("AttackState");
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                _playerController.SkillPlayer();
                Debug.Log("SkillState");
            }
            else if (combinedInput != Vector3.zero) // 터치 조이스틱도 추가해야! 
            {
                _playerController.MovePlayer();
            }
            else if (0 < _playerController.playerStats.currentHP ||
                     _playerController.playerStats.currentHP < _playerController.playerStats.maxHP)
            {
                _playerController.DamagedPlayer();
            }
            else if (_playerController.playerStats.currentHP <= 0)
            {
                _playerController.DiePlayer();
            }
            else
            {
                _playerController.IdlePlayer();
            }
        }
    }
}
