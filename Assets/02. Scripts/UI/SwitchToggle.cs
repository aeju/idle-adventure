using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SwitchToggle : MonoBehaviour
{
    [SerializeField] private RectTransform handleRectTransform;
    [SerializeField] private Color backgroundActiveColor;
    [SerializeField] private Color handActiveColor;

    private Image backgroundImage;
    private Image handleImage;

    private Color backgroundDefaultColor;
    private Color handleDefaultColor;
    
    private Toggle toggle;
    private Vector2 handlePosition;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        handlePosition = handleRectTransform.anchoredPosition;

        backgroundImage = handleRectTransform.parent.GetComponent<Image>();
        handleImage = handleRectTransform.GetComponent<Image>();

        backgroundDefaultColor = backgroundImage.color;
        handleDefaultColor = handleImage.color;
        
        toggle.onValueChanged.AddListener(OnSwitch);
        
        if (toggle.isOn)
            OnSwitch(true);
    }

    void OnSwitch(bool on)
    {
        /*
        if (on)
            handleRectTransform.anchoredPosition = handlePosition * -1;
        else
        {
            handleRectTransform.anchoredPosition = handlePosition;
        }
        */
        
        /* 애니메이션 x 
        handleRectTransform.anchoredPosition = on ? handlePosition * -1 : handlePosition;
        backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor;
        handleImage.color = on ? handActiveColor : handleDefaultColor;
        */
        
        handleRectTransform.DOAnchorPos (on ? handlePosition * -1 : handlePosition, .4f).SetEase(Ease.InOutBack);
        backgroundImage.DOColor(on ? backgroundActiveColor : backgroundDefaultColor, .6f);
        handleImage.DOColor(on ? handActiveColor : handleDefaultColor, .4f);
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
