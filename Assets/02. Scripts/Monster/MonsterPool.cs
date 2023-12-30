using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 위치 : 고정 / 랜덤 포지션 / 스폰 포인트 / 계산 / 특정 위치 주변 
// 리스폰 시간 : 30초 
public class MonsterPool : MonoBehaviour
{
    // Singleton instance
    public static MonsterPool Instance;
    
    public GameObject monsterPrefab;
    private Queue<GameObject> monsterPool = new Queue<GameObject>();
    private Queue<GameObject> respawnQueue = new Queue<GameObject>();
    
    public int poolSize = 10;
    public float RespawnTime = 30f;

    public int activeMonstersCount = 0; // 현재 살아있는 몬스터 수 추적용

    void Awake()
    {
        // Check if an instance already exists
        if (Instance == null)
        {
            // If not, set this instance to be the Singleton instance and make it persistent
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists and it's not this one, destroy this instance
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
    
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject monster = Instantiate(monsterPrefab);
            monster.SetActive(false);
            monsterPool.Enqueue(monster);
        }
    }

    public GameObject GetMonster()
    {
        if (monsterPool.Count > 0)
        {
            GameObject monster = monsterPool.Dequeue();
            monster.SetActive(true);
            activeMonstersCount++; 
            return monster;
        }
        return null; 
    }

    // 반환되는 몬스터 
    public void ReturnMonster(GameObject monster)
    {
        //monster.SetActive(false);
        //monsterPool.Enqueue(monster);
        //activeMonstersCount--;
        StartCoroutine(RebornMonster(monster));
    }

    private IEnumerator RebornMonster(GameObject monster)
    {
        yield return new WaitForSeconds(RespawnTime); 
        monster.SetActive(true);
        //monsterPool.Enqueue(monster);
        //activeMonstersCount++;
        ReactivateMonster(monster);
    }
    
    public void ReactivateMonster(GameObject monster)
    {
        if (!monster.activeInHierarchy) // Check if the GameObject is not active before reactivating
        {
            monster.SetActive(true);
            monsterPool.Enqueue(monster);
            activeMonstersCount++;
        }
        /*
        monster.SetActive(true);
        monsterPool.Enqueue(monster);
        activeMonstersCount++;
        */
    }
    
    public void AddToRespawnQueue(GameObject monster)
    {
        respawnQueue.Enqueue(monster);
        StartCoroutine(RebornMonster(monster));
    }
}
