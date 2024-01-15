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
    // 체력
    public int maxHP = 100;
    public int currentHP;
    
    public Slider hpSlider;
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
    public GameObject Monster;
    public int CombatPower = 10; // 전투력
    
    // 쿨타임
    public float skillCooldown = 5f;
    public float lastSkillTime = -5f;
    public bool isSkillOnCooldown = false;
    
    // 조이스틱
    public FullScreenJoystick joystick;
    
    
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        anim = GetComponent<Animator>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Monster = GameObject.FindGameObjectWithTag("monster");

        currentHP = maxHP;
        hpSlider.value = (float) currentHP / (float) maxHP; 
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
    
    void OnDrawGizmos()
    {
        // Draw a blue line from the object's position in the direction it's facing
        Gizmos.color = Color.blue;
        //Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5f); // Adjust the length as needed
        if (Monster != null)
        {
            Gizmos.DrawLine(transform.position, transform.position + Monster.transform.position * 5f); // Adjust the length as needed
        }
        

        // Draw a red sphere at the object's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 0.2f); // Adjust the size of the sphere as needed
    }

    void PlayerAttack()
    {
        // 레이 생성한 후, 발사될 위치 + 진행 방향
        Ray ray = new Ray(transform.position, Monster.transform.position);
        
        Monster = GameObject.FindGameObjectWithTag("monster");
        // 레이가 부딪힌 대상의 정보를 저장할 변수를 생성
        RaycastHit hitInfo = new RaycastHit();

        Debug.Log("1. Lay");
        
        float radius = 1f; // Radius of the sphere cast
        float distance = 1f;
        if (Physics.SphereCast(ray, radius, out hitInfo, distance))
        // 레이를 발사하고, 만일 부딪힌 물체가 있으면
        //if (Physics.Raycast(ray, out hitInfo))
            
        {
            Debug.Log("2. Lay Hit");
            // 만일 레이에 부딪힌 대상의 레이어가 Enemy라면, 데미지 함수를 실행
            //if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            
            // 만일 레이에 부딪힌 대상의 태그가 monster라면, 데미지 함수를 실행
            if (hitInfo.transform.gameObject.tag == "monster")
            {
                Debug.Log("3. Lay Enemy Hit");
                EnemyFSM eFSM = Monster.GetComponent<EnemyFSM>();
                eFSM.HitEnemy(CombatPower);
            }
        }
        
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
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
        // 현재 플레이어 hp(%)를 hp 슬라이더의 value에 반영
        hpSlider.value = (float) currentHP / (float) maxHP; 
    }
}
