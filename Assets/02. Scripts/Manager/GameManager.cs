using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터 소환, 배치 
public class GameManager : MonoBehaviour
{
    public MonsterPool monsterPool;
    public MapEditor mapEditor;
    
    public Vector3 spawnPosition = Vector3.zero; // 몬스터를 소환할 위치

    void Start()
    {
        SpawnMonsters();
        //SpawnMonstersAtPoints();
        
    }
    
    public void ReturnMonster(GameObject monster)
    {
        monsterPool.activeMonstersCount--;
        StartCoroutine(RebornMonster(monster));
    }

    private IEnumerator RebornMonster(GameObject monster)
    {
        yield return new WaitForSeconds(monsterPool.RespawnTime);
        monsterPool.ReactivateMonster(monster);
    }
    

    void SpawnMonsters()
    {
        for (int i = 0; i < monsterPool.poolSize; i++)
        {
            GameObject monster = MonsterPool.Instance.GetMonster();
            
            //GameObject monster = monsterPool.GetMonster();
            if (monster != null)
            {
                monster.transform.position = GetSpawnPosition(i);
                monster.SetActive(true);
            }
        }
    }

    // 작업 중 : 몬스터 소환 위치 
    Vector3 GetSpawnPosition(int index)
    {
        // 임시 : 일렬로 배치
        return spawnPosition + new Vector3(index * 2.0f, 0, 0); 
    }
    
    void SpawnMonstersAtPoints()
    {
        foreach (var point in mapEditor.monsterSpawnPoints)
        {
            GameObject monster = monsterPool.GetMonster();
            if (monster != null)
            {
                monster.transform.position = point;
                monster.SetActive(true);
                // Additional setup for the monster can be done here
            }
        }
    }
}
