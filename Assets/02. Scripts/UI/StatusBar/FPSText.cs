using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// FrameRate 이벤트 구독, 변경할 때만 업데이트
public class FPSText : MonoBehaviour
{
    public TextMeshProUGUI frameRateText;
    
    void OnEnable()
    {
        FrameRate.OnFrameRateChanged += UpdateFrameRateText;
    }

    void OnDisable()
    {
        FrameRate.OnFrameRateChanged -= UpdateFrameRateText;
    }
    
    void UpdateFrameRateText(int frameRate)
    {
        frameRateText.text = string.Format("{0}FPS", frameRate);
    }
}
