using UnityEngine;
using UnityEngine.EventSystems;

// 터치 : bg 나타남 -> 터치 해제 : 사라짐
// 드래그 : handle 
public class FullScreenJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform joystickBackground;
    public RectTransform joystickHandle;
    
    public bool isDragging = false; // 드래그되는 동안만 입력값 받기
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("joystick down");
        // 조이스틱 위치 = 터치 위치 
        joystickBackground.position = eventData.position; // Vector3로 받아와서 x
        joystickHandle.position = eventData.position;
        
        isDragging = true;
        JoystickOn();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground.parent as RectTransform, eventData.position, null, out localPoint);
        
        // 드래그 방향 : 현재 위치 - 배경 위치 
        Vector2 direction = eventData.position - (Vector2)joystickBackground.position;
        // 핸들 반경 제한
        float radius = joystickBackground.sizeDelta.x / 2;
        Vector2 clampedDirection = Vector2.ClampMagnitude(direction, radius);
        joystickHandle.anchoredPosition = clampedDirection;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("joystick up");
        isDragging = false;
        JoystickOff();
    }

    public void JoystickOn()
    {
        joystickBackground.gameObject.SetActive(true);
        joystickHandle.gameObject.SetActive(true);
    }

    public void JoystickOff()
    {
        joystickBackground.gameObject.SetActive(false);
        joystickHandle.gameObject.SetActive(false);
    }

    public Vector2 GetInputDirection()
    {
        return (joystickHandle.anchoredPosition - joystickBackground.anchoredPosition).normalized;
    }
}