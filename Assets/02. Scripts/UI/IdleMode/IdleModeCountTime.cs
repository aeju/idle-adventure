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
        idleModeTime_text.text = Utilities.FormatTimeHHMM(TimeManager.Instance.GetTime(TimerId));
    }
}
