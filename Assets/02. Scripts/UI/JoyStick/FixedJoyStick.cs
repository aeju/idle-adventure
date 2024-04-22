using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoyStick : MonoBehaviour,IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Vector2 joystickInput;
    [SerializeField] private RectTransform handle;
    private RectTransform joystickRectTransform;

    private void Start()
    {
        joystickRectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - (Vector2)joystickRectTransform.position;
        direction = Vector2.ClampMagnitude(direction, joystickRectTransform.sizeDelta.x / 2);
        handle.anchoredPosition = direction;

        joystickInput = direction / (joystickRectTransform.sizeDelta.x / 2);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.anchoredPosition = Vector2.zero;
        joystickInput = Vector2.zero;
    }
}
