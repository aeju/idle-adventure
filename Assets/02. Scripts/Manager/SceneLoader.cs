using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneLoader : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]private string loadingScene = "LoadingScene";
    
    public void OnPointerDown(PointerEventData eventData)
    {
        LoadNextScene();
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(loadingScene); 
    }

}
