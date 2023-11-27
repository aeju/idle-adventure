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
    
    // 공격
    //public GameObject[] Monster;
    public GameObject Monster;
    public int CombatPower = 10; // 전투력
    
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();

        Monster = GameObject.FindGameObjectWithTag("monster");
    }
    
    void Update()
    {
        if (alive)
        {
            PlayerMove();
            // 수정 필요! 이동 중에는 attack x 공격부터 끝내고 수정하기
            if (Monster.GetComponent<MonsterController>().Current_HP > 0) // 몬스터가 죽지 않았을 때
            {
                anim.SetBool("isAttack", true);
            }
        }
    }

    public void PlayerMove()
    {
        // terrain raycast
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;

        float UIDist = 0.9f;
        
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist + UIDist;
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

    void PlayerAttack()
    {
        MonsterController monsterController = Monster.GetComponent<MonsterController>();
        
        if (monsterController.Current_HP > 0)
        {
            monsterController.Current_HP -= CombatPower;
        }
    }
}
