using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 쿼드트리의 루트 노드, 기본적인 메소드
public class Quadtree : MonoBehaviour
{
    private Rectangle boundary; // 경계 정의
    private int capacity; // 최대 용량
    private List<Point> points; // 포함된 점들의 목록
    private bool divided; // 분할 여부

    // 하위 노드들
    private Quadtree northEast;
    private Quadtree northWest;
    private Quadtree southEast;
    private Quadtree southWest;
    
    // 노드 분할 깊이 제한
    public int maxDepth;
    private int currentDepth; // 현재 노드의 깊이 
    
    public Quadtree(Rectangle _boundary, int _capacity, int _maxDepth, int _currentDepth = 0)
    {
        boundary = _boundary;
        capacity = _capacity;
        maxDepth = _maxDepth;
        currentDepth = _currentDepth;
        points = new List<Point>();
        divided = false;
    }

    // 점 삽입
    //public bool Insert(GameObject objPoint)
    public bool Insert(Point point)
    {
        if (boundary.Contains(point) == false)
        {
            Debug.Log($"Point {point.x}, {point.z} is outside the Quadtree boundary.");
            return false;
        }

        if (points.Count < capacity)
        {
            points.Add(point);
            Debug.Log($"Inserted point {point.x}, {point.z} successfully. Total points: {points.Count}");
            return true;
        }
        else // divide 필요
        {
            if (divided == false)
            {
                Subdivide();
                Debug.Log("Quadtree subdivided due to capacity overflow.");
            }

            if (northEast.Insert(point) == true)
            {
                return true;
            }
            else if (northWest.Insert(point) == true)
            {
                return true;
            }
            else if (southEast.Insert(point) == true)
            {
                return true;
            }
            else if (southWest.Insert(point) == true)
            {
                return true;
            }
            
            Debug.LogError($"Failed to insert the point {point.x}, {point.z} into any sub-Quadtree.");
        }
        
        // error
        return false;
    }

    // 노드 사분
    void Subdivide()
    {
        if(currentDepth >= maxDepth) // 깊이 제한 확인
        {
            Debug.Log($"최대 깊이 도달: currentDepth = {currentDepth}. No further subdivision.");
            return; // 최대 깊이에 도달했으면 더 이상 분할하지 않음
        }
        
        var x = boundary.centerX;
        var z = boundary.centerZ;
        var w = boundary.width;
        var l = boundary.length;
        
        Debug.Log($"Subdividing Quadtree. Center: ({x}, {z}), Width: {w}, Height: {l}");
        
        
        var ne = new Rectangle(x + w / 2, z + l / 2, w / 2, l / 2);
        
        // 하위 노드 생성 시 깊이를 하나 증가시키고 최대 깊이를 전달
        //northEast = new Quadtree(ne, capacity);
        northEast = new Quadtree(ne, capacity, maxDepth, currentDepth + 1);
        
        var nw = new Rectangle(x - w / 2, z + l / 2, w / 2, l / 2);
        //northWest = new Quadtree(nw, capacity);
        northWest = new Quadtree(nw, capacity, maxDepth, currentDepth + 1);
        
        var se = new Rectangle(x + w / 2, z - l / 2, w / 2, l / 2);
        //southEast = new Quadtree(se, capacity);
        southEast = new Quadtree(se, capacity, maxDepth, currentDepth + 1);
        
        var sw = new Rectangle(x - w / 2, z - l / 2, w / 2, l / 2);
        //southWest = new Quadtree(sw, capacity);
        southWest = new Quadtree(sw, capacity, maxDepth, currentDepth + 1);

        divided = true;
    }

    // 구조 시각화
    public void Show(Texture2D tex)
    {
        // 경계선 그리기
        for (float x = 0; x < boundary.width * 2; x++)
        {
            
            tex.SetPixel((int)(boundary.centerX + x - boundary.width), (int)(boundary.centerZ - boundary.length - 1), Color.red);
            tex.SetPixel((int)(boundary.centerX + x - boundary.width), (int)(boundary.centerZ + boundary.length), Color.green);
        }
        for (float z = 0; z < boundary.length * 2; z++)
        {
            tex.SetPixel((int)(boundary.centerX - boundary.width - 1), (int)(boundary.centerZ + z - boundary.length), Color.blue);
            tex.SetPixel((int)(boundary.centerX + boundary.width), (int)(boundary.centerZ + z - boundary.length), Color.gray);
        }

        // 재귀적으로 자식 보이기
        if (divided == true)
        {
            northEast.Show(tex);
            northWest.Show(tex);
            southEast.Show(tex);
            southWest.Show(tex);
        }
        
        // 실제 포인트 그리기
        for (int i = 0, length = points.Count; i < length; i++)
        {
            tex.SetPixel((int)points[i].x, (int)points[i].z, Color.white);
        }
    }
}
