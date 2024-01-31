using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // 스탯
    public int MaxHP; // 생명력 
    public int Attack; // 공격력 
    public int Defense; // 방어력
    public int Critical_Hit_Rate; // 치명타 확률
    public int Accuracy; // 명중 
    public int Critical_Hit_Damage; // 치명타 피해
    public int Evasion; // 회피
    public int Attack_Speed; // 공격속도
    public int HP_Recovery; // 생명력 회복
    public int Movement_Speed = 10; // 이동속도
    public int Critical_Hit_Rate_Resist; // 치명타 확률 저항
}
