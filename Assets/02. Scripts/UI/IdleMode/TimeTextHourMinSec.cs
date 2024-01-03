using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// HH:mm:ss (절전 모드)
public class TimeTextHourMinSec : MonoBehaviour
{
    public TextMeshProUGUI timeTextHourMinSec;

    void Update()
    {
        if (timeTextHourMinSec != null)
        {
            timeTextHourMinSec.text = TimeManager.GetCurrentTimeKST().ToString("HH:mm:ss");
        }
    }
}
