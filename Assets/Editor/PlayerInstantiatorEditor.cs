using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
<<<<<<< HEAD

// 플레이어 프리팹, 스크립트, 저장할 위치 -> 부모 게임 오브젝트
// 지정할 것 : 플레이어 포지션, 스케일
public class PlayerInstantiatorEditor : EditorWindow
{
    private GameObject prefab;
    private string objectName = "NewObject";
    private string savePath = "Assets/03. Prefabs/";
    private Vector3 position = Vector3.zero;
    private Vector3 scale = Vector3.one;
=======
using UnityEditor.PackageManager.UI;


// 플레이어 프리팹, 스크립트, 저장할 위치 -> 부모 게임 오브젝트
// 플레이어 프리팹 - 스크립트2, 캔버스 프리팹(hp, 쿨타임 슬라이더), 스프라이트 렌더러

// 지정할 것 : 플레이어 포지션, 스케일
// script1 = PlayerStats.cs
// script2 = PlayerController.cs
public class PlayerInstantiatorEditor : EditorWindow
{
    // 설정할 필요 o
    private string objectName = "PlayerName";
    private GameObject PlayerPrefab;
    private GameObject CanvasPrefab;
    private string savePath = "Assets/03. Prefabs/";
    
    // 설정할 필요 x
    private MonoScript script1;
    private MonoScript script2;
    private Vector3 position = new Vector3(0, 0.75f, 0);
    private Vector3 scale = new Vector3(2, 2, -1);
>>>>>>> main

    [MenuItem("Tools/Prefab Instantiator")]
    public static void ShowWindow()
    {
<<<<<<< HEAD
        GetWindow<PlayerInstantiatorEditor>("Player Instantiator");
=======
        var window = GetWindow<PlayerInstantiatorEditor>("Player Instantiator");
        window.AutoScripts();
>>>>>>> main
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Instantiator", EditorStyles.boldLabel);

<<<<<<< HEAD
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
        objectName = EditorGUILayout.TextField("Object Name", objectName);
        savePath = EditorGUILayout.TextField("Save Path", savePath);
=======
        objectName = EditorGUILayout.TextField("Object Name", objectName);
        PlayerPrefab = (GameObject)EditorGUILayout.ObjectField("Player Prefab", PlayerPrefab, typeof(GameObject), false);
        CanvasPrefab = (GameObject)EditorGUILayout.ObjectField("Canvas Prefab", CanvasPrefab, typeof(GameObject), false);
        savePath = EditorGUILayout.TextField("Save Path", savePath);
        
        script1 = (MonoScript)EditorGUILayout.ObjectField("Player Stats Script", script1, typeof(MonoScript), false);
        script2 = (MonoScript)EditorGUILayout.ObjectField("Player Controller Script", script2, typeof(MonoScript), false);
>>>>>>> main
        position = EditorGUILayout.Vector3Field("Position", position);
        scale = EditorGUILayout.Vector3Field("Scale", scale);
        
        if (GUILayout.Button("Create Prefab Instance"))
        {
            CreatePrefabInstance();
        }
    }

<<<<<<< HEAD
    private void CreatePrefabInstance()
    {
        if (prefab == null)
        {
            EditorUtility.DisplayDialog("Error", "No prefab selected", "OK");
=======
    private void AutoScripts()
    {
        script1 = FindScriptByName("PlayerStats");
        script2 = FindScriptByName("PlayerController");
    }

    private MonoScript FindScriptByName(string scriptName)
    {
        foreach (var script in Resources.FindObjectsOfTypeAll<MonoScript>())
        {
            if (script.name == scriptName)
                return script;
        }

        return null;
    }


    private void CreatePrefabInstance()
    {
        if (PlayerPrefab == null || CanvasPrefab == null)
        {
            EditorUtility.DisplayDialog("Error", "빈 프리팹 있음!", "OK");
>>>>>>> main
            return;
        }

        GameObject instance = new GameObject(objectName);
<<<<<<< HEAD
        GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        prefabInstance.transform.localPosition = position;
        prefabInstance.transform.localScale = scale;
=======
        GameObject prefabInstance = PrefabUtility.InstantiatePrefab(PlayerPrefab) as GameObject;
        prefabInstance.transform.SetParent(instance.transform);
        prefabInstance.transform.localPosition = position;
        prefabInstance.transform.localScale = scale;
        
        GameObject canvasInstance = PrefabUtility.InstantiatePrefab(CanvasPrefab) as GameObject;
        canvasInstance.transform.SetParent(prefabInstance.transform, false); // 캔버스 프리팹(Slider) - 플레이어 프리팹 자식으로 설정
        prefabInstance.AddComponent<SpriteRenderer>(); // 스프라이트 렌더러 컴포넌트 추가 
        
        if (script1 != null)
        {
            prefabInstance.AddComponent(script1.GetClass());
        }

        if (script2 != null)
        {
            prefabInstance.AddComponent(script2.GetClass());
        }
>>>>>>> main

        string finalPath = AssetDatabase.GenerateUniqueAssetPath(savePath + "/" + objectName + ".prefab");
        PrefabUtility.SaveAsPrefabAsset(instance, finalPath);

        DestroyImmediate(instance); 
    }
}
