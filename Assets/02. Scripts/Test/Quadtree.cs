using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QNodeIndex : int
{
    UPPERLEFT = 0,
    UPPERRIGHT,
    LOWERRIGHT,
    LOWERLEFT,
    STRADDLING,
    OUTOFAREA // 영역 밖을 벗어난 경우
}

// 쿼드트리의 루트 노드, 기본적인 메소드
public class Quadtree : MonoBehaviour
{
    public QNode rootNode;
    
    public Quadtree(Vector2 size)
    {
        rootNode = new QNode(this, null, new Bounds(Vector2.zero, size), 0);
    }
}
