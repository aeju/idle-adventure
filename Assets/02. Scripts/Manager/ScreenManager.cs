using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// On: 정해진 시간 동안 입력 x, Idle Mode Btn
// Off: 조이스틱 입력 최댓값
public class ScreenManager : MonoBehaviour
{
    public float idleTime = 60.0f; // 방치 시간 : 1분 
    private float currentTime;
    
    public Canvas idleModeCanvas; // idle Mode(검은 화면) 표시 Canvas
    private CountTime countTime;
    
    private void Start()
    {
        idleModeCanvas.enabled = false; 
        currentTime = 0; // 타이머 초기화

        countTime = gameObject.AddComponent<CountTime>();
    }

    private void Update()
    {
        //currentTime = 0;
        // 사용자의 입력이 감지되면 타이머를 초기화
        //if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1))
        if (Input.anyKey || Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            currentTime = 0;
            if (idleModeCanvas.enabled)
            {
                idleModeCanvas.enabled = false; 
                countTime.IdleModeOff();
            }
            
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
    
    // IdleModeBtn에서 호출
    public void ActivateIdleModeScreen()
    {
        if (!idleModeCanvas.enabled)
        {
            Debug.Log("Idle Mode Screen On (Button)");
            idleModeCanvas.enabled = true;
            countTime.IdleModeOn();
        }
    }
}
