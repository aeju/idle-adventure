using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "Character Stats/Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    public int MaxHP;
    public int Attack;
    public int Defense;
}
