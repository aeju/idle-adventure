using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 랜덤 위치 생성
// 같은 spawnpoint에 생기지 x
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }
    
    // 현재 시간
    private float currentTime;
    // 생성 시간
    public float createTime = 1;
    // 생성할 최소 시간
    public float minTime = 0.5f;
    // 생성할 최대 시간
    public float maxTime = 1.5f;
    // 적 공장
    public GameObject enemyFactory;
    
    // 오브젝트 풀 크기
    public int poolSize = 10;
    // 오브젝트 풀 배열
    public List<GameObject> enemyObjectPool;
    // SpawnPoint들
    public Transform[] spawnPoints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        createTime = Random.Range(minTime, maxTime);
        // 오브젝트 풀 생성
        enemyObjectPool = new List<GameObject>();
        
        // 오브젝트 풀에 넣을 에너미 개수만큼 반복해서
        for (int i = 0; i < poolSize; i++)
        {
            // 에너미 공장에서 에너미 생성
            GameObject enemy = Instantiate(enemyFactory);
            // 에너미를 오브젝트 풀에 넣기
            enemyObjectPool.Add(enemy);
            // 비활성화
            enemy.SetActive(false);
        }
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        // 생성 시간이 돼서
        if (currentTime > createTime)
        {
            // 오브젝트풀에 에너미가 있다면
            if (enemyObjectPool.Count > 0)
            {
                // 오브젝트풀에서 enemy를 가져다 사용
                GameObject enemy = enemyObjectPool[0];
                // 오브젝트풀에서 에너미 제거
                enemyObjectPool.Remove(enemy);
                // 랜덤으로 위치 선택
                int index = Random.Range(0, spawnPoints.Length);
                // 에너미 위치
                enemy.transform.position = spawnPoints[index].position;
                // 에너미 활성화
                enemy.SetActive(true);
            }
            
            createTime = Random.Range(minTime, maxTime);
            currentTime = 0;
        }
    }
}
