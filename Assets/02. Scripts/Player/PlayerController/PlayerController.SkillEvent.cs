using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attack(5마리) / 앞으로 , skill(전부) / 앞 뒤로 애니메이션 이벤트 
public partial class PlayerController : MonoBehaviour
{
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackMaximumMonsters; // 최대 공격 가능한 몬스터 수 
    [SerializeField] private float attackRadius; 
    [SerializeField] private Transform attackPosition; // 공격 위치 
    
    [SerializeField] private float skillRadius; 
    
    
    [SerializeField] private Transform skillPosition;
        
    public void DetectAndAttack()
    {
        // 공격 위치에서 지정된 반경 내에 있는 모든 콜라이더 검출
        Collider[] hitColliders = Physics.OverlapSphere(attackPosition.position, attackRadius, monsterLayerMask);

        // 검출된 몬스터들을 거리에 따라 정렬
        List<Collider> sortedMonsters = hitColliders // 거리 계산x, 거리 비교o -> sqrMagnitude
            .OrderBy(collider => (collider.transform.position - attackPosition.position).sqrMagnitude)
            .ToList();

        // 가장 가까운 몬스터부터 최대 공격 가능한 몬스터 수만큼 공격
        for (int i = 0; i < Mathf.Min(attackMaximumMonsters, sortedMonsters.Count); i++)
        {
            EnemyFSM enemyFsm = sortedMonsters[i].GetComponent<EnemyFSM>();
            if (enemyFsm != null)
            {
                int attackDamage = CombatCalculator.CalculateAttackDamage(playerStats.attack, 
                    enemyFsm.monsterStats.Defense, playerStats.attack_Multiplier, playerStats.critical_Multiplier);
                enemyFsm.HitEnemy(attackDamage); // 일반공격 
            }
        }
    }
    
    // 기본 공격 (attack02)
    public void PlayerAttackAnim()
    {
        CreateAttackEffect();
        DetectAndAttack();

        /*
        EnemyFSM enemyFsm = nearestMonster.GetComponent<EnemyFSM>();
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
        */
    }
    
    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Physics.SphereCast(transform.position, attackTest.position,)
    }
    
    /*
    public void DetectAttackEnemy()
    {
        // 레이가 부딪힌 대상의 정보를 저장할 변수
        RaycastHit hitInfo = new RaycastHit();
        
        // 레이 생성한 후, 발사될 위치 + 진행 방향
        Ray ray = new Ray(transform.position, transform.right);
        
        Monster = GameObject.FindGameObjectWithTag("monster");
        


        Debug.Log("1. Lay");
        
        float radius = 1f; // Radius of the sphere cast
        float distance = 1f;
        if (Physics.SphereCast(ray, radius, out hitInfo, distance))
        {
            Debug.Log("2. Lay Hit");
            // 만일 레이에 부딪힌 대상의 태그가 monster라면, 데미지 함수를 실행
            if (hitInfo.transform.gameObject.tag == "monster")
            {
                Debug.Log("3. Lay Enemy Hit");
                EnemyFSM eFSM = Monster.GetComponent<EnemyFSM>();
                eFSM.HitEnemy(CombatPower);
            }
        }
        
        Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
    }

    public void DetectSkillEnemy()
    {
        
        
        float detectionRadius = 5f; 
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, monsterLayerMask);

        nearestMonster = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestMonster = collider.gameObject;
            }
        }
    }
    */


   
    
    // 스킬 공격 (attack01)
    public void PlayerSkillAnim()
    {
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
    }
}
