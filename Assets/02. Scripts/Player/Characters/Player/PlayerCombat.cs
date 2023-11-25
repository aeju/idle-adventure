using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerCombat
{
    void PlayerSkill();
    //public void PlayerSkill();
}

public class PlayerCombat : MonoBehaviour, IPlayerCombat
{
    private PlayerStats playerStats;
    
    public int Current_HP; // 현재 체력
    public float Cooldown_Time; // 쿨타임
    
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PlayerSkill()
    {
        
    }

    void PlayerCriticalSkill()
    {
        
    }

    void PlayerActiveSkill()
    {
        
    }

    void PlayerAttacked()
    {
        
    }

    void PlayerDeath()
    {
        
    }
}
