using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentTime : MonoBehaviour
{
    public TextMeshProUGUI timeTextHourMin; // HH:mm (상단바) 
    public TextMeshProUGUI timeTextHourMinSec; // HH:mm:ss 
    
    void Update()
    {
        DateTime currentTime = DateTime.Now; //using System; 필요
        
        if (timeTextHourMinSec != null) // HH:mm:ss
        {
            timeTextHourMinSec.text = currentTime.ToString("HH:mm:ss");
        }

        if (timeTextHourMin != null) // HH:mm
        {
            timeTextHourMin.text = currentTime.ToString("HH:mm");
        }
    }
}
