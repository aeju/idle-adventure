using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attack, skill 애니메이션 이벤트 
public partial class PlayerController : MonoBehaviour
{
    // 기본 공격 (attack02)
    public void PlayerAttackAnim()
    {
        var attackMonsters = GetMonstersInFront();

        if (attackMonsters != null)
        {
            CreateAttackEffect();
            
            // 각 몬스터에 대해 공격 수행
            foreach (var monster in attackMonsters)
            {
                EnemyFSM enemyFsm = monster.GetComponent<EnemyFSM>();
                
                if (enemyFsm != null)
                {
                    int attackDamage = CombatCalculator.CalculateAttackDamage(playerStats.attack, enemyFsm.monsterStats.Defense, playerStats.attack_Multiplier, playerStats.critical_Multiplier);
                    enemyFsm.HitEnemy(attackDamage); // 일반공격 
                    Debug.Log("[attack]Hit Enemy");
                }
                else
                {
                    Debug.Log("No EnemyFSM");
                    return;
                }
            }
        }
    }
    
    // 스킬 공격 (attack01)
    public void PlayerSkillAnim()
    {
        // 범위 내의 몬스터들을 받아오기 (최대 10마리)
        var skillMonsters = monstersInRange();

        // 스킬 이벤트말고, 스킬 버튼 누를 때 
        if (skillMonsters != null)
        {
            CreateSkillEffect(); // 이펙트 생성
            
            // 각 몬스터에 대해 공격 수행
            foreach (var monster in skillMonsters)
            {
                EnemyFSM enemyFsm = monster.GetComponent<EnemyFSM>();
            
                if (enemyFsm != null)
                {
                    int attackDamage = CombatCalculator.CalculateSkillDamage(playerStats.attack, enemyFsm.monsterStats.Defense, playerStats.skill_Multiplier);
                    enemyFsm.HitEnemy(attackDamage); // 스킬 공격
                    Debug.Log("[skill]HitEnemy");
                }
                else
                {
                    Debug.Log("No EnemyFSM");
                }
            }
        }
    }
}
