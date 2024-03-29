using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    // public static EnemyManager Instance { get; private set; }
    private float currentTime; // 경과 시간 추적
    public GameObject enemyFactory;

    // 다음 적 생성까지의 시간
    public float createTime = 1;

    // 적 생성 최소 대기 시간
    public float minTime = 0.5f;

    // 적 생성 최대 대기 시간
    public float maxTime = 1.5f;
    
    // 최대 몬스터 수 (오브젝트 풀 크기)
    public int maxMonsters = 10;

    // 오브젝트 풀 배열 (생성된 적 보관)
    public List<GameObject> enemyObjectPool;

    // 적이 생성될 위치 (SpawnPoint들)
    public Transform[] spawnPoints;

    // 사용된 spawnPoints 인덱스 추적을 위한 리스트
    private List<int> usedSpawnPoints  = new List<int>();
    
    void Start()
    {
        CreateMonsterPool();
    }

    // 오브젝트 풀 생성
    void CreateMonsterPool()
    {
        enemyObjectPool = new List<GameObject>();

        // 오브젝트 풀에 넣을 에너미 개수만큼 반복해서
        for (int i = 0; i < maxMonsters; i++)
        {
            // 몬스터 프리팹을 인스턴스화하여 생성 (에너미 공장에서 에너미 생성)
            GameObject enemy = Instantiate(enemyFactory);
            enemy.name = $"Monster_{i + 1:00}"; // 몬스터 이름 지정 (Monster_01 ... )
            enemy.SetActive(false); // 초기 상태 : 비활성화
            
            enemyObjectPool.Add(enemy); // 오브젝트 풀에 몬스터 추가
        }
    }

    void Update()
    {
        // 시간 업데이트
        currentTime += Time.deltaTime;

        // 지정된 시간이 지나면 적 생성
        if (currentTime > createTime)
        {
            SpawnMonster();
            createTime = Random.Range(minTime, maxTime); // 다음 생성 시간 랜덤 설정
            currentTime = 0; // 시간 초기화
        }
    }

    // 적 생성 함수
    void SpawnMonster()
    {
        if (enemyObjectPool.Count > 0) // 오브젝트 풀에 사용 가능한 에너미가 있다면
        {
            // 사용 가능한 spawnPoint 인덱스 가져오기
            int index = GetRandomSpawnPoint();
            
            // 유효한 인덱스가 있다면 
            if (index != -1)
            {
                GameObject enemy = enemyObjectPool[0]; // 오브젝트 풀에서 enemy를 가져다 사용
                enemyObjectPool.Remove(enemy); // 오브젝트 풀에서 에너미 제거
                enemy.transform.position = spawnPoints[index].position; // 에너미 위치 설정
                enemy.SetActive(true); // 에너미 활성화
                
                // 사용한 인덱스 기록
                usedSpawnPoints.Add(index);
            }
        }
    }

    // 사용 가능한 spawnPoint 인덱스 반환 함수
    int GetRandomSpawnPoint()
    {
        List<int> availableSpawnPoints = new List<int>();
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (!usedSpawnPoints.Contains(i))
            {
                availableSpawnPoints.Add(i); // 아직 사용되지 않은 spawnPoint 인덱스 추가
            }
        }

        if (availableSpawnPoints.Count > 0)
        {
            // 사용 가능한 spawnPoint 중 하나를 랜덤하게 선택
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            return availableSpawnPoints[randomIndex];
        }
        else
        {
            // 모든 spawnPoints가 사용되었다면, 리스트 초기화하고 다시 시작
            usedSpawnPoints.Clear();
            return GetRandomSpawnPoint();
        }
    }
}
