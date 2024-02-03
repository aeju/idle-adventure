using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public partial class PlayerController : MonoBehaviour
{
    public int currentHP; // 현재 체력
    
    public int maxHP; // 생명력 
    public int attack; // 공격력 
    public int defense; // 방어력
    protected int movement_Speed; // 이동속도
    
    protected int critical_Hit_Rate; // 치명타 확률
    protected int accuracy; // 명중 
    protected int hP_Recovery; // 생명력 회복
    
    public void AssignStats()
    {
        maxHP = playerStats.MaxHP;
        currentHP = maxHP; // HP 초기화
        attack = playerStats.Attack;
        defense = playerStats.Defense;
        movement_Speed = playerStats.Movement_Speed;
        critical_Hit_Rate = playerStats.Critical_Hit_Rate;
        hP_Recovery = playerStats.HP_Recovery;
    }
}
