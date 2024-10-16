using System.Linq;
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

    // 쿼드트리에서 포인트 찾아 삭제하는 로직
    public bool Remove(string monsterName)
    {
        // 현재 노드에서 식별자에 해당하는 포인트를 찾음
        Point pointToRemove = points.FirstOrDefault(p => p.monsterName == monsterName);
        if (pointToRemove != null)
        {
            points.Remove(pointToRemove);
            Debug.Log($"Removed point {pointToRemove.x}, {pointToRemove.z} with ID {monsterName}.");
            return true;
        }

        // 현재 노드에서 찾지 못했다면, 분할된 노드들을 검사
        if (divided)
        {
            return northEast.Remove(monsterName) || northWest.Remove(monsterName) ||
                   southEast.Remove(monsterName) || southWest.Remove(monsterName);
        }

        // 포인트를 찾지 못한 경우
        Debug.Log($"Failed to find point with ID {monsterName}.");
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
        northEast = new Quadtree(ne, capacity, maxDepth, currentDepth + 1);
        
        var nw = new Rectangle(x - w / 2, z + l / 2, w / 2, l / 2);
        northWest = new Quadtree(nw, capacity, maxDepth, currentDepth + 1);
        
        var se = new Rectangle(x + w / 2, z - l / 2, w / 2, l / 2);
        southEast = new Quadtree(se, capacity, maxDepth, currentDepth + 1);
        
        var sw = new Rectangle(x - w / 2, z - l / 2, w / 2, l / 2);
        southWest = new Quadtree(sw, capacity, maxDepth, currentDepth + 1);

        divided = true;
    }
    
    // 영역 조회 : 특정 영역(range)에 포함된 점들을 찾아 foundPoints 리스트에 추가
    public void Query(Rectangle range, List<Point> foundPoints)
    {
        // 현재 노드의 경계가 조회 범위와 겹치지 않으면, 더 이상 검사할 필요 x
        if (!boundary.Intersects(range))
        {
            Debug.Log("[Query] 0. Query range does not intersect with the boundary.");
            return;
        }
        else
        {
            Debug.Log("[Query] 1. Querying..."); // 조회 시작 로그
            int foundCountBefore = foundPoints.Count; // 조회 전 찾은 점의 개수
            
            // 현재 노드에 저장된 모든 점들을 순회
            foreach (var point in points)
            {
                // 점이 조회 범위 내에 있는지 확인
                if (range.Contains(point))
                {
                    foundPoints.Add(point);
                    Debug.Log($"[Query] 2. Found point at ({point.x}, {point.z}) within the query range.");
                }
            }
            // 만약 현재 노드가 분할되었다면, 각 하위 노드에 대해서도 같은 조회를 재귀적으로 수행
            if (divided)
            {
                northEast.Query(range, foundPoints);
                northWest.Query(range, foundPoints);
                southEast.Query(range, foundPoints);
                southWest.Query(range, foundPoints);
            }
            
            int foundCountAfter = foundPoints.Count; // 조회 후 찾은 점의 개수
            Debug.Log($"[Query] 3. Query completed. Total points found in range: {foundCountAfter - foundCountBefore}");
        }
    }
    
    public bool Exists(string monsterName)
    {
        if (points.Any(p => p.monsterName == monsterName))
        {
            return true;
        }
        
        if (divided)
        {
            return northEast.Exists(monsterName) ||
                   northWest.Exists(monsterName) ||
                   southEast.Exists(monsterName) ||
                   southWest.Exists(monsterName);
        }

        return false;
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
