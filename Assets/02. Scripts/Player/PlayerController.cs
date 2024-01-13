using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public interface IPlayerController
{
    void PlayerMove();
    //bool alive { get; set; }
}

public class PlayerController : MonoBehaviour, IPlayerController
{
    public Slider hPSlider;
    public Slider cooldownSlider;
    
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
    
    // 쿨타임
    public float skillCooldown = 5f;
    public float lastSkillTime = -5f;
    public bool isSkillOnCooldown = false;
    
    // 조이스틱
    public FullScreenJoystick joystick;
    
    // 체력
    public int currentHP = 100;
    
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
            
            if (Input.GetKeyDown(KeyCode.Z))
            {
                BasicAttack();
            }
            
            else if (Input.GetKeyDown(KeyCode.X))
            {
                CriticalAttack();
            }
            
            else if (Input.GetKeyDown(KeyCode.C))
            {
                if (!isSkillOnCooldown)
                {
                    StartCoroutine(SkillCoroutine());
                }
                else
                {
                    float remainCooldown = (lastSkillTime + skillCooldown) - Time.time;
                    Debug.Log($"남은 시간: {remainCooldown}");
                }
            }
        }
    }

    public void PlayerMove()
    {
        // terrain raycast
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;

        float UIDist = 0.75f;
        
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist + UIDist;
                transform.position = movePos;
            }
        }
        
        anim.SetBool("isMove", false);
        
        float horizontalInput = 0f;
        float verticalInput = 0f;
        
        // 키보드 + 조이스틱 입력을 위한 새로운 변수
        Vector3 combinedInput = Vector3.zero;;

        if (joystick.isDragging) // 조이스틱값 들어올 때만
        {
            // 조이스틱 입력값
            Vector2 joystickInput = joystick.GetInputDirection();
            combinedInput = new Vector3(joystickInput.x, 0, joystickInput.y);
            Debug.Log(combinedInput);
        }
        else // 키보드
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
            combinedInput = new Vector3(horizontalInput, 0, verticalInput);
        }
        
        Vector3 moveVelocity = combinedInput.normalized * playerStats.Movement_Speed * Time.deltaTime;
        transform.position += moveVelocity;
        
        // 애니메이션
        // bool isMoving = moveDirection != Vector3.zero;
        bool isMoving = joystick.isDragging ? joystick.GetInputDirection() != Vector2.zero : (horizontalInput != 0 || verticalInput != 0);
        
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (isMoving)
        {
            // isDragging : true -> joystick 입력값 / false -> 키보드 입력값
            float xDirectionInput = joystick.isDragging ? joystick.GetInputDirection().x : horizontalInput; 
            
            if (xDirectionInput > 0)
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
        if (monsterController == null)
        {
            Debug.LogError("MonsterController is null.");
            return;
        }

        Debug.Log("Monster HP before if: " + monsterController.Current_HP);

        if (monsterController.Current_HP > 0)
        {
            monsterController.TakeDamage(CombatPower);
        }
        else
        {
            Debug.Log("Monster is already dead or HP is zero.");
        }
        /*
    
        MonsterController monsterController = Monster.GetComponent<MonsterController>();
        Debug.Log("PlayerAttack called");
        Debug.Log("Monster HP before if:" + monsterController.Current_HP);
        
        if (monsterController != null && monsterController.Current_HP > 0)
        {
            monsterController.TakeDamage(CombatPower);
            Debug.Log("Monster HP inside if: " + monsterController.Current_HP);
            //monsterController.Current_HP -= CombatPower;
            //ShowDamageText(monsterController, CombatPower);
        }
        */
    }
    
    // 일반 공격 
    void BasicAttack()
    {
        anim.SetTrigger("AttackTrigger");
    }
    
    // 치명타 공격
    void CriticalAttack()
    {
        anim.SetTrigger("AttackTrigger");
    }
    
    // 스킬 (쿨타임 10초) -> 코루틴으로 변경
    void Skill()
    {
        anim.SetTrigger("AttackTrigger");
    }

    IEnumerator SkillCoroutine()
    {
        anim.SetTrigger("AttackTrigger");

        isSkillOnCooldown = true;
        lastSkillTime = Time.time;

        // 슬라이더 초기화 
        if (cooldownSlider != null)
        {
            cooldownSlider.value = 0;
            cooldownSlider.maxValue = skillCooldown;
        }

        // 쿨타임 동안 슬라이더 업데이트
        while (Time.time < lastSkillTime + skillCooldown)
        {
            if (cooldownSlider != null)
            {
                cooldownSlider.value = Time.time - lastSkillTime;
            }
            yield return null; // 다음 프레임까지 기다리도록 보장
        }
        isSkillOnCooldown = false;

        if (cooldownSlider != null)
        {
            cooldownSlider.value = cooldownSlider.maxValue;
        }
    }
    
    // 피격 함수
    public void PlayerDamaged(int damage)
    {
        // 에너미의 공격력만큼 플레이어의 체력 깎기
        currentHP -= damage;
    }
}
