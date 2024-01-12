using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentFPS : MonoBehaviour
{
    public TextMeshProUGUI frameRateText;
    
    void Update()
    {
        frameRateText.text = string.Format("Current FPS: {0}", Application.targetFrameRate);
    }
}
