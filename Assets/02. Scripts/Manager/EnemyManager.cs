using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[System.Serializable]
public struct ClusterInfo
{
    public Transform clusterCenter; // 클러스터 중심 위치
    public float clusterRadius; // 클러스터 반경
    public GameObject monsterPrefab; // 이 클러스터에서 사용할 몬스터 프리팹
}
*/

public class EnemyManager : Singleton<EnemyManager>
{
    //public ClusterInfo[] clusters; // 클러스터 정보 배열
    
    private float currentTime; // 경과 시간 추적
    public GameObject monsterPrefab;

    private bool isFirstSpawn = true; // 첫 소환을 위한 플래그
    // 다음 적 생성까지의 시간
    public float createTime = 1;
    // 적 생성 최소 대기 시간
    public float minTime = 0.5f;
    // 적 생성 최대 대기 시간
    public float maxTime = 1.5f;
    
    // 최대 몬스터 수 (오브젝트 풀 크기)
    private int maxMonsters = 3;
    public int basicMonsters = 3;
    public int monsterOffset = 2;
    
    public List<GameObject> enemyObjectPool = new List<GameObject>(); // 오브젝트 풀 배열 (생성된 적 보관)
    
    public Transform[] spawnPoints; // 적이 생성될 위치 (SpawnPoint들)

    // 사용된 spawnPoints 인덱스 추적을 위한 리스트
    private List<int> usedSpawnPoints  = new List<int>();
    
    
    void Start()
    {
        CreateMonsterPool();
        SpawnMonster(); // 게임 시작 시 바로 몬스터 생성
    }
    
    // 오브젝트 풀 생성
    void CreateMonsterPool()
    { 
        // 몬스터 수 지정값 안에서 랜덤
        int minMonster = basicMonsters - monsterOffset;
        int maxMonster = basicMonsters + monsterOffset;
        maxMonsters = Random.Range(minMonster, maxMonster + 1);
        
        for (int i = 0; i < maxMonsters; i++) // 오브젝트 풀에 넣을 에너미 개수만큼 반복해서
        {
            GameObject enemy = Instantiate(monsterPrefab); // 몬스터 프리팹을 인스턴스화하여 생성 
            enemy.name = $"{monsterPrefab.name}_{i + 1:00}"; // 몬스터 이름 지정 (프리팹 이름_01 ... )
            enemy.SetActive(false); // 초기 상태 : 비활성화
            
            enemyObjectPool.Add(enemy); // 오브젝트 풀에 몬스터 추가
        }
    }
    
    void Update()
    {
        // 시간 업데이트
        currentTime += Time.deltaTime;

        // 처음 : 곧바로 / 그 후 : 지정된 시간이 지나면 적 생성
        if (isFirstSpawn || currentTime > createTime)
        {
            SpawnMonster();
            createTime = Random.Range(minTime, maxTime); // 다음 생성 시간 랜덤 설정
            currentTime = 0; // 시간 초기화
            isFirstSpawn = false; // 첫 소환 후 플래그를 false로 설정
        }
    }

    // 적 생성 함수
    void SpawnMonster()
    {
        GameObject enemy = GetEnemyFromPool();
        if (enemy != null) // 오브젝트 풀에 사용 가능한 에너미가 있다면
        {
            // 사용 가능한 spawnPoint 인덱스 가져오기
            int index = GetRandomSpawnPoint();
            
            // 유효한 인덱스가 있다면
            if (index != -1)
            {
                enemy.transform.position = spawnPoints[index].position; // 에너미 위치 설정
                enemy.SetActive(true); // 에너미 활성화 
                usedSpawnPoints.Add(index); // 사용한 인덱스 기록

                if (QuadtreeManager.Instance != null)
                {
                    QuadtreeManager.Instance.InsertEnemy(enemy.transform.position); 
                }
            }
        }
    }

    // 오브젝트 풀에서 사용 가능한 적을 반환 (적 생성 로직, 관리 로직 분리)
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
