using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerController
{
    void PlayerMove();
    //bool alive { get; set; }
}

public class PlayerController : MonoBehaviour, IPlayerController
{
    private PlayerStats playerStats;
    
    // 애니메이션
    private Animator anim;
    
    // 상태 (생존)
    public bool alive = true;
    
    // terrain 
    public LayerMask terrainLayer;
    public float groundDist;
    
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (alive)
            PlayerMove();
    }

    public void PlayerMove()
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
        //moveVelocity = moveDirection * Movement_Speed * scaleFactor;
        moveVelocity = moveDirection * playerStats.Movement_Speed * scaleFactor;
        transform.position += moveVelocity * playerStats.Movement_Speed * Time.deltaTime;
        
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
    
    void PlayerAutoMove()
    {
        
    }
}
