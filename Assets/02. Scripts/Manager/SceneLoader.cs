using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneLoader : MonoBehaviour, IPointerDownHandler
{
    public string loadingScene = "LoadingScene";
    
    public void OnPointerDown(PointerEventData eventData)
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(loadingScene); 
    }
}
