using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전투력 공식
public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }
    
    public PlayerStats playerStats;
    
    public int maxHP;
    public int attack;
    public int defense;

    public int combatPower;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        maxHP = playerStats.MaxHP;
        attack = playerStats.Attack;
        defense = playerStats.Defense;

        combatPower = CalculateCombatPower(maxHP, attack, defense);
    }

    public int CalculateCombatPower(int maxHP, int attack, int defense)
    {
        return maxHP + attack + defense;
    }
}
