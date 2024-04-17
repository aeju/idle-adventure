using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum AttackType
{
    Attack,
    Skill
}

// attack, skill 애니메이션 이벤트 
public partial class PlayerController : MonoBehaviour
{
    [Header("# 최대 공격 가능 몬스터 수")]
    [SerializeField] private int attackMonsterMaxCount = 5;
    [SerializeField] private int skillMonsterMaxCount = 10;
    
    
    // 기본 공격 (attack02)
    public void PlayerAttackAnim()
    {
        var attackMonsters = GetMonstersInFront(attackMonsterMaxCount);
        
        // 몬스터가 존재하는 경우, 공격 로직 수행
        if (attackMonsters.Count > 0)
        {
            HitMonsters(attackMonsters, AttackType.Attack);
            CreateAttackEffect();
        }
    }
    
    // 스킬 공격 (attack01)
    public void PlayerSkillAnim()
    {
        var skillMonsters = GetmonstersInRange(skillMonsterMaxCount);

        if (skillMonsters.Count > 0)
        {
            HitMonsters(skillMonsters,  AttackType.Skill);
            CreateSkillEffect(); 
        }
    }

    // 지정된 몬스터들 공격
    private void HitMonsters(List<GameObject> monsters, AttackType attackType)
    {
        if (monsters.Count == 0) return;

        foreach (var monster in monsters)
        {
            HitMonster(monster, attackType);
        }
    }
    
    // 개별 몬스터 공격
    private void HitMonster(GameObject monster, AttackType attackType)
    {
        EnemyFSM enemyFsm = monster.GetComponent<EnemyFSM>();
        
        if (enemyFsm != null)
        {
            int attackDamage = 0;
            
            // 공격 타입에 따라 적에게 적용할 공격력 계산
            if (attackType == AttackType.Attack)
            {
                attackDamage = CombatCalculator.CalculateAttackDamage(
                    playerStats.attack, 
                    enemyFsm.monsterStats.Defense, 
                    playerStats.attack_Multiplier, 
                    playerStats.critical_Multiplier);
            }
            
            else // AttackType.Skill
            {
                attackDamage = CombatCalculator.CalculateSkillDamage(
                    playerStats.attack, 
                    enemyFsm.monsterStats.Defense, 
                    playerStats.skill_Multiplier);
            }
            enemyFsm.HitEnemy(attackDamage);
        }
        else
        {
            Debug.LogError("No enemyFSM");
        }
    }
    
    // 기본 공격 - 바라보는 앞 방향에 있는 몬스터 탐지
    public List<GameObject> GetMonstersInFront(int attackMonsterMaxCount)
    {
        // 플레이어의 바라보는 방향 계산
        Vector3 forward = flipX ? transform.right : -transform.right;
        Vector3 searchCenter = transform.position + forward * (detectionRadius / 2);

        return SearchMonsters(searchCenter, detectionRadius / 2, attackMonsterMaxCount);
    }
    
    // 스킬 - 방향 상관x 모든 몬스터 탐지
    public List<GameObject> GetmonstersInRange(int skillMonsterMaxCount)
    {
        return SearchMonsters(transform.position, detectionRadius, skillMonsterMaxCount);
    }
    
    // 몬스터 검색/정렬 공통 메서드
    private List<GameObject> SearchMonsters(Vector3 searchCenter, float searchRadius, int maxCount)
    {
        Collider[] colliders = Physics.OverlapSphere(searchCenter, searchRadius, monsterLayerMask);
        return colliders
            .Select(collider => collider.gameObject)
            .Where(gameObject => gameObject != this.gameObject)
            .OrderBy(gameObject => (transform.position - gameObject.transform.position).sqrMagnitude)
            .Take(maxCount)
            .ToList();
    }
}
