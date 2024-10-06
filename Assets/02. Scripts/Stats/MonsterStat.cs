using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster Stats", menuName = "Monster Stat/Monster Stat")]
public class MonsterStat : StatBase
{
    public int Coin;
    public int Exp;
    
    [Header("Potion Drop")]
    public int MinPotionDrop = 1;
    public int MaxPotionDrop = 5; 
}
