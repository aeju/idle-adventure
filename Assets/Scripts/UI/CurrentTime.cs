using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentTime : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    
    void Start()
    {
        
    }

    void Update()
    {
        DateTime currentTime = DateTime.Now; //using System; 필요
        timeText.text = currentTime.ToString("HH:mm:ss");
    }
}
