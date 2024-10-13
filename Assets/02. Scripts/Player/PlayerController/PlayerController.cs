using System;
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
    
    [SerializeField] private GameObject hudDamageText;
    
    [SerializeField] private Transform ponpo;
    public Rigidbody rigid;
    private SphereCollider detectionCollider;
    
    // 탐지 시간 : 1초 
    [Header("# 몬스터 탐지 ")] 
    public float detectPeriod = 1f; // idle상태, 검색 주기 
    public float detectionRadius = 5f; // 탐지 반경 설정
    
    [Header("# 플레이어 상태")]
    public bool isAlive = true; // 생존
    public bool isFlipX; // 좌우반전 
    public bool isMoving = false;
    public bool isFighting = false;
    public bool isMonsterDetected = false;
    public bool isArrived = false;
    public bool autoModeActive = false; // 자동 이동
    public bool isRespawnRequested = false;
    
    private Vector3 originPos; // 재소환 위치
    private Vector3 originScale; // 재소환 방향

    private void Awake()
    {
        // 컴포넌트 참조 초기화
        ponpo = transform.GetChild(0);
        anim = ponpo.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        
        detectionCollider = GetComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
    }

    // 상태: 필요에 따라 인스턴스화, 상태 컨텍스트(PlayerController)를 통해 관리
    void Start()
    {
        InitializeStates(); // State 패턴 초기 설정
        PlayerInit();
    }

    // 초기 1회 필요 
    void PlayerInit()
    {
        ResetPlayerState(); // 플레이어 상태 초기화
        
        // 초기 위치, 스케일(방향) 저장
        originPos = transform.position; 
        originScale = ponpo.transform.localScale; 
        
        // 몬스터 레이어 마스크 설정
        monsterLayerMask = LayerMask.GetMask("Enemy");
        
        // 체력에 대한 이벤트 구독 
        playerStats.OnPlayerHPChanged += HandlePlayerHpChange; 
        playerStats.OnPlayerHPChanged += CheckAutoPotion;
        
        // 포션매니저, 조이스틱 참조 설정
        potionManager = PotionManager.Instance;
        if (potionManager == null)
        {
            Debug.LogError("PotionManager instance not found!");
        }
        
        if (!joystick)
        {
            Debug.LogError("No joystick");
        }
    }
    
    // 리스폰 (재시작 버튼에서)
    public void Respawn()
    { 
        // 위치, 스케일 초기화
        transform.position = originPos;
        ponpo.localScale = originScale; 
        
        isRespawnRequested = false;
        
        anim.Rebind(); // 애니메이터 리셋
        
        ResetPlayerState(); // 플레이어 상태 초기화
        RespawnEffect(); // 리스폰 이펙트 실행
        SoundManager.Instance.PlaySFX("Respawn");  // 리스폰 효과음 재생
    }
    
    // 플레이어 상태 초기화 (Init, Respawn 공통 로직)
    private void ResetPlayerState()
    {
        isAlive = true; // 살아있는 상태
        isFlipX = false;
        isMoving = false;
        isFighting = false;
        isMonsterDetected = false;
        isArrived = false;
        autoModeActive = false;
        
        // HP 초기화
        playerStats.CurrentHP = playerStats.maxHP; 
        
        // 이펙트 비활성화 
        DeactivateEffects(); 
        
        // 스킬 쿨다운 초기화
        ResetSkillCooldowns();
    }
    
    // 스킬 쿨다운 초기화
    private void ResetSkillCooldowns()
    {
        isSkillOnCooldown = false;
        lastSkillTime = -skillCooldown;
        lastHitTime = -hitCooldown;
    }

    private void OnDisable()
    {
        if (autoPotionCoroutine != null)
        {
            StopCoroutine(autoPotionCoroutine);
        }
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
            float offsetDirection = isFlipX ? -1.0f : 1.0f;
            Vector3 damagePosition = transform.position + new Vector3(offsetDirection * 1.0f, 2.0f, 0);
            Utilities.CreateDamageText(hudDamageText, transform.root, enemyHitPower, damagePosition, isFlipX); // 자식으로 생성
        }
        else
        {
            Debug.LogError("HUD Damage Text prefab is not assigned");
        }
    }
    
    // FlipX 기준으로 스프라이트 방향 전환
    public void FlipPlayer(float horizontalInput)
    {
        if (horizontalInput < 0 && isFlipX || horizontalInput > 0 && !isFlipX)
        {
            Vector3 theScale = ponpo.localScale; 
            theScale.x *= -1; // ponpo의 localScale x 값을 반전시켜 방향 전환
            ponpo.localScale = theScale;
            
            isFlipX = !isFlipX; // flipX 상태 업데이트
        }
    }
}
