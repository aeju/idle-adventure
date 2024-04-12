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

      InitUI();
   }

   private void InitUI()
   {
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
   
   public void OnEnable()
   {
      UpdateUI();
   }
   
   // 강화되는 스탯 
   private void UpdateUI()
   {
      if (playerStats != null)
      {
         SetStatsText(maxHP, playerStats.maxHP);
         SetStatsText(attack, playerStats.attack);
         SetStatsText(defense, playerStats.defense);
      }
   }
   
   private void SetStatsText(TextMeshProUGUI tmp, int statValue)
   {
      tmp.text = statValue.ToString();
   }
}
