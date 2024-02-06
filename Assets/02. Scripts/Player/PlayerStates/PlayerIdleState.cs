using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : MonoBehaviour, IPlayerState
{
    private PlayerController _playerController;

    public void Handle(PlayerController playerController)
    {
        if (!_playerController)
        {
            _playerController = playerController;

            _playerController.isAlive = true;
            
            _playerController.anim = _playerController.GetComponent<Animator>();

            _playerController.HPSliderUpdate();

            _playerController.attackEffect.SetActive(false);
            _playerController.skillEffect.SetActive(false);
        }
    }
}
