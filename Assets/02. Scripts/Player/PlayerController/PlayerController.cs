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
    private IPlayerState _idleState, _moveState, _attackState, _damagedState, _dieState;

    private PlayerStateContext _playerStateContext;
    
    // 교체 필요
    public Direction CurrentMoveDirection
    {
        get;
        private set;
    }
    
    // 이전 스크립트
    
    public PlayerStats playerStats;
    
    // 애니메이션
    public Animator anim;
    
    // 상태 (생존)
    public bool isAlive = true;
    
    // terrain 
    public LayerMask terrainLayer;
    public float groundDist;
    
    // 공격
    public int CombatPower = 10; // 전투력
    
    // 쿨타임
    public float skillCooldown = 5f;
    public float lastSkillTime = -5f;
    public bool isSkillOnCooldown = false;
    
    // 조이스틱
    public FullScreenJoystick joystick;

    // 레이어 마스크 : 가장 가까운 몬스터 탐지에 필요
    public LayerMask monsterLayerMask;
    
    public GameObject nearestMonster;
    
    // 공격 이펙트
    public GameObject attackEffect;
    public GameObject skillEffect;

    void Start()
    {
        _playerStateContext = new PlayerStateContext(this);

        _idleState = gameObject.AddComponent<PlayerIdleState>();
        _moveState = gameObject.AddComponent<PlayerMoveState>();
        _attackState = gameObject.AddComponent<PlayerAttackState>();
        _damagedState = gameObject.AddComponent<PlayerDamagedState>();
        _dieState = gameObject.AddComponent<PlayerDieState>();
        
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
    
    public void AttackPlayer()
    {
        _playerStateContext.Transition(_attackState);
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
            HPSliderUpdate();
            
            if (playerStats.currentHP <= 0)
            {
                DiePlayer();
            }
            else
            {
                DamagedPlayer();
            }
        }
    }
    
    /*
   public void Move(Direction direction)
   {
       CurrentMoveDirection = direction;
       _playerStateContext.Transition(_moveState);
   }
   */
}
