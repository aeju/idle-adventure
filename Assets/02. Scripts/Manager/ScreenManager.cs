using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    public float idleTime = 60.0f; // 1분 동안의 방치 시간
    private float currentTime;
    
    public Canvas blackScreenCanvas; // 검은 화면을 표시하기 위한 Canvas

    private void Start()
    {
        blackScreenCanvas.enabled = false; 
        currentTime = 0; // 타이머 초기화
    }

    private void Update()
    {
        // 사용자의 입력이 감지되면 타이머를 초기화
        if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            currentTime = 0;
            blackScreenCanvas.enabled = false; 
        }
        else
        {
            currentTime += Time.deltaTime; // 입력이 없을 경우 타이머 증가

            // 지정된 방치 시간이 초과되면 검은 화면을 활성화
            if (currentTime >= idleTime)
            {
                blackScreenCanvas.enabled = true; // idle Mode 잠금화면 활성화 
            }
        }
    }
}
