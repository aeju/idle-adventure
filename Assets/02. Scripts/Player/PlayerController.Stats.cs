using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public partial class PlayerController : MonoBehaviour
{
    public int currentHP;
    
    public int maxHP; // 생명력 
    public int attack; // 공격력 
    public int defense; // 방어력
    protected int movement_Speed; // 이동속도
    
    protected int critical_Hit_Rate; // 치명타 확률
    protected int accuracy; // 명중 
    protected int hP_Recovery; // 생명력 회복
    
    public void AssignStats()
    {
        maxHP = statss.MaxHP;
        attack = statss.Attack;
        defense = statss.Defense;
        movement_Speed = statss.Movement_Speed;
        critical_Hit_Rate = statss.Critical_Hit_Rate;
        hP_Recovery = statss.HP_Recovery;
    }
}
