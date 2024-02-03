using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    public int CalculateCombatPower(int maxHP, int attack, int defense)
    {
        return maxHP + attack + defense;
        
    }
}
