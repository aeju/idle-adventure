using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PercentSlider : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI percentageText;
    public Slider slider;
    
    private void Awake()
    {
        slider = GetComponent<Slider>(); // Slider 컴포넌트 가져오기

        if (slider == null)
        {
            Debug.LogError("Slider component not found on this GameObject!");
        }

        if (percentageText == null)
        {
            Debug.LogError("TextMeshProUGUI component not assigned!");
        }
    }
    
    void Start()
    {
        if (slider != null && percentageText != null)
        {
            // 시작할 때 현재 슬라이더 값으로 텍스트 업데이트
            UpdatePercentageText(slider.value);

            // 슬라이더 값이 변경될 때마다 텍스트 업데이트
            slider.onValueChanged.AddListener(UpdatePercentageText);
        }
    }

    // 텍스트 업데이트
    private void UpdatePercentageText(float value)
    {
        // 슬라이더 값을 0-100 범위의 정수 퍼센트로 변환
        int percentage = Mathf.RoundToInt(value * 100f);
        
        // 텍스트 업데이트
        percentageText.text = $"{percentage}%";
    }

    // 슬라이더 값을 설정하는 메서드 (AutoPotionSlider에서 호출)
    public void SetSliderValue(float value)
    {
        slider.value = value;
        UpdatePercentageText(value);
    }
    
    // 현재 슬라이더 값을 반환
    public float GetSliderValue()
    {
        return slider.value;
    }

    private void OnDestroy()
    {
        // 리스너 제거 (메모리 누수 방지)
        slider.onValueChanged.RemoveListener(UpdatePercentageText);
    }
}
