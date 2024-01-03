using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// HH:mm (상단바) 
public class TimeTextHourMin : MonoBehaviour
{
    public TextMeshProUGUI timeTextHourMin;

    void Update()
    {
        if (timeTextHourMin != null)
        {
            timeTextHourMin.text = TimeManager.GetCurrentTimeKST().ToString("HH:mm");
        }
    }
}
