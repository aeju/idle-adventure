using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DetectTest : MonoBehaviour
{
    public Vector3 size;
    //public Vector3 center;
    public Vector3 centerOffset;
    public LayerMask MonsterLayer;
    
    void Start()
    {
        
        //Collider[] hit = Physics.OverlapBox(transform.position, size, , MonsterLayer);
        
        /*
        for(int i = 0; i < hit.Length; ++i)
        {
            Debug.Log(hit[i].name);
        }
        */
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }

    void Update()
    {
        Collider[] hit = Physics.OverlapBox(transform.position + centerOffset, size, Quaternion.identity, this.MonsterLayer);
        
        if (hit != null)
        {
            Debug.Log(hit[0].name);
        }
        else
        {
            Debug.Log("no");
        }
        
        // 레이 시각화 
        Vector3 rayStart = transform.position + centerOffset; // 레이 시작 위치
        Vector3 rayDirection = Vector3.forward; // 또는 원하는 방향
        Debug.DrawRay(rayStart, rayDirection * 10, Color.blue); // 10은 레이의 길이, 색상은 파란색
    }
}
