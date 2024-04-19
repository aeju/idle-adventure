using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private IPlayerState _idleState, _moveState, _autoState, _attackState, _skillState, _damagedState, _dieState;
    
    private PlayerStateContext _playerStateContext;
    
    public PlayerStats playerStats;
    
    // 애니메이션
    public Animator anim;
    
    // 조이스틱
    public FullScreenJoystick joystick;
    
    // 슬라이더
    public Slider hpSlider;
    public Slider cooldownSlider;
    
    public GameObject hudDamageText;
    
    public Transform ponpo;
    public Rigidbody rigid;
    
    [SerializeField] private float detectionRadius = 5f; // 탐지 반경 설정
    
    [Header("# 플레이어 상태")]
    public bool isAlive = true; // 생존
    public bool flipX; // 좌우반전 
    public bool autoModeActive; // 자동 공격

    // 상태: 필요에 따라 인스턴스화, 상태 컨텍스트(PlayerController)를 통해 관리
    void Start()
    {
        ponpo = transform.GetChild(0);
        anim = ponpo.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        if (!joystick)
        {
            Debug.LogError("No joystick");
        }
        
        InitializeStates(); // State 패턴 초기 설정
        PlayerInit();
    }

    void PlayerInit()
    {
        // 초기 1회 필요 
        isAlive = true;
        flipX = false;
        DeactivateEffects();
        monsterLayerMask = LayerMask.GetMask("Enemy");
        playerStats.OnPlayerHPChanged += HandlePlayerHpChange; // 체력에 대한 이벤트 구독
        
        isSkillOnCooldown = false;
        lastSkillTime = -skillCooldown;
        lastHitTime = -hitCooldown;
    }
    
    // 상태 객체와 상태 관리자 인스턴스를 초기화
    private void InitializeStates()
    {
        // 상태 객체: 인스턴스화 필요 (일반 클래스 인스턴스로 생성)
        _idleState = new PlayerIdleState(); 
        _moveState = new PlayerMoveState();
        _autoState = new PlayerAutoState();
        _attackState = new PlayerAttackState();
        _skillState = new PlayerSkillState();
        _damagedState = new PlayerDamagedState();
        _dieState = new PlayerDieState();

        // 상태 관리자 인스턴스 생성 및 초기 상태로 전환
        _playerStateContext = new PlayerStateContext(this);
        _playerStateContext.Transition(_idleState);
    }

    public void IdlePlayer()
    {
        _playerStateContext.Transition(_idleState);
    }
    
    public void MovePlayer()
    {
        _playerStateContext.Transition(_moveState);
    }

    public void AutoPlayer()
    {
        _playerStateContext.Transition(_autoState);
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
        if (!isAlive) return;
        _playerStateContext.Transition(_dieState);
    }
    
    // EnemyFSM에서 공격할 때, 호출 (->_damageState)
    public void ReceiveDamage(int damage)
    {
        if (isAlive && playerStats.CurrentHP > 0)
        {
            playerStats.CurrentHP -= damage;
            CreateDamageText(damage);

            if (playerStats.CurrentHP <= 0)
            {
                DiePlayer();
            }
            
            else
            {
                DamagedPlayer();
            }
        }
    }

    void HandlePlayerHpChange(int currentHP, int maxHP)
    {
        Debug.Log($"[Player] Handling HP Change. New HP: {currentHP}/{maxHP}");
        Utilities.HPSliderUpdate(hpSlider, playerStats.CurrentHP, playerStats.maxHP);
    }
    
    void CreateDamageText(int enemyHitPower)
    {
        if (hudDamageText != null) // 데미지 텍스트 
        {
            // flipX을 기준으로 위치 계산
            float offsetDirection = flipX ? -1.0f : 1.0f;
            Vector3 damagePosition = transform.position + new Vector3(offsetDirection * 1.0f, 2.0f, 0);
            Utilities.CreateDamageText(hudDamageText, transform.root, enemyHitPower, damagePosition, flipX); // 자식으로 생성
        }
        else
        {
            Debug.LogError("HUD Damage Text prefab is not assigned");
        }
    }
    
    // FlipX 기준으로 스프라이트 방향 전환
    public void FlipPlayer(float horizontalInput)
    {
        if (horizontalInput < 0 && flipX || horizontalInput > 0 && !flipX)
        {
            Vector3 theScale = ponpo.localScale; 
            theScale.x *= -1; // ponpo의 localScale x 값을 반전시켜 방향 전환
            ponpo.localScale = theScale;
            
            flipX = !flipX; // flipX 상태 업데이트
        }
    }
}
