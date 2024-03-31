using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 노드 구조 
public class QNode : MonoBehaviour
{
    private Quadtree quadtree;
    private QNode parent;
    private Bounds bounds;
    private int depth = 0;
    
    public QNode(Quadtree quadtree, QNode parent, Bounds bounds, int depth)
    {
        this.quadtree = quadtree;
        this.parent = parent;
        this.bounds = bounds;
        this.depth = depth;
    }
}
