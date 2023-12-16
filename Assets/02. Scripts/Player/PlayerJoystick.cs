using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// background : 터치한 곳에 생성
// handle : 터치한 곳에서 드래그한 방향의 지점에 생성 

public class PlayerJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform background;   // Joystick background
    public RectTransform handle;       // Joystick handle

    private Vector2 joystickPosition = Vector2.zero;
    private RectTransform rectTransform;

    void Start()
    {
        Debug.Log(("Joystick"));
        rectTransform = GetComponent<RectTransform>();
        background.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(("OnPointerDown"));
        background.gameObject.SetActive(true);
        handle.gameObject.SetActive(true);
        joystickPosition = eventData.position;
        background.position = eventData.position;
        handle.position = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(("OnDrag"));
        Vector2 direction = eventData.position - joystickPosition;
        handle.position = joystickPosition + Vector2.ClampMagnitude(direction, background.sizeDelta.x / 2);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(("OnPointerUp"));
        background.gameObject.SetActive(false);
        handle.gameObject.SetActive(false);
    }
}