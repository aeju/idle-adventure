using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PlayerJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public GameObject joystickPrefab;
    private GameObject currentJoystick = null;
    private Image joystickBackground;
    private Image joystickHandle;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentJoystick == null)
        {
            currentJoystick = Instantiate(joystickPrefab, eventData.position, Quaternion.identity, transform);
            joystickBackground = currentJoystick.GetComponent<Image>();
            joystickHandle = currentJoystick.transform.GetChild(0).GetComponent<Image>();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - (Vector2)joystickBackground.transform.position;
        direction = Vector2.ClampMagnitude(direction, joystickBackground.rectTransform.sizeDelta.x / 2);
        joystickHandle.rectTransform.anchoredPosition = direction;
        
        // Add movement logic here based on 'direction'
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentJoystick != null)
        {
            Destroy(currentJoystick);
        }
    }
}
