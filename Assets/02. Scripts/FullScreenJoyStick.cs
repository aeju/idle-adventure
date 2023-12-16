using UnityEngine;
using UnityEngine.EventSystems;

public class FullScreenJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;

    public void Start()
    {
        Debug.Log("joystick start");
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("joystick down");
        joystickBackground.position = eventData.position;
        joystickHandle.position = eventData.position;
        joystickBackground.gameObject.SetActive(true);
        joystickHandle.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("joystick drag");
        Vector2 direction = eventData.position - (Vector2)joystickBackground.position;
        //float radius = joystickBackground.sizeDelta.x / 2; // 절반으로 제한
        float radius = joystickBackground.sizeDelta.x / 4;
        Vector3 newHandlePosition = (Vector3)Vector2.ClampMagnitude(direction, radius);
        joystickHandle.position = joystickBackground.position + newHandlePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("joystick up");
        joystickBackground.gameObject.SetActive(false);
        joystickHandle.gameObject.SetActive(false);
    }
}