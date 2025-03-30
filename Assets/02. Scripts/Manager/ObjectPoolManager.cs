using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    private Dictionary<string, Queue<GameObject>> objectPools = new Dictionary<string, Queue<GameObject>>();

    // 오브젝트 - 오브젝트 풀에서 가져오거나 새로 생성
    public GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        // 1. 해당 프리팹의 풀이 없다면 : 새로 생성
        if (!objectPools.ContainsKey(prefab.name))
        {
            objectPools[prefab.name] = new Queue<GameObject>();
        }

        // 2-1. 풀에 오브젝트가 있으면 : 꺼내서 사용
        GameObject obj;
        if (objectPools[prefab.name].Count > 0)
        {
            obj = objectPools[prefab.name].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
        }
        else
        {
            // 2-2. 풀에 오브젝트가 없으면 새로 생성
            obj = Instantiate(prefab, position, rotation);
            obj.name = prefab.name;
        }

        obj.SetActive(true);
        return obj;
    }

    // 오브젝트 - 오브젝트 풀에 반환 
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        if (!objectPools.ContainsKey(obj.name))
        {
            objectPools[obj.name] = new Queue<GameObject>();
        }
        objectPools[obj.name].Enqueue(obj);
    }
}
