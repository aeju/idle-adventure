using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stat", menuName = "Character Stat/Player Stat")]
public class PlayerStat : StatBase
{
    public int Critical_Hit_Rate; // 치명타 확률
    public int Accuracy; // 명중 
    public int HP_Recovery; // 생명력 회복
    public string Class; // 직업 종류
    public string Character_Name;
}
