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
    
    public Quadtree(Rectangle _boundary, int _capacity)
    {
        boundary = _boundary;
        capacity = _capacity;
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
        var x = boundary.centerX;
        var z = boundary.centerZ;
        var w = boundary.width;
        var h = boundary.height;
        
        Debug.Log($"Subdividing Quadtree. Center: ({x}, {z}), Width: {w}, Height: {h}");
        
        var ne = new Rectangle(x + w / 2, z + h / 2, w / 2, h / 2);
        northEast = new Quadtree(ne, capacity);
        
        var nw = new Rectangle(x - w / 2, z + h / 2, w / 2, h / 2);
        northWest = new Quadtree(nw, capacity);
        
        var se = new Rectangle(x + w / 2, z - h / 2, w / 2, h / 2);
        southEast = new Quadtree(se, capacity);
        
        var sw = new Rectangle(x - w / 2, z - h / 2, w / 2, h / 2);
        southWest = new Quadtree(sw, capacity);

        divided = true;
    }

    // 구조 시각화
    public void Show(Texture2D tex)
    {
        // 경계선 그리기
        for (float x = 0; x < boundary.width * 2; x++)
        {
            
            tex.SetPixel((int)(boundary.centerX + x - boundary.width), (int)(boundary.centerZ - boundary.height - 1), Color.red);
            tex.SetPixel((int)(boundary.centerX + x - boundary.width), (int)(boundary.centerZ + boundary.height), Color.green);
        }
        for (float z = 0; z < boundary.height * 2; z++)
        {
            tex.SetPixel((int)(boundary.centerX - boundary.width - 1), (int)(boundary.centerZ + z - boundary.height), Color.blue);
            tex.SetPixel((int)(boundary.centerX + boundary.width), (int)(boundary.centerZ + z - boundary.height), Color.gray);
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
