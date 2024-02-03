using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 전투력 공식
public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    private GameObject player;
    
    private PlayerController playerStats;

    [SerializeField] private int maxHP;
    private int attack;
    private int defense;

    public int combatPower;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");

        if (player != null)
        {
            playerStats = player.GetComponent<PlayerController>();
            if (playerStats != null)
            {
                Debug.Log("playerStats.maxHP" + playerStats.maxHP);
                Debug.Log("playerStats.attack" + playerStats.attack);
                Debug.Log("playerStats.defense" + playerStats.defense);
                combatPower = CalculateCombatPower(playerStats.maxHP, playerStats.attack, playerStats.defense);
                Debug.Log("playerStats.combatPower" + combatPower);
            }
        }
        
    }

    public int CalculateCombatPower(int maxHP, int attack, int defense)
    {
        return maxHP + attack + defense;
    }
}
