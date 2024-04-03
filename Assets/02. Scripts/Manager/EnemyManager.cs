using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ClusterInfo
{
    public string clusterName; // 클러스터 이름
    public Transform clusterCenter; // 클러스터 중심 위치
    public float clusterRadius; // 클러스터 반경
    public GameObject monsterPrefab; // 이 클러스터에서 사용할 몬스터 프리팹
    public int basicMonsters; // 기본 몬스터 수 (실제 생성 : 기본 몬스터 수 - 10% ~ 기본 몬스터 수 + 10%)
    
    [HideInInspector] public int actualMonstersCount; // 생성된 몬스터 수 
}


public class EnemyManager : Singleton<EnemyManager>
{
    public ClusterInfo[] clusters; // 클러스터 정보 배열
    
    private float currentTime; // 경과 시간 추적

    private bool isFirstSpawn = true; // 첫 소환을 위한 플래그
    
    [SerializeField] private float createTime; // 다음 적 생성까지의 시간
    [SerializeField] private float minTime = 0.5f; // 적 생성 최소 대기 시간
    [SerializeField] private float maxTime = 1.5f; // 적 생성 최대 대기 시간
    
    List<GameObject> enemyObjectPool = new List<GameObject>(); // 오브젝트 풀 배열 (생성된 적 보관)
    
    void Start()
    {
        CreateMonsterPoolByCluster();
    }
    
    void Update()
    {
        // 시간 업데이트
        currentTime += Time.deltaTime;

        // 처음 : 곧바로 / 그 후 : 지정된 시간이 지나면 적 생성
        if (isFirstSpawn || currentTime > createTime)
        {
            SpawnMonstersByCluster();
            createTime = Random.Range(minTime, maxTime); // 다음 생성 시간 랜덤 설정
            currentTime = 0; // 시간 초기화
            isFirstSpawn = false; // 첫 소환 후 플래그를 false로 설정
        }
    }
    
    void CreateMonsterPoolByCluster()
    {
        foreach (ClusterInfo cluster in clusters)
        {
            // 몬스터 수 지정값 안에서 랜덤
            int minMonsters = Mathf.RoundToInt(cluster.basicMonsters - (cluster.basicMonsters * 0.1f));
            int maxMonsters = Mathf.RoundToInt(cluster.basicMonsters + (cluster.basicMonsters * 0.1f));
            int randomMonsters = Random.Range(minMonsters, maxMonsters + 1);

            cluster.actualMonstersCount = randomMonsters; // 실제 생성된 몬스터 수 저장

            // 클러스터별 부모 오브젝트 생성
            GameObject clusterParent = new GameObject($"Cluster_{cluster.clusterName}");

            for (int i = 0; i < randomMonsters; i++)
            {
                GameObject enemy = Instantiate(cluster.monsterPrefab);
                enemy.SetActive(false); // 초기 상태 : 비활성화
                enemy.name = $"{cluster.monsterPrefab.name}_{i + 1:00}"; // 몬스터 이름 지정 (프리팹 이름_01 ... )
                enemy.transform.SetParent(clusterParent.transform); // 부모 설정
                enemyObjectPool.Add(enemy); // 오브젝트 풀에 몬스터 추가
            }
        }
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

    // 적 생성 함수
    void SpawnMonstersByCluster()
    {
        foreach (ClusterInfo cluster in clusters)
        {
            for (int i = 0; i < cluster.actualMonstersCount; i++)
            {
                GameObject enemy = GetEnemyFromPool();
                if (enemy != null) // 오브젝트 풀에 사용 가능한 에너미가 있다면
                {
                    Vector3 spawnPosition = GenerateSpawnPosition(cluster.clusterCenter.position, cluster.clusterRadius);
                    enemy.transform.position = spawnPosition;
                    enemy.SetActive(true); // 에너미 활성화 
                    
                    if (QuadtreeManager.Instance != null) // 위치 저장 
                    {
                        QuadtreeManager.Instance.InsertEnemy(enemy.transform.position); 
                    }
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

    // 적 인스턴스를 오브젝트 풀로 반환
    public void ReturnEnemyToPool(GameObject enemy)
    {
        if (!enemyObjectPool.Contains(enemy))
        {
            enemy.SetActive(false); // 적 인스턴스를 비활성화
            enemyObjectPool.Add(enemy); // 오브젝트 풀에 적 인스턴스 추가
        }
    }
    
    void OnDrawGizmos()
    {
        if (clusters == null) 
            return;

        foreach (ClusterInfo cluster in clusters)
        {
            // 클러스터 중심 - 작은 구체
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(cluster.clusterCenter.position, 2f);
            
            // 클러스터 반경
            Gizmos.color = Color.blue;
            Vector3 cubeSize = new Vector3(cluster.clusterRadius * 2, 0.5f, cluster.clusterRadius * 2);
            Gizmos.DrawWireCube(cluster.clusterCenter.position, cubeSize);
        }
    }
}
