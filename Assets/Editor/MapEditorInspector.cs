using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapEditor))]
public class MapEditorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        MapEditor mapEditor = (MapEditor)target;

        // Add your custom inspector GUI code here
        // For example, input fields for grid dimensions, monster counts, etc.

        if (GUILayout.Button("Set Spawn Point"))
        {
            // Logic to set a spawn point
            // You might need to figure out a way to determine x and y
        }
        
        if (GUILayout.Button("Clear Spawn Points"))
        {
            // Clear all spawn points
        }
        
        if (GUILayout.Button("Update Grid"))
        {
            mapEditor.InitializeGrid();
        }

        // Add other controls as needed
    }
}
