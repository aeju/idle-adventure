using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingNextScene : MonoBehaviour
{
    //public int sceneNumber = 3;
    public Slider loadingBar;
    public TextMeshProUGUI loadingText;

    [SerializeField]private string MainScene = "DevelopScene";
    
    void Start()
    {
        StartCoroutine(TransitionNextScene());
    }
    
    // 비동기 씬 로드 코루틴
    IEnumerator TransitionNextScene()
    {
        // 지정된 씬을 비동기 형식으로 로드
        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(num);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(MainScene);
        
        // 로드되는 씬의 모습이 화면에 보이지 않게
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            // 로딩 진행률을 슬라이더 바, 텍스트로 표시
            loadingBar.value = asyncOperation.progress;
            loadingText.text = (asyncOperation.progress * 100f).ToString() + "%";
        }
        
        // 씬 로드 진행률 90% 넘어가면
        if (asyncOperation.progress >= 0.9f)
        {
            // 로드된 씬 화면에 보이게
            asyncOperation.allowSceneActivation = true;
        }
        
        // 다음 프레임이 될 때까지 기다리기
        yield return null;
    }
}
