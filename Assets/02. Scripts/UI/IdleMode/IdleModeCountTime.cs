using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IdleModeCountTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI idleModeTime_text;
    
    private const string TimerId = "IdleModeTimer"; 

    void Start()
    {
        UpdateTimeDisplay();
    }
    
    void Update()
    {
        if (idleModeTime_text == null)
        {
            Debug.LogError("Failed to find TextMeshProUGUI component");
        }
        else 
        {
            UpdateTimeDisplay();
        }
    }
    
    private void UpdateTimeDisplay()
    {
        idleModeTime_text.text = FormatTime(TimeManager.Instance.GetTime(TimerId));
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
}
