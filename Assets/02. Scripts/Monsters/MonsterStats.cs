using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterStats : MonoBehaviour
{
    public MonsterStat monsterStats;

    // 스탯
    public int maxHP; // 생명력
    public int currentHP;
    public int attack; // 공격력 
    public int defense; // 방어력
    public int movement_Speed; // 이동속도
    
    public int coin;
    public string monsterName;

    public void Start()
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
        maxHP = monsterStats.MaxHP;
        currentHP = maxHP;
        attack = monsterStats.Attack; 
        defense = monsterStats.Defense; 
        movement_Speed = monsterStats.Movement_Speed; // 이동속도
    
        coin = monsterStats.Coin;
        monsterName = monsterStats.Monster_Name;
    }
}
