using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharecterProfile : MonoBehaviour
{
    private PlayerStat playerStat;

    public int maxHP; // 생명력 
    public int attack; // 공격력 
    public int defense; // 방어력
    public int movement_Speed; // 이동속도
    
    public int critical_Hit_Rate; // 치명타 확률
    public int accuracy; // 명중 
    public int hP_Recovery; // 생명력 회복

    public void Start()
    {
        Debug.Log("start");
        
        if (playerStat != null)
        {
            Debug.Log("!=");
            maxHP = playerStat.MaxHP;
            Debug.Log("maxHP" + maxHP);
            attack = playerStat.Attack;
            defense = playerStat.Defense;
            movement_Speed = playerStat.Movement_Speed;

            critical_Hit_Rate = playerStat.Critical_Hit_Rate;
            accuracy = playerStat.Accuracy;
            hP_Recovery = playerStat.HP_Recovery;
        }
    }
}
