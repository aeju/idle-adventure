using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 스탯 : 생명력, 공격력, 방어력, 치명타 확률, 명중, 치명타 피해, 회피, 공격속도, 생명력 회복, 이동속도, 치명타 확률 저항
/// </summary>
public class Ponpo : MonoBehaviour
{
    // 이동 (Movement_Speed)
    Vector3 movement;
    private int direction = 1;
    
    public int Current_HP; // 현재 체력
    public float Cooldown_Time; // 쿨타임
    
    // 스탯
    public int HP; // 생명력 
    public int Attack; // 공격력 
    public int Defense; // 방어력
    public int Critical_Hit_Rate; // 치명타 확률
    public int Accuracy; // 명중 
    public int Critical_Hit_Damage; // 치명타 피해
    public int Evasion; // 회피
    public int Attack_Speed; // 공격속도
    public int HP_Recovery; // 생명력 회복
    public int Movement_Speed; // 이동속도
    public int Critical_Hit_Rate_Resist; // 치명타 확률 저항
    
    // 전투력
    public int Combat; // 전투력
    
    // 화면 출력
    public TextMeshPro _HP;
    public TextMeshPro _cur_HP;

    public bool alive = true;
    
    // terrain 
    public LayerMask terrainLayer;
    public float groundDist;
    
    void Start()
    {
        //_HP. = HP;
        //_cur_HP.text = HP;
    }

    void Update()
    {
        if (alive)
            Move();
    }

// terrain raycast 필요!
    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;
        
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            //direction = -1;
            moveVelocity = Vector3.left;

            //transform.localScale = new Vector3(direction, 1, 1);
            //if (!anim.GetBool("isJump"))
              //  anim.SetBool("isRun", true);
        }
        
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            //direction = 1;
            moveVelocity = Vector3.right;

            //transform.localScale = new Vector3(direction, 1, 1);
            //if (!anim.GetBool("isJump"))
              //  anim.SetBool("isRun", true);
        }
        
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            moveVelocity += Vector3.forward;
            //if (!anim.GetBool("isJump"))
              //  anim.SetBool("isRun", true);
        }

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            moveVelocity += Vector3.back;
            //if (!anim.GetBool("isJump"))
              //  anim.SetBool("isRun", true);
        }
        
        transform.position += moveVelocity * Movement_Speed * Time.deltaTime;
    }
    
    
}
