using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// On: 정해진 시간 동안 입력 x, Idle Mode Btn
// Off: 조이스틱 입력 최댓값
public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }
    
    public float idleTime = 30.0f; // 방치 시간 : 30초 
    private float currentTime;
    
    public Canvas idleModeCanvas; // idle Mode(검은 화면) 표시 Canvas
    public CountTime countTime;
    
    private bool isIdleModeActive = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep the instance alive across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    
    private void Start()
    {
        idleModeCanvas.enabled = false;
        ResetIdleModeTimer(); // 타이머 초기화
    }

    private void Update()
    {

        if (!isIdleModeActive)
        {
            // 입력값이 있으면, 절전모드 타이머 초기화
            if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                ResetIdleModeTimer();
            }

            else
            {
                currentTime += Time.deltaTime; // 입력이 없을 경우 타이머 증가

                // 지정된 방치 시간이 초과되면 검은 화면을 활성화
                if (currentTime >= idleTime && !idleModeCanvas.enabled)
                {
                    Debug.Log("Idle Mode Screen On");
                    idleModeCanvas.enabled = true; // idle Mode 잠금화면 활성화
                    countTime.IdleModeOn();
                }
            }
        }
    }
    
    // IdleModeBtn에서 호출
    public void ActivateIdleModeScreen()
    {
        if (!idleModeCanvas.enabled)
        {
            idleModeCanvas.enabled = true;
            countTime.IdleModeOn();
        }
    }
    
    public void DeactivateIdleModeCanvas()
    {
        if (idleModeCanvas.enabled)
        {
            idleModeCanvas.enabled = false;
            countTime.IdleModeOff();
        }
    }
    
    public void ResetIdleModeTimer()
    {
        currentTime = 0;
    }
}
