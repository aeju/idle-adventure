using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// 플레이어 프리팹, 스크립트, 저장할 위치 -> 부모 게임 오브젝트
// 지정할 것 : 플레이어 포지션, 스케일
public class PlayerInstantiatorEditor : EditorWindow
{
    private GameObject prefab;
    private string objectName = "NewObject";
    private string savePath = "Assets/03. Prefabs/";
    private Vector3 position = Vector3.zero;
    private Vector3 scale = Vector3.one;

    [MenuItem("Tools/Prefab Instantiator")]
    public static void ShowWindow()
    {
        GetWindow<PlayerInstantiatorEditor>("Player Instantiator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Prefab Instantiator", EditorStyles.boldLabel);

        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
        objectName = EditorGUILayout.TextField("Object Name", objectName);
        savePath = EditorGUILayout.TextField("Save Path", savePath);
        position = EditorGUILayout.Vector3Field("Position", position);
        scale = EditorGUILayout.Vector3Field("Scale", scale);
        
        if (GUILayout.Button("Create Prefab Instance"))
        {
            CreatePrefabInstance();
        }
    }

    private void CreatePrefabInstance()
    {
        if (prefab == null)
        {
            EditorUtility.DisplayDialog("Error", "No prefab selected", "OK");
            return;
        }

        GameObject instance = new GameObject(objectName);
        GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        prefabInstance.transform.localPosition = position;
        prefabInstance.transform.localScale = scale;

        string finalPath = AssetDatabase.GenerateUniqueAssetPath(savePath + "/" + objectName + ".prefab");
        PrefabUtility.SaveAsPrefabAsset(instance, finalPath);

        DestroyImmediate(instance); 
    }
}
