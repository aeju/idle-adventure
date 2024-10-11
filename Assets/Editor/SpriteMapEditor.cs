using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpriteMapEditor : EditorWindow
{
    // 설정값
    private float centerX;
    private float centerY;
    private float width; // x범위
    private float length; // z범위
    private string parentName;
    private string childName;
    private int count;
    private Vector3 childPosition = Vector3.zero;
    private Vector3 childRotation = Vector3.zero;
    private Vector3 childScale = Vector3.one;
    //private Sprite[] sprites; // 스프라이트 배열 
    private Sprite[] sprites = new Sprite[0]; // 스프라이트 배열 초기화 (null 오류)
    
    private Vector2 scrollPosition; // 스크롤 위치
    
    [MenuItem("Tools/3. Sprite Map Editor")]
    public static void ShowWindow()
    {
        GetWindow<SpriteMapEditor>("Sprite Map Editor");
    }

    private void OnGUI()
    {
        GUILayout.Label("스프라이트 맵 생성기", EditorStyles.boldLabel);
        
        centerX = EditorGUILayout.FloatField("Center X", centerX);
        centerY = EditorGUILayout.FloatField("Center Y", centerY);
        width = EditorGUILayout.FloatField("범위 Width", width);
        length = EditorGUILayout.FloatField("범위 Length", length);
        parentName = EditorGUILayout.TextField("부모 오브젝트 이름", parentName);
        childName = EditorGUILayout.TextField("오브젝트 이름", childName);
        childPosition = EditorGUILayout.Vector3Field("Position", childPosition);
        childRotation = EditorGUILayout.Vector3Field("Rotation", childRotation);
        childScale = EditorGUILayout.Vector3Field("Scale", childScale);
        count = EditorGUILayout.IntField("생성할 개수", count);
        
        // 생성 버튼
        if (GUILayout.Button("맵에 배치"))
        {
            SpawnSprites();
        }
        
        // 스프라이트 배열 입력
        EditorGUILayout.LabelField("<스프라이트 목록>");
        
        // 스크롤 뷰 시작
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200)); // 스크롤 높이 설정
        
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i] = (Sprite)EditorGUILayout.ObjectField($"스프라이트 {i + 1}", sprites[i], typeof(Sprite), false);
        }

        if (GUILayout.Button("스프라이트 추가"))
        {
            System.Array.Resize(ref sprites, sprites.Length + 1);
        }
        
        EditorGUILayout.EndScrollView(); // 스크롤 뷰 종료
    }
    
    private void SpawnSprites()
    {
        // 빈 부모 오브젝트 생성
        GameObject parentObject = new GameObject(parentName);
        
        for (int i = 0; i < count; i++)
        {
            // 자식 오브젝트 생성 
            GameObject childObject = new GameObject($"{childName}({i})");
            childObject.transform.SetParent(parentObject.transform);

            // 랜덤 스프라이트 선택
            Sprite selectedSprite = sprites[Random.Range(0, sprites.Length)];

            // 스프라이트 렌더러 추가 및 스프라이트 설정
            SpriteRenderer spriteRenderer = childObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = selectedSprite;
            
            float xPos, yPos, zPos;

            // 랜덤 간격
            xPos = Random.Range(-width, width); // x축 랜덤 범위
            yPos = centerY; // Y축 사용자 입력값
            zPos = Random.Range(-length, length); // z축 랜덤 범위

            childObject.transform.localPosition = new Vector3(xPos, yPos, zPos) + childPosition;
            childObject.transform.localRotation = Quaternion.Euler(childRotation);
            childObject.transform.localScale = childScale;
        }
    }
}
