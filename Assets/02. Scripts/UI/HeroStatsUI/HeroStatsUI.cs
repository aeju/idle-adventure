using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class HeroStatsUI : MonoBehaviour
{
   public PlayerStats playerStats;
   
   public TextMeshProUGUI maxHP;
   public TextMeshProUGUI attack;
   public TextMeshProUGUI defense;
   public TextMeshProUGUI movement_Speed;
   
   public TextMeshProUGUI hero_name;
   public TextMeshProUGUI class_Type;

   public void Start()
   {
      maxHP.text = playerStats.maxHP.ToString();
      attack.text = playerStats.attack.ToString();
      defense.text = playerStats.defense.ToString();
      movement_Speed.text = playerStats.movement_Speed.ToString();
      
      hero_name.text = playerStats.name;
      class_Type.text = playerStats.class_Type;
   }
}
