using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IPlayerCharacter
{
    void PlayerMove();
    //public void PlayerSkill();
}

/// <summary>
/// 스탯 : 생명력, 공격력, 방어력, 치명타 확률, 명중, 치명타 피해, 회피, 공격속도, 생명력 회복, 이동속도, 치명타 확률 저항
/// </summary>
public class Player : MonoBehaviour, IPlayerCharacter
{
    public void PlayerMove()
    {
        
    }
}
