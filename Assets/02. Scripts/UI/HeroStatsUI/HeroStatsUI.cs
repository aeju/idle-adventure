using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
      if (playerStats == null)
         playerStats = GetComponent<PlayerStats>();
      
      if (playerStats != null)
      {
         SetStatsText(maxHP, playerStats.maxHP);
         SetStatsText(attack, playerStats.attack);
         SetStatsText(defense, playerStats.defense);
         SetStatsText(movement_Speed, playerStats.movement_Speed);

         hero_name.text = playerStats.name;
         class_Type.text = playerStats.class_Type;
      }
   }
   
   private void SetStatsText(TextMeshProUGUI tmp, int statValue)
   {
      tmp.text = statValue.ToString();
   }
}
