using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// attack, skill 애니메이션 이벤트 
public partial class PlayerController : MonoBehaviour
{
    [SerializeField] private int attackMonsterMaxCount = 5;
    [SerializeField] private int skillMonsterMaxCount = 10;
    
    
    // 기본 공격 (attack02)
    public void PlayerAttackAnim()
    {
        // 내 위치 앞쪽의 몬스터들을 받아오기 (최대 5마리)
        var attackMonsters = GetMonstersInFront(attackMonsterMaxCount);

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
        // var skillMonsters = monstersInRange();
        var skillMonsters = monstersInRange(skillMonsterMaxCount);

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

    private void AttackMonsters(List<GameObject> monsters)
    {
        
    }
    
    
    
    // 앞,뒤 모든 몬스터 10마리 메
    //public List<GameObject> monstersInRange()
    public List<GameObject> monstersInRange(int skillMonsterMaxCount)
    {
        List<GameObject> skillMonsters = new List<GameObject>();

        // 현재 위치에서 detectionRadius 내의 모든 콜라이더를 검색
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, monsterLayerMask);
        
        // 거리에 따라 몬스터 리스트를 정렬 (sqrtMagnitude : 두 오브젝트 단순 거리 비교)
        skillMonsters = colliders
            .Select(collider => collider.gameObject) // 검색된 콜라이더에서 게임 오브젝트 추출
            .Where(gameObject => gameObject != this.gameObject) // 플레이어 자신은 제외
            .OrderBy(gameObject => (transform.position - gameObject.transform.position).sqrMagnitude) // 거리에 따라 정렬
            //.Take(10) // 최대 10마리의 몬스터만 반환
            .Take(skillMonsterMaxCount) // 최대 10마리의 몬스터만 반환
            .ToList();
        
        return skillMonsters;
    }
    
    // 플레이어가 바라보는 앞 방향으로만 몬스터를 탐지
    //public List<GameObject> GetMonstersInFront()
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
            //.Take(5) // 최대 5마리까지
            .Take(attackMonsterMaxCount) // 최대 5마리까지
            .ToList();

        return attackMonsters;
    }
}
