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
        quadTree = new Quadtree(boundary, capacity);
        
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        // Gizmo로 지형 위에 눕혀진 박스, DrawWireCube(중심점, 너비/높이/깊이)
        Gizmos.DrawWireCube(new Vector3(boundaryCenterX, 0, boundaryCenterZ), new Vector3(boundaryWidth, 0.1f, boundaryLength));
    }
    
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
    
    /*
        // 쿼드 트리가 없다면, 새로 만듦
        if (quadtree == null)
        {
            quadtree = new Quadtree(totalArea);
        }

    */
}
