using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PonpoStats : PlayerStats
{
    void Awake()
    {
        MaxHP = 100;
        Attack = 10;
        Defense = 50;
        Critical_Hit_Rate = 100;
        Accuracy = 100;
        Critical_Hit_Damage = 100;
        Evasion = 100;
        Attack_Speed = 100;
        HP_Recovery = 10;
        HP_Recovery = 10;
        Movement_Speed = 10;
        Critical_Hit_Rate_Resist = 10;
    }
}
