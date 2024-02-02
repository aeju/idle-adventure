using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerPrefsDelete : EditorWindow
{
    [MenuItem("Tools/Reset PlayerPrefs")]
    private static void ShowWindow()
    {
        GetWindow<PlayerPrefsDelete>("Reset PlayerPrefs");
    }
    
    private void OnGUI()
    {
        if (GUILayout.Button("Reset PlayerPrefs"))
        {
            if (EditorUtility.DisplayDialog("Reset PlayerPrefs",
                    "모든 PlayerPrefs를 삭제?", "Yes", "No"))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
                Debug.Log("PlayerPrefs deletion complete");
                EditorUtility.DisplayDialog("PlayerPrefs", "삭제 완료", "OK");
            }
        }
    }
}
