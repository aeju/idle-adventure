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
    
    // terrain 
    public LayerMask terrainLayer;
    public float groundDist;
    
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

    // 상태 (생존)
    public bool alive = true;
    
    // 애니메이션
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (alive)
            Move();
    }

// terrain raycast 필요!
    void Move()
    {
        // terrain raycast
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist;
                transform.position = movePos;
            }
        }
        
        Vector3 moveVelocity = Vector3.zero;
        anim.SetBool("isMove", false);
        
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            direction = -1;
            moveVelocity = Vector3.left;
            
            anim.SetBool("isMove", true);
            //sif (!anim.GetBool("isJump"))
              //  anim.SetBool("isRun", true);
        }
        
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
            
            anim.SetBool("isMove", true);
            //if (!anim.GetBool("isJump"))
              //  anim.SetBool("isRun", true);
        }
        
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            moveVelocity += Vector3.forward;
            
            anim.SetBool("isMove", true);
            //if (!anim.GetBool("isJump"))
              //  anim.SetBool("isRun", true);
        }

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            moveVelocity += Vector3.back;
            
            anim.SetBool("isMove", true);
            //if (!anim.GetBool("isJump"))
              //  anim.SetBool("isRun", true);
        }
        
        transform.position += moveVelocity * Movement_Speed * Time.deltaTime;
    }
    
    
}
