using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public interface IPlayerController
{
    //void PlayerMove();
}

// 필요 : 공격 -> Move x (state 분리)
public partial class PlayerController : MonoBehaviour, IPlayerController
{
    // State Pattern 적용을 위해 추가 
    private IPlayerState _idleState, _moveState, _attackState, _skillState, _damagedState, _dieState;

    private PlayerStateContext _playerStateContext;
    
    public PlayerStats playerStats;
    
    // 애니메이션
    public Animator anim;
    
    // 상태 (생존)
    public bool isAlive = true;
    
    // terrain 
    public LayerMask terrainLayer;
    public float groundDist;
    
    // 조이스틱
    public FullScreenJoystick joystick;
    
    // 슬라이더
    public Slider hpSlider;
    public Slider cooldownSlider;

    // 가장 가까운 몬스터 탐지
    public LayerMask monsterLayerMask; // 레이어 마스크 
    public GameObject nearestMonster;
    
    public GameObject hudDamageText;

    public bool flipX;

    //public List<Transform> FlipExclude;
    //public Transform flipExclude; // 슬라이더바 
    //public RectTransform flipExclude;

    public Transform ponpo;

    // 상태: 필요에 따라 인스턴스화, 상태 컨텍스트(PlayerController)를 통해 관리
    void Start()
    {
        ponpo = transform.GetChild(0);

        // 상태 객체: 인스턴스화 필요 (일반 클래스 인스턴스로 생성)
        _idleState = new PlayerIdleState(); 
        _moveState = new PlayerMoveState();
        _attackState = new PlayerAttackState();
        _skillState = new PlayerSkillState();
        _damagedState = new PlayerDamagedState();
        _dieState = new PlayerDieState();
        
        // 상태 관리자 인스턴스 생성 및 초기 상태로 전환
        _playerStateContext = new PlayerStateContext(this);
        _playerStateContext.Transition(_idleState);

        PlayerInit();
    }

    void PlayerInit()
    {
        // 초기 1회 필요 
       
        anim = GetComponent<Animator>();
        //anim = ponpo.GetComponent<Animator>();
        
        isAlive = true;
        flipX = false;
        DeactivateEffects();
        //HPSliderUpdate();
        HPSliderUpdate(hpSlider, playerStats.currentHP, playerStats.maxHP);
        monsterLayerMask = LayerMask.GetMask("Enemy");
        StartCoroutine(DetectNearestMonsterCoroutine());
    }

    public void IdlePlayer()
    {
        _playerStateContext.Transition(_idleState);
    }
    
    public void MovePlayer()
    {
        _playerStateContext.Transition(_moveState);
    }
    
    public void AttackPlayer()
    {
        _playerStateContext.Transition(_attackState);
    }
    
    public void SkillPlayer()
    {
        _playerStateContext.Transition(_skillState);
    }
    
    public void DamagedPlayer()
    {
        _playerStateContext.Transition(_damagedState);
    }
    
    public void DiePlayer()
    {
        _playerStateContext.Transition(_dieState);
    }
    
    // EnemyFSM에서 공격할 때, 호출 (->_damageState)
    public void ReceiveDamage(int damage)
    {
        if (isAlive && playerStats.currentHP > 0)
        {
            playerStats.currentHP -= damage;
            CreateDamageText(damage);
            HPSliderUpdate(hpSlider, playerStats.currentHP, playerStats.maxHP);
        }

        else
        {
            DiePlayer();
        }
    }
    
    // 체크 시간 : 3초
    public IEnumerator DetectNearestMonsterCoroutine()
    {
        while (true)
        {
            DetectAndAttackNearestMonster();
            yield return new WaitForSeconds(3f);
        }
    }
    
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
    }

    void HPSliderUpdate(Slider hpSlider, int currentHP, int maxHP)
    {
        CombatUtilities.HPSliderUpdate(hpSlider, playerStats.currentHP, playerStats.maxHP);
    }
    
    void CreateDamageText(int hitPower)
    {
        if (hudDamageText != null) // 데미지 텍스트 
        {
            // flipX을 기준으로 위치 계산
            float offsetDirection = flipX ? -1.0f : 1.0f;
            Vector3 damagePosition = transform.position + new Vector3(1.0f, 2.0f, 0);
            //Vector3 damagePosition = transform.position + new Vector3(offsetDirection * 1.0f, 2.0f, 0);
            //GameObject damageText = Instantiate(hudDamageText, damagePosition, Quaternion.identity, transform.root); // 자식으로 생성
            
            GameObject damageText = Instantiate(hudDamageText, damagePosition, Quaternion.identity, transform); // 자식으로 생성
            
            Vector3 damageScale = new Vector3(offsetDirection * 1.0f, 1.0f, 1.0f);
            damageText.transform.localScale = damageScale;
            
            damageText.GetComponent<DamageText>().damage = hitPower;
        }
    }

    public void SliderRight()
    {
        if (!flipX)
        {
            hpSlider.direction = Slider.Direction.RightToLeft;
            cooldownSlider.direction = Slider.Direction.RightToLeft;
        }
        else
        {
            hpSlider.direction = Slider.Direction.LeftToRight;
            cooldownSlider.direction = Slider.Direction.RightToLeft;
        }
    }
}
