using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 쿼드트리 관리
public class QuadtreeManager : Singleton<QuadtreeManager>
{
    Texture2D tex;
    public Renderer targetRenderer;
    private Quadtree quadTree = null;

    public int capacity = 0;

    // Quadtree 경계 설정 속성
    public float boundaryCenterX = 0f;
    public float boundaryCenterZ = 0f;
    public float boundaryWidth = 100f;
    public float boundaryLength = 100f;
    
    Color32[] resetColorArray;
    Color32 resetColor = new Color32(0, 0, 0, 255);

    public int maxDepth = 4; // 최대 깊이 설정
    
    // 마지막으로 조회된 queryArea를 저장하는 필드
    private Rectangle lastQueryArea;
    private bool hasQueried = false; // queryArea가 설정되었는지 확인하기 위한 플래그
    
    void Start()
    {
        tex = new Texture2D((int)boundaryWidth, (int)boundaryLength, TextureFormat.RGB24, false);
        tex.filterMode = FilterMode.Point;

        // texture 초기화
        resetColorArray = tex.GetPixels32();
        ClearTexture(tex);
        tex.Apply(false);
        
        // Rectangle 객체 = 쿼드트리 경계
        Rectangle boundary = new Rectangle(boundaryCenterX, boundaryCenterZ, boundaryWidth, boundaryLength);

        // 쿼드 트리가 없다면, 새로 만듦
        if (quadTree == null)
        {
            quadTree = new Quadtree(boundary, capacity, maxDepth);
        }
        
        // 시각화를 위해 텍스처를 targetRenderer에 할당
        targetRenderer.material.mainTexture = tex;
    }

    void Update()
    {
        // 마우스 클릭 위치에 점 추가
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var x = hit.textureCoord.x * tex.width;
                var y = hit.textureCoord.y * tex.height;

                var p = new Point(x, y);
                quadTree.Insert(p);

                quadTree.Show(tex);
                tex.Apply(false);
            }
        }
    }


    void ClearTexture(Texture2D tex)
    {
        for (int i = 0, len = resetColorArray.Length; i < len; i++)
        {
            resetColorArray[i] = resetColor;
        }
        tex.SetPixels32(resetColorArray);
    }
    
    // 몬스터 위치 정보 : Point 객체로 변환하여 쿼드트리에 삽입
    public void InsertEnemy(Vector3 position)
    {
        Debug.Log($"Trying to insert enemy at: {position}"); // 적 생성 위치 로그
        // Vector3 위치를 Point로 변환
        Point point = new Point(position.x, position.z); 
        
        // 쿼드트리에 포인트 삽입
        if (!quadTree.Insert(point))
        {
            Debug.LogError($"Point {point.x}, {point.z} is outside the Quadtree boundary.");
        }
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
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        // Gizmo로 지형 위에 눕혀진 박스, DrawWireCube(중심점, 너비/높이/깊이)
        Gizmos.DrawWireCube(new Vector3(boundaryCenterX, 0, boundaryCenterZ), new Vector3(boundaryWidth, 0.1f, boundaryLength));
    }
}
