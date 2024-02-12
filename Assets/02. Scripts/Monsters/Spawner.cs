using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public float spawnTime;
    public int spriteType;
    public int health;
    public float speed;
}

// 적을 주기적으로 생성
public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint; // 적이 생성될 위치
    public SpawnData[] spawnData; // 스폰 데이터(각 레벨에 따른 - 스폰 시간, 적의 스탯 등)

    private float timer;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.2f) // 타이머가 일정 시간 값에 도달하면 소환
        {
            timer = 0f; // 시간 초기화
            Spawn();
        }
    }

    void Spawn()
    {
        //GameManager.instance.pool.Get(1);
        GameObject enemy = GameManager.Instance.pool.Get(Random.Range(0, 2));
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position; // 자식 오브젝트에서만 선택되도록 랜덤 시작은 1부터
    }
}
