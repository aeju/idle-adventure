using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPotionSlider : MonoBehaviour
{
    [SerializeField] private PercentSlider autoPotionSlider;
    
    private void Start()
    {
        // 저장된 값 불러오기 (기본값: 70%)
        float savedPercent = OptionManager.Instance.GetInt(OptionManager.AutoPotionPercentKey, 70) / 100f;
        
        // 슬라이더 초기값 설정
        autoPotionSlider.SetSliderValue(savedPercent);
        
        // PotionManager의 값 업데이트
        UpdatePotionManagerValue(savedPercent);
        
        // 슬라이더 값 변경 시 이벤트 연결
        autoPotionSlider.slider.onValueChanged.AddListener(OnAutoPotionSliderChanged);
    }

    private void OnAutoPotionSliderChanged(float value)
    {
        // 슬라이더 값이 변경될 때마다 PotionManager와 OptionManager 업데이트
        UpdatePotionManagerValue(value);
        SaveAutoPotionPercent(value);
    }

    private void UpdatePotionManagerValue(float value)
    {
        // PotionManager의 autoPotionPercent 업데이트
        PotionManager.Instance.autoPotionPercent = value;
    }

    private void SaveAutoPotionPercent(float value)
    {
        // OptionManager를 통해 값 저장 (정수로 변환하여 저장)
        int percentAsInt = Mathf.RoundToInt(value * 100f);
        OptionManager.Instance.SetInt(OptionManager.AutoPotionPercentKey, percentAsInt);
    }

    private void OnDestroy()
    {
        // 리스너 제거
        if (autoPotionSlider != null && autoPotionSlider.slider != null)
        {
            autoPotionSlider.slider.onValueChanged.RemoveListener(OnAutoPotionSliderChanged);
        }
    }
}
