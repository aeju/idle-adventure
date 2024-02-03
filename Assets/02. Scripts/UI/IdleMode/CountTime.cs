using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountTime : MonoBehaviour
{
    public float blackScreenTime;
    public bool isBlackScreenActive;
    
    public TextMeshProUGUI idleModeTime;
    
    // Start is called before the first frame update
    void Start()
    {
        idleModeTime.text = "0분";
    }

    void Update()
    {
        if (isBlackScreenActive)
        {
            blackScreenTime += Time.deltaTime;
            if (idleModeTime != null)
            {
                idleModeTime.text = FormatTime(blackScreenTime);
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
