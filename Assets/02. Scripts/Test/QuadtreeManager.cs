using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 쿼드트리 관리
public class QuadtreeManager : Singleton<QuadtreeManager>
{
    Texture2D tex;
    public Renderer targetRenderer;
    Quadtree quadTree;

    public int capacity = 1;
    public int totalArea = 100;

    Color32[] resetColorArray;
    Color32 resetColor = new Color32(0, 0, 0, 255);

    void Start()
    {
        int resolution = 256;
        
        tex = new Texture2D(resolution, resolution, TextureFormat.RGB24, false);
        tex.filterMode = FilterMode.Point;

        // texture 초기화
        resetColorArray = tex.GetPixels32();
        ClearTexture(tex);
        tex.Apply(false);

        int size = resolution / 2;

        Rectangle boundary = new Rectangle(size, size, size, size);

        quadTree = new Quadtree(boundary, capacity);

        // 시각화
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
    
    /*
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
    }
    */
    
    /*
    // 몬스터가 생성될 때 호출 (위치를 quadtree에 삽입)
    public void InsertMonster(GameObject monster)
    {
        quadtree.Insert(monster);
    }
    
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        
        // 지형 위에 눕혀진 박스 DrawWireCube(중심점, 너비/높이/깊이)
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(totalArea.x, 0.1f, totalArea.y));
    }
    */
}
