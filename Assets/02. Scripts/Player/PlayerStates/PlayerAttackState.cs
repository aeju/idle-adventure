using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackState : MonoBehaviour, IPlayerState
{
    private PlayerController _playerController;

    public void Handle(PlayerController playerController)
    {
        if (!_playerController)
            _playerController = playerController;
        
        _playerController.monsterLayerMask = LayerMask.GetMask("Enemy");
        StartCoroutine(DetectNearestMonsterCoroutine());
    }

    void Update()
    {
        // 일반 공격
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerAttack();
        }

        // 스킬 공격
        if (Input.GetKeyDown(KeyCode.X))
        {
            PlayerSkill();
        }
    }
    
    // 일반 공격 
    void PlayerAttack()
    {
        _playerController.anim.SetTrigger("AttackTrigger");
    }
    
    // 치명타 공격 (쿨타임 10초) -> 코루틴으로 변경
    public void PlayerSkill()
    {
        _playerController.anim.SetTrigger("SkillTrigger");
        
        if (!_playerController.isSkillOnCooldown)
        {
            StartCoroutine(SkillCoroutine());
        }
        else
        {
            float remainCooldown = (_playerController.lastSkillTime + _playerController.skillCooldown) - Time.time;
            Debug.Log($"남은 시간: {remainCooldown}");
        }
    }
    
    
    public IEnumerator SkillCoroutine()
    {
        //_playerController.anim.SetTrigger("SkillTrigger");

        _playerController.isSkillOnCooldown = true;
        _playerController.lastSkillTime = Time.time;

        // 스킬 쿨타임 슬라이더
        {
            if (_playerController.cooldownSlider != null) // 슬라이더 초기화 
            {
                _playerController.cooldownSlider.value = 0;
                _playerController.cooldownSlider.maxValue = _playerController.skillCooldown;
            }

            // 쿨타임 동안 슬라이더 업데이트
            while (Time.time < _playerController.lastSkillTime + _playerController.skillCooldown)
            {
                if (_playerController.cooldownSlider != null)
                {
                    _playerController.cooldownSlider.value = Time.time - _playerController.lastSkillTime;
                }
                yield return null; // 다음 프레임까지 기다리도록 보장
            }
            _playerController.isSkillOnCooldown = false;

            if (_playerController.cooldownSlider != null)
            {
                _playerController.cooldownSlider.value = _playerController.cooldownSlider.maxValue;
            }
        }
    }
    
    // 이펙트, 코루틴을 파라미터로 받는 공통 함수 (일정 시간 후 비활성화)
    void ActivateEffect(GameObject effect, float duration)
    {
        if (effect != null)
        {
            effect.SetActive(true);
            StartCoroutine(DeactivateEffect(effect, duration));
        }
    }

    // 지정된 시간이 지난 후 게임 오브젝트를 비활성화하는 코루틴
    IEnumerator DeactivateEffect(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (effect != null)
        {
            effect.SetActive(false);
        }
    }
    
    // 기본 공격 이펙트
    void CreateAttackEffect()
    {
        ActivateEffect(_playerController.attackEffect, 1.5f); 
    }

    // 스킬 공격 이펙트
    void CreateSkillEffect()
    {
        ActivateEffect(_playerController.skillEffect, 1.5f); 
    }
    
    
    // 체크 시간 : 3초
    public IEnumerator DetectNearestMonsterCoroutine()
    {
        while (true)
        {
            DetectAndAttackNearestMonster();
            yield return new WaitForSeconds(3f);
        }
    }
    
    void DetectAndAttackNearestMonster()
    {
        float detectionRadius = 5f; 
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, _playerController.monsterLayerMask);

        _playerController.nearestMonster = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                _playerController.nearestMonster = collider.gameObject;
            }
        }

        if (_playerController.nearestMonster != null)
        {
            Debug.Log("nearestMonster:" + _playerController.nearestMonster);
        }
    }
    
    // 기본 공격 (attack02): 치명타 - 공격력의 175% (나중)
    void PlayerAttackAnim()
    {
        EnemyFSM enemyFsm = _playerController.nearestMonster.GetComponent<EnemyFSM>();
        CreateAttackEffect();

        if (enemyFsm != null)
        {
            int attackDamage = _playerController.CalculateAttackDamage(_playerController.playerStats.attack, _playerController.playerStats.attack_Multiplier, _playerController.playerStats.critical_Multiplier);
            enemyFsm.HitEnemy(attackDamage); // 일반공격 
            Debug.Log("3. HitEnemy");
        }
        else
        {
            return;
        }
    }
    
    // 스킬 공격 (attack01)
    void PlayerSkillAnim()
    {
        EnemyFSM enemyFsm = _playerController.nearestMonster.GetComponent<EnemyFSM>();
        CreateSkillEffect();

        if (enemyFsm != null)
        {
            int attackDamage = _playerController.CalculateSkillDamage(_playerController.playerStats.attack, _playerController.playerStats.skill_Multiplier);
            enemyFsm.HitEnemy(attackDamage); // 스킬공격 
            Debug.Log("3. HitEnemy");
        }
        else
        {
            return;
        }
    }
}
