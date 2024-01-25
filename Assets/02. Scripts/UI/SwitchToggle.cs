using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        
        handleRectTransform.anchoredPosition = on ? handlePosition * -1 : handlePosition;
        backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor;
        handleImage.color = on ? handActiveColor : handleDefaultColor;
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}
