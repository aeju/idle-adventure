using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 금화 금액 
[CreateAssetMenu(fileName = "New Monster Stats", menuName = "Monster Stat/Monster Stat")]
public class MonsterStat : StatBase
{
    public string Monster_Name;
    public int Coin;
}
