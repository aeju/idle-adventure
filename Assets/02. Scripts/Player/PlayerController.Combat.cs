using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    // 전투력 계산 공식
    public int CalculateCombatPower(int maxHP, int attack, int defense)
    {
        CombatPower = maxHP + attack + defense;
        return CombatPower; 
    }
}
