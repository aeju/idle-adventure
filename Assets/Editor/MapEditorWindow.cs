using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEditorWindow : EditorWindow
{
    [MenuItem("Tools/Map Editor")]
    public static void ShowWindow()
    {
        GetWindow<MapEditorWindow>("Map Editor");
    }

    void OnGUI()
    {
        GUILayout.Label("Map Editor Settings", EditorStyles.boldLabel);

        if (GUILayout.Button("Update Grid"))
        {
            MapEditor mapEditor = FindObjectOfType<MapEditor>();
            if (mapEditor != null)
            {
                mapEditor.InitializeGrid();
            }
            else
            {
                Debug.LogError("MapEditor component not found in the scene!");
            }
        }

        // 추가 설정 필요 
    }
}
