
using System;
using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    public MonsterStat monsterStats;

    // 스탯
    public int MaxHP { get; private set; } // 생명력
    public int _currentHP;

    public int CurrentHP
    {
        get => _currentHP;
        set
        {
            if (_currentHP != value)
            { 
                _currentHP = value;
                OnEnemyHPChanged?.Invoke(_currentHP, MaxHP); // HP 변경 시 이벤트 발생 
            }
        }
    }
    public int Attack { get; private set; } // 공격력 
    public int Defense { get; private set; } // 방어력
    public int Movement_Speed { get; private set; } // 이동속도
    
    public float Attack_multiplier { get; private set; } 
    public float Critical_multiplier { get; private set; }

    public int Coin { get; private set; }
    public string MonsterName { get; private set; }
    public int Exp { get; private set; }
    
    public int MinPotionDrop { get; private set; } // 최소 포션 드롭 개수
    public int MaxPotionDrop { get; private set; } // 최대 포션 드롭 개수
    
    public event Action<int, int> OnEnemyHPChanged;

    public void Awake()
    {
        if (monsterStats != null)
        {
            AssignStats();
        }
        else
        {
            // 경고
        }
    }

    public void AssignStats()
    {
        MaxHP = monsterStats.MaxHP;
        CurrentHP = MaxHP;
        Attack = monsterStats.Attack; 
        Defense = monsterStats.Defense; 
        Movement_Speed = monsterStats.Movement_Speed;

        Attack_multiplier = monsterStats.Attack_Multiplier;
        Critical_multiplier = monsterStats.Critical_Multiplier;
        
        Coin = monsterStats.Coin;
        Exp = monsterStats.Exp;
        MonsterName = monsterStats.Name;
        
        MinPotionDrop = monsterStats.MinPotionDrop;
        MaxPotionDrop = monsterStats.MaxPotionDrop;
    }
}
