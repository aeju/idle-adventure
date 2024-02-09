using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class PlayerStats : MonoBehaviour
{
    public PlayerStat playerStat;

    private PlayerController _playerController;
    
    public void Start()
    {
        if (playerStat != null)
        {
            AssignStats();
        }
        else
        {
            // 경고
        }

        _playerController = GetComponentInParent<PlayerController>();
    }
    
    public int currentHP; // 현재 체력
    public int maxHP; // 생명력 
    public int attack; // 공격력 
    public int defense; // 방어력
    public int movement_Speed; // 이동속도
    
    public int critical_Hit_Rate; // 치명타 확률
    public int accuracy; // 명중 
    public int hP_Recovery; // 생명력 회복

    public string name;
    public string class_Type;
    
    public float attack_Multiplier; // 기본 공격 퍼센트
    public float critical_Multiplier; // 치명타 퍼센트
    public float skill_Multiplier; // 스킬 공격 퍼센트

    public int combatPower; // 전투력

    public void AssignStats()
    {
        maxHP = playerStat.MaxHP;
        currentHP = maxHP; // HP 초기화
        attack = playerStat.Attack;
        defense = playerStat.Defense;
        movement_Speed = playerStat.Movement_Speed;
        critical_Hit_Rate = playerStat.Critical_Hit_Rate;

        name = playerStat.Name;
        class_Type = playerStat.Class_Type;
        
        attack_Multiplier = playerStat.Attack_Multiplier;
        critical_Multiplier = playerStat.Critical_Multiplier;
        skill_Multiplier = playerStat.Skill_Multiplier;
        

        CalculateCombatPower(playerStat.MaxHP, playerStat.Attack, playerStat.Defense);
    }
    
    // 전투력 계산 공식
    public int CalculateCombatPower(int maxHP, int attack, int defense)
    {
        combatPower = maxHP + attack + defense;
        return combatPower; 
    }

    public void AttackAnim()
    {
        if (_playerController != null)
        {
            _playerController.PlayerAttackAnim();
        }
    }

    public void SkillAnim()
    {
        if (_playerController != null)
        {
            _playerController.PlayerSkillAnim();
        }
    }
    
}
