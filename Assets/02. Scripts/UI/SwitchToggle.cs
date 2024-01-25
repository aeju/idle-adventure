using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private RectTransform handleRectTransform;
    // 활성 상태
    [SerializeField] private Color backgroundActiveColor;
    [SerializeField] private Color handActiveColor;

    // 비활성 상태
    private Color backgroundDefaultColor;
    private Color handleDefaultColor;
    
    private Image backgroundImage;
    private Image handleImage;
    
    private Toggle toggle;
    private Vector2 handlePosition; // 핸들 기본 위치

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePosition = handleRectTransform.anchoredPosition;

        backgroundImage = handleRectTransform.parent.GetComponent<Image>();
        handleImage = handleRectTransform.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;
        
        // 토글 상태 변경 시 호출할 메소드 등록 
        toggle.onValueChanged.AddListener(OnSwitch);
        
        // 초기 상태 설정
        if (toggle.isOn)
            OnSwitch(true);
    }

    void OnSwitch(bool on)
    {
        /*
        if (on) // 활성화된 경우 
            handleRectTransform.anchoredPosition = handlePosition * -1; // 핸들의 위치 반대방향으로 뒤집기 
        else
        {
            handleRectTransform.anchoredPosition = handlePosition; // 핸들의 위치 원래로 되돌리기 
        }
        */
        
        /* 애니메이션 x 
        handleRectTransform.anchoredPosition = on ? handlePosition * -1 : handlePosition; // 핸들 위치: true면 반대로, false면 원래 위치로 
        backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor; // 배경 이미지 색상 : 활성화 -> activeColor, 비활성화 -> defaultColor
        handleImage.color = on ? handActiveColor : handleDefaultColor; // 핸들 이미지 색상 : 활성화 -> activeColor, 비활성화 -> defaultColor
        */
        
        // 토글 상태에 따라 핸들의 위치, 색상 변경 
        handleRectTransform.DOAnchorPos (on ? handlePosition * -1 : handlePosition, .4f).SetEase(Ease.InOutBack);
        backgroundImage.DOColor(on ? backgroundActiveColor : backgroundDefaultColor, .6f);
        handleImage.DOColor(on ? handActiveColor : handleDefaultColor, .4f);
    }

    // 리스터 제거 (메모리 누수 방지)
    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
