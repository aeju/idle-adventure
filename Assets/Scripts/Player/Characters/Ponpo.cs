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

        // 입력값
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        float scaleFactor = 0.1f;
        
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;
        moveVelocity = moveDirection * Movement_Speed * scaleFactor;
        transform.position += moveVelocity * Movement_Speed * Time.deltaTime;
        
        // 애니메이션
        bool isMoving = moveDirection != Vector3.zero;
        
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (isMoving)
        {
            if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(-2f, 2f, -1f);
            }
            else
            {
                transform.localScale = new Vector3(2f, 2f, -1f);
            }
            anim.SetBool("isMove", true);
        }
    }
}