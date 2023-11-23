using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Ponpo : MonoBehaviour, IPlayerController
{
    private IPlayerController playerController;

    // 전투력
    public int Combat; // 전투력

    // 화면 출력
    public TextMeshPro _HP;

    public TextMeshPro _cur_HP;

    // 상태 (생존)
    public bool alive = true;

    void Update()
    {
        if (playerController != null)
        {
            playerController.PlayerMove();
        }
    }

    public void PlayerMove()
    {
        
    }
}