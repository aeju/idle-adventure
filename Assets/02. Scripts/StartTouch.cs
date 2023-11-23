using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartTouch : MonoBehaviour
{
    void Update()
    {
        // 터치나 마우스 클릭을 감지
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            LoadNextScene();
        }
        
        // 테스트 목적으로 마우스 클릭도 인식
        if (Input.GetMouseButtonDown(0))
        {
            LoadNextScene();
        }
        
        void LoadNextScene()
        {
            // 메인 씬 불러오기
            SceneManager.LoadScene("MainScene");
        }
    }
}