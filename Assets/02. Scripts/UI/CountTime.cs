using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// black screen이 활성화 -> 시간 측정
public class CountTime : MonoBehaviour
{
    public float blackScreenTime;
    public bool isBlackScreenActive;

    public TextMeshProUGUI idleModeTime_text;

    void Start()
    {
        idleModeTime_text = GameObject.Find("Idle Mode Count Time").GetComponent<TextMeshProUGUI>();
        
        if (idleModeTime_text == null)
        {
            Debug.LogError("Failed to find TextMeshProUGUI component");
        }
    }
    
    public void IdleModeOn()
    {
        Debug.Log("Idle Mode On");
        isBlackScreenActive = true;
        blackScreenTime = 0; // 타이머 초기화 
    }

    public void IdleModeOff()
    {
        isBlackScreenActive = false;
    }

    void Update()
    {
        if (isBlackScreenActive)
        {
            blackScreenTime += Time.deltaTime;
            if (idleModeTime_text != null)
            {
                idleModeTime_text.text = FormatTime(blackScreenTime);
            }
            else
            {
                Debug.LogError("TextMeshProUGUI is not assigned in CountTime");
            }
        }
    }

    // 시간 형식 : 60분 이하 -> @@분, 60분 이상 -> @시간 @분
    private string FormatTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);

        if (hours > 0)
        {
            return hours + "시간 " + minutes + "분";
        }
        else
        {
            return minutes + "분";
        }
    }

    public float GetCountTime()
    {
        return blackScreenTime;
    }
}
