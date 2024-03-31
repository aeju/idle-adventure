using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QDrawMode : int
{
    INSERT,
    QUERY
}

// 쿼드트리 관리
public class QuadtreeManager : Singleton<QuadtreeManager>
{
    public Vector2 totalArea = new Vector3(100f, 100f);

    public Quadtree quadtree = null;

    private void Awake()
    {
        /*
        // 쿼드 트리가 없다면, 새로 만듦
        if (quadtree == null)
        {
            quadtree = new Quadtree(totalArea);
        }
        */
    }
    
    /*
    // 몬스터가 생성될 때 호출 (위치를 quadtree에 삽입)
    public void InsertMonster(GameObject monster)
    {
        quadtree.Insert(monster);
    }
    */
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        
        // 지형 위에 눕혀진 박스 DrawWireCube(중심점, 너비/높이/깊이)
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(totalArea.x, 0.1f, totalArea.y));
    }
}
