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
            int attackDamage = CombatCalculator.CalculateAttackDamage(playerStats.attack, enemyFsm.monsterStats.defense, playerStats.attack_Multiplier, playerStats.critical_Multiplier);
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
        EnemyFSM enemyFsm = nearestMonster.GetComponent<EnemyFSM>();
        CreateSkillEffect();

        if (enemyFsm != null)
        {
            //int attackDamage = CombatCalculator.CalculateSkillDamage(playerStats.attack, playerStats.skill_Multiplier);
            int attackDamage = CombatCalculator.CalculateSkillDamage(playerStats.attack, enemyFsm.monsterStats.defense, playerStats.skill_Multiplier);
            enemyFsm.HitEnemy(attackDamage); // 스킬공격 
            Debug.Log("3. HitEnemy");
        }
        else
        {
            return;
        }
    }
}
