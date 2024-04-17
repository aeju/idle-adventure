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
    
    // 기본 공격 - 플레이어가 바라보는 앞 방향으로만 몬스터 탐지
    public List<GameObject> GetMonstersInFront(int attackMonsterMaxCount)
    {
        List<GameObject> attackMonsters = new List<GameObject>();
        
        // 플레이어의 바라보는 방향 계산
        Vector3 forward = flipX ? transform.right : -transform.right;
        Vector3 center = transform.position + forward * (detectionRadius / 2);
        
        Collider[] colliders = Physics.OverlapSphere(center, detectionRadius / 2, monsterLayerMask);
        
        attackMonsters = colliders
            .Select(collider => collider.gameObject)
            .Where(gameObject => gameObject != this.gameObject) // 플레이어 자신은 제외
            .OrderBy(gameObject => (transform.position - gameObject.transform.position).sqrMagnitude) // 거리에 따라 정렬
            .Take(attackMonsterMaxCount) // 최대 5마리까지
            .ToList();

        return attackMonsters;
    }
    
    // 스킬 - 앞,뒤 모든 몬스터 탐지
    public List<GameObject> GetmonstersInRange(int skillMonsterMaxCount)
    {
        List<GameObject> skillMonsters = new List<GameObject>();

        // 현재 위치에서 detectionRadius 내의 모든 콜라이더를 검색
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, monsterLayerMask);
        
        // 거리에 따라 몬스터 리스트를 정렬 (sqrtMagnitude : 두 오브젝트 단순 거리 비교)
        skillMonsters = colliders
            .Select(collider => collider.gameObject) // 검색된 콜라이더에서 게임 오브젝트 추출
            .Where(gameObject => gameObject != this.gameObject) // 플레이어 자신은 제외
            .OrderBy(gameObject => (transform.position - gameObject.transform.position).sqrMagnitude) // 거리에 따라 정렬
            .Take(skillMonsterMaxCount) // 최대 10마리의 몬스터만 반환
            .ToList();
        
        return skillMonsters;
    }
}
