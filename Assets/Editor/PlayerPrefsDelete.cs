using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerPrefsDelete
{
    [MenuItem("Tools/Reset PlayerPrefs")]
    private static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs 삭제 완료");
    }
}
