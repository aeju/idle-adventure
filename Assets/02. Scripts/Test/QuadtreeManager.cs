using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 쿼드트리 관리
public class QuadtreeManager : Singleton<QuadtreeManager>
{
    private Quadtree quadTree = null;

    public int capacity = 0;

    // Quadtree 경계 설정 속성
    public float boundaryCenterX = 0f;
    public float boundaryCenterZ = 0f;
    public float boundaryWidth = 100f;
    public float boundaryLength = 100f;

    public int maxDepth = 4; // 최대 깊이 설정
    
    // 마지막으로 조회된 queryArea를 저장하는 필드
    private Rectangle lastQueryArea;
    private bool hasQueried = false; // queryArea가 설정되었는지 확인하기 위한 플래그
    
    void Start()
    {
        Rectangle boundary = new Rectangle(boundaryCenterX, boundaryCenterZ, boundaryWidth, boundaryLength);

        // 쿼드 트리가 없다면, 새로 만듦
        if (quadTree == null)
        {
            quadTree = new Quadtree(boundary, capacity, maxDepth);
        }
    }
    
    // 몬스터 위치 정보 : Point 객체로 변환하여 쿼드트리에 삽입
    public void InsertEnemy(Vector3 position, string monsterName)
    {
        Debug.Log($"Trying to insert enemy at: {position}"); // 적 생성 위치 로그
        // Vector3 위치를 Point로 변환
        Point point = new Point(position.x, position.z, monsterName); 
        
        // 쿼드트리에 포인트 삽입
        if (!quadTree.Insert(point))
        {
            Debug.LogError($"Point {point.x}, {point.z} is outside the Quadtree boundary.");
        }
    }

    // 몬스터 위치 삭제 : 쿼드트리에서 제거 
    public bool RemoveEnemy(string monsterName)
    {
        Debug.Log($"Trying to remove enemy name: {monsterName}");
        bool removed = quadTree.Remove(monsterName);
        
        // 쿼드트리에서 포인트 삭제 시도
        if (!removed)
        {
            Debug.LogError($"Failed to remove enemy with ID: {monsterName}");
        }
        return removed;
    }
    
    public List<Point> QueryEnemy(Rectangle range)
    {
        List<Point> foundPoints = new List<Point>();
        quadTree.Query(range, foundPoints);
        return foundPoints;
    }
    
    public List<Point> QueryNearbyEnemies(Vector3 playerPosition, float radius)
    {
        // 플레이어 위치를 중심으로 하는 사각형 영역 생성
        Rectangle queryArea = new Rectangle(playerPosition.x, playerPosition.z, radius, radius);
        List<Point> foundPoints = new List<Point>();
        quadTree.Query(queryArea, foundPoints);
        return foundPoints;
    }
    
    // 몬스터 위치 갱신
    public void UpdateMonsterPosition(string monsterName, Vector3 newPosition) 
    {
        // quadTree에 monster이름에 해당하는 값이 존재한다면, 지우기
        if (quadTree.Exists(monsterName))
        {
            // 오래된 값을 삭제하고
            if (RemoveEnemy(monsterName))
            {
                // 새로운 위치값으로 갱신
                InsertEnemy(newPosition, monsterName);
            }
        }
        else // 없다면 
        {
            InsertEnemy(newPosition, monsterName);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        // Gizmo로 지형 위에 눕혀진 박스, DrawWireCube(중심점, 너비/높이/깊이)
        Gizmos.DrawWireCube(new Vector3(boundaryCenterX, 0, boundaryCenterZ), new Vector3(boundaryWidth, 0.1f, boundaryLength));
    }
}
