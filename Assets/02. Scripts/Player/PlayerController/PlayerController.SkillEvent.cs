using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attack, skill 애니메이션 이벤트 
public partial class PlayerController : MonoBehaviour
{
    // 기본 공격 (attack02)
    public void PlayerAttackAnim()
    {
        EnemyFSM enemyFsm = nearestMonster.GetComponent<EnemyFSM>();
        CreateAttackEffect();

        if (enemyFsm != null)
        {
            int attackDamage = CombatCalculator.CalculateAttackDamage(playerStats.attack, enemyFsm.monsterStats.Defense, playerStats.attack_Multiplier, playerStats.critical_Multiplier);
            enemyFsm.HitEnemy(attackDamage); // 일반공격 
            Debug.Log("3. HitEnemy");
        }
        else
        {
            return;
        }
    }
    
    // 스킬 공격 (attack01)
    public void PlayerSkillAnim()
    {
        // 범위 내의 몬스터들을 받아오기 (최대 10마리)
        var monsters = monstersInRange();

        // 스킬 이벤트말고, 스킬 버튼 누를 때 
        if (monsters != null)
        {
            CreateSkillEffect(); // 이펙트 생성
            
            // 각 몬스터에 대해 공격 수행
            foreach (var monster in monsters)
            {
                EnemyFSM enemyFsm = monster.GetComponent<EnemyFSM>();
            
                if (enemyFsm != null)
                {
                    int attackDamage = CombatCalculator.CalculateSkillDamage(playerStats.attack, enemyFsm.monsterStats.Defense, playerStats.skill_Multiplier);
                    enemyFsm.HitEnemy(attackDamage); // 스킬 공격
                    Debug.Log("[attack]HitEnemy");
                }
                else
                {
                    Debug.Log("No EnemyFSM");
                }
            }
        }
        
        /*
        EnemyFSM enemyFsm = nearestMonster.GetComponent<EnemyFSM>();
        CreateSkillEffect();

        if (enemyFsm != null)
        {
            int attackDamage = CombatCalculator.CalculateSkillDamage(playerStats.attack, enemyFsm.monsterStats.Defense, playerStats.skill_Multiplier);
            enemyFsm.HitEnemy(attackDamage); // 스킬공격 
            Debug.Log("3. HitEnemy");
        }
        else
        {
            return;
        }
        */
    }
}
