using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Queue 사용
// 스폰된 위치에서 몬스터 생성
// 죽고나서, 30초 후 같은 지점에서 생성
public class PoolManager : MonoBehaviour
{
    // 풀 담당 리스트 (변수 개수와 1:1 관계)
    private List<GameObject>[] pools;
    
    public GameObject[] prefabs;
    
    /*
    // 프리팹 보관할 변수
    public EnemyFSM monsterPrefab;
    public int poolSize = 10;
    
    private Queue<EnemyFSM> pool = new Queue<EnemyFSM>();
    */
    
    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // 선택한 풀의 놀고 (비활성화 된) 있는 게임오브젝트 접근
        foreach (GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                // 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        
        // 모두 쓰고 있다면 (= 못 찾았으면?)
        if (!select)
        {
            // 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        
        return select;
    }
    

    /*
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            EnemyFSM monster = Instantiate(monsterPrefab);
            monster.Deactivate();
            pool.Enqueue(monster);
        }
    }

    public EnemyFSM GetMonster()
    {
        if (pool.Count > 0)
        {
            EnemyFSM monster = pool.Dequeue();
            monster.Activate();
            return monster;
        }

        else // 풀이 비었다면 새로운 몬스터 생성
        {
            EnemyFSM monster = Instantiate(monsterPrefab);
            monster.Activate();
            return monster;
        }
    }
    
    public void ReturnMonster(EnemyFSM monster)
    {
        monster.Deactivate();
        pool.Enqueue(monster);
    }
    */
}
