using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// HH:mm (상단바) 
public class TimeTextHourMin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeTextHourMin;

    void Update()
    {
        if (timeTextHourMin != null)
        {
            timeTextHourMin.text = Utilities.GetCurrentTimeKST().ToString("HH:mm");
        }
    }
}
