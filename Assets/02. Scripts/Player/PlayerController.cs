using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public interface IPlayerController
{
    int currentHP  { get; set; }
    void PlayerMove();
}

// 필요 : 공격 -> Move x (state 분리)
public class PlayerController : MonoBehaviour, IPlayerController
{
    private PlayerStats playerStats;
    
    // 체력
    //private int maxHP;
    public int currentHP { get; set; }

    public Slider hpSlider;
    public Slider cooldownSlider;

    // 애니메이션
    private Animator anim;
    
    // 상태 (생존)
    public bool isAlive = true;
    
    // terrain 
    public LayerMask terrainLayer;
    public float groundDist;
    
    // 공격
    //public GameObject Monster;
    public int CombatPower; // 전투력
    
    // 쿨타임
    public float skillCooldown = 5f;
    public float lastSkillTime = -5f;
    public bool isSkillOnCooldown = false;
    
    // 조이스틱
    public FullScreenJoystick joystick;

    // 레이어 마스크 : 가장 가까운 몬스터 탐지에 필요
    public LayerMask monsterLayerMask;
    
    [SerializeField]
    private GameObject nearestMonster;
    
    // 공격 이펙트
    public Transform attackPosition;
    public Transform skillPosition;
    public GameObject attackEffect;
    public GameObject skillEffect;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            currentHP = playerStats.MaxHP;
            CombatPower = playerStats.Attack;
        }
        anim = GetComponent<Animator>();

        //Monster = GameObject.FindGameObjectWithTag("monster");
        
        //hpSlider.value = (float) currentHP / (float) playerStats.MaxHP;
        
        monsterLayerMask = LayerMask.GetMask("Enemy");
        PlayerHPSlider();
        StartCoroutine(DetectNearestMonsterCoroutine());
        
        attackEffect.SetActive(false);
    }
    
    void Update()
    {
        if (isAlive)
        {
            PlayerMove();
            
            // z : 기본 공격
            if (Input.GetKeyDown(KeyCode.Z))
            {
                PlayerAttack();
            }
            // x : 크리티컬 공격
            else if (Input.GetKeyDown(KeyCode.X))
            {
                PlayerSkill();
            }
            
            else if (Input.GetKeyDown(KeyCode.C)) // 필요시, 치트키 용으로 사용
            {
                PlayerSkill();
            }
        }
        
        // 죽음 -> 나중: GameManager에서 관리 
        if (isAlive == true && currentHP <= 0) // 죽음 1회만 처리하기 위한 플래그
        {
            // 애니메이션 -> 죽음
            anim.SetTrigger("isDead");
            isAlive = false;
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

    // 체크 시간 : 3초
    IEnumerator DetectNearestMonsterCoroutine()
    {
        while (true)
        {
            DetectAndAttackNearestMonster();
            yield return new WaitForSeconds(3f);
        }
    }
    
    // 추가: 캐릭터 정면에 있을 때만 공격 가능 
    void DetectAndAttackNearestMonster()
    {
        float detectionRadius = 5f; 
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, monsterLayerMask);

        nearestMonster = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestMonster = collider.gameObject;
            }
        }

        if (nearestMonster != null)
        {
            Debug.Log("nearestMonster:" + nearestMonster);
        }
    }
    
    // 기본 공격 (attack02)
    void PlayerAttackAnim()
    {
        Debug.Log("1. PlayerAttackAnim()");
        EnemyFSM enemyFsm = nearestMonster.GetComponent<EnemyFSM>();

        // CreateAttackEffect();
        
        if (enemyFsm != null)
        {
            Debug.Log(enemyFsm != null);
            enemyFsm.HitEnemy(CombatPower);
            Debug.Log("3. HitEnemy");
            CreateAttackEffect();
        }
    }
    
    void CreateAttackEffect()
    {
        if (attackEffect != null && attackPosition != null)
        {
            attackEffect.SetActive(true);
            // 추가 : 1초 후, 다시 SetActive(false)로 변환 
            
            //GameObject effectInstance = Instantiate(attackEffect, attackPosition.position, attackPosition.rotation);
            //effectInstance.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // 크기를 0.1로 설정
        }
    }

    // 치명타 공격 (attack01)
    void PlayerSkillAnim()
    {
        Debug.Log("1. PlayerAttackAnim()");

        EnemyFSM enemyFsm = nearestMonster.GetComponent<EnemyFSM>();
        if (enemyFsm != null)
        {
            Debug.Log(enemyFsm != null);
            enemyFsm.HitEnemy(CombatPower);
            Debug.Log("3. HitEnemy");
            //CreateAttackEffect();
        }
        
    }
    
    // 일반 공격 
    void PlayerAttack()
    {
        anim.SetTrigger("BasicAttackTrigger");
        //anim.SetTrigger("CriticalAttackTrigger");
    }
    
    // 치명타 공격 (쿨타임 10초) -> 코루틴으로 변경
    void PlayerSkill()
    {
        anim.SetTrigger("CriticalAttackTrigger");
        
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

    IEnumerator SkillCoroutine()
    {
        anim.SetTrigger("CriticalAttackTrigger");

        isSkillOnCooldown = true;
        lastSkillTime = Time.time;

        // 스킬 쿨타임 슬라이더
        {
            if (cooldownSlider != null) // 슬라이더 초기화 
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
        
    }

    // 피격 함수
    public void PlayerDamaged(int damage)
    {
        // 에너미의 공격력만큼 플레이어의 체력 깎기
        currentHP -= damage;
        // Hp슬라이더 업데이트
        PlayerHPSlider();
    }

    // 현재 플레이어 hp(%)를 hp 슬라이더의 value에 반영
    public void PlayerHPSlider()
    {
        hpSlider.value = (float) currentHP / (float) playerStats.MaxHP; 
    }
}
