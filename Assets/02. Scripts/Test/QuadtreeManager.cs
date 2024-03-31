using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QDrawMode : int
{
    INSERT,
    QUERY
}

// 쿼드트리 관리
public class QuadtreeManager : MonoBehaviour
{
    public QDrawMode drawMode = QDrawMode.INSERT;
    
    public Vector2 totalArea = new Vector3(64f, 64f);

    public Quadtree quadtree = null;

    private void Awake()
    {
        
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        
        // Gizmo로 지형 위에 눕혀진 박스
        // DrawWireCube(중심점, 너비/높이/깊이)
        Gizmos.DrawWireCube(new Vector3(0, 0, 0), new Vector3(totalArea.x, 0.1f, totalArea.y));
    }
}
