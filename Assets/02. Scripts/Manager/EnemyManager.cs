using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
// (25 +- 5 마리 * 4 클러스터)
public struct ClusterInfo
{
    public Transform clusterCenter; // 클러스터 중심 위치
    public float clusterRadius; // 클러스터 반경
    public GameObject monsterPrefab; // 클러스터에서 사용할 몬스터 프리팹
    public int maxMonster; // 클러스트 당 최대 몬스터 수 
    public int monsterOffset;
}

public class EnemyManager : Singleton<EnemyManager>
{
    public ClusterInfo[] clusters; // 클러스터 정보 배열
    
    // 오브젝트 풀 배열 (생성된 적 보관)
    public List<GameObject> enemyObjectPool;
    
    private float currentTime; // 경과 시간 추적
    
    // 다음 적 생성까지의 시간
    public float createTime = 1;
    
    // 적 생성 최소 대기 시간
    public float minTime = 0.5f;

    // 적 생성 최대 대기 시간
    public float maxTime = 1.5f;
    
    // 오브젝트 풀 크기
    private int maxMonsters;
    
    
    void Start()
    {
        CalculateMaxMonsters(); // 오브젝트 풀 크기 계산
        CreateMonsterPool(); // 오브젝트 풀 생성
    }

    void Update()
    {
        // 시간 업데이트
        currentTime += Time.deltaTime;

        // 지정된 시간이 지나면 적 생성
        if (currentTime > createTime)
        {
            SpawnMonstersByCluster();
            createTime = Random.Range(minTime, maxTime); // 다음 생성 시간 랜덤 설정
            currentTime = 0; // 시간 초기화
        }
    }
    
    // 오브젝트 풀 크기 계산
    void CalculateMaxMonsters()
    {
        maxMonsters = 0;
        foreach (var cluster in clusters)
        {
            maxMonsters += (cluster.maxMonster + cluster.monsterOffset); // 기본값 + off최댓값으로 풀 생성
        }
    }
    
    // 오브젝트 풀 생성
    void CreateMonsterPool()
    {
        enemyObjectPool = new List<GameObject>();

        foreach (var cluster in clusters)
        {
            for (int i = 0; i < maxMonsters; i++)
            {
                GameObject enemy = Instantiate(cluster.monsterPrefab);
                enemy.name = $"Monster_{i + 1:00}"; // 몬스터 이름 지정 (Monster_01 ... )
                enemy.SetActive(false); // 초기 상태는 비활성화
                enemyObjectPool.Add(enemy); // 오브젝트 풀에 몬스터 추가
            }
        }
    }

    void SpawnMonstersByCluster()
    {
        foreach (var cluster in clusters)
        {
            int monstersToSpawn = Random.Range(cluster.maxMonster - cluster.monsterOffset, cluster.maxMonster + cluster.monsterOffset + 1);
            for (int i = 0; i < monstersToSpawn; i++)
            {
                GameObject enemy = GetEnemyFromPool();
                if (enemy != null) // 오브젝트 풀에 사용 가능한 에너미가 있다면
                {
                    Vector3 spawnPosition = GenerateSpawnPosition(cluster.clusterCenter.position, cluster.clusterRadius);
                    enemy.transform.position = spawnPosition;
                    enemy.SetActive(true);
                }
            }
        }
    }
    
    // 오브젝트 풀에서 사용 가능한 몬스터를 반환하는 함수, 프리팹을 매개변수로 받도록 
    GameObject GetEnemyFromPool()
    {
        foreach (GameObject enemy in enemyObjectPool)
        {
            if (!enemy.activeSelf) // 비활성화 상태인 적을 찾음
            {
                return enemy;
            }
        }
        return null; // 사용 가능한 적이 없으면 null 반환
    }

    // 클러스터 반경 내에서 중복되지 않는 랜덤 위치를 생성
    Vector3 GenerateSpawnPosition(Vector3 center, float radius)
    {
        Vector3 spawnPosition;
        do
        {
            float randomAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
            float randomRadius = Random.Range(0, radius);
            spawnPosition = new Vector3(center.x + Mathf.Cos(randomAngle) * randomRadius, 0, center.z + Mathf.Sin(randomAngle) * randomRadius);
        }
        while (IsPositionOccupied(spawnPosition));

        return spawnPosition;
    }

    // 주어진 위치가 이미 사용되었는지 확인
    bool IsPositionOccupied(Vector3 position)
    {
        foreach (var enemy in enemyObjectPool)
        {
            if (enemy.activeSelf && Vector3.Distance(enemy.transform.position, position) < 1.0f) // 간단한 충돌 검사
            {
                return true;
            }
        }
        return false;
    }
    
    // 적 인스턴스를 오브젝트 풀로 반환
    public void ReturnEnemyToPool(GameObject enemy)
    {
        if (!enemyObjectPool.Contains(enemy))
        {
            enemy.SetActive(false); // 적 인스턴스를 비활성화
            enemyObjectPool.Add(enemy); // 오브젝트 풀에 적 인스턴스 추가
        }
    }
}
