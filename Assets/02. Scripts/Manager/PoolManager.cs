using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 오브젝트 풀링 관리
public class PoolManager : MonoBehaviour
{
    // 프리팹 보관할 변수 (풀링할 프리팹들)
    public GameObject[] prefabs;
    // 풀 담당 리스트 (변수 개수와 1:1 관계), 현재 활성화되지 않은 게임 오브젝트들의 리스트
    private List<GameObject>[] pools;

    // 1대1 관계
    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    // 요청된 인덱스에 해당하는 풀에서 비활성화된 게임 오브젝트를 찾아 반환 (없으면 새로 생성)
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
}
