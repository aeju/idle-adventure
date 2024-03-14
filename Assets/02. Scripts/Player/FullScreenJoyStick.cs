using UnityEngine;
using UnityEngine.EventSystems;

// 터치 : bg 나타남 -> 터치 해제 : 사라짐
// 드래그 : handle 
public class FullScreenJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform joystickBackground; // 배경 이미지
    [SerializeField] private RectTransform joystickHandle; // 핸들 이미지
    
    private Vector2 startHandlePosition; // 핸들 시작값
    public bool isDragging = false; // 드래그되는 동안만 입력값 받기
    
    public void OnPointerDown(PointerEventData eventData)
    {
        // 조이스틱 위치 = 터치 위치 
        joystickBackground.position = eventData.position; // Vector3로 받아와서 x
        joystickHandle.position = eventData.position;
        
        startHandlePosition = joystickHandle.anchoredPosition; // 핸들 시작값
        
        isDragging = true;
        JoystickOn();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;

        // 마우스 위치 -> (RectTransform의) 로컬 좌표로 변환 
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle
                (joystickBackground, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            Vector2 direction = localPoint;

            // 핸들 반경 전환
            float radius = joystickBackground.sizeDelta.x / 2;
            Vector2 clampedDirection = Vector2.ClampMagnitude(direction, radius);
            joystickHandle.anchoredPosition = clampedDirection;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        JoystickOff();
        ResetJoystickHandle(); // 핸들 위치 초기화
    }
    
    public void ResetJoystickHandle()
    {
        joystickHandle.anchoredPosition = Vector2.zero; // 핸들을 초기 위치로
    }

    private void JoystickOn()
    {
        joystickBackground.gameObject.SetActive(true);
        joystickHandle.gameObject.SetActive(true);
    }

    private void JoystickOff()
    {
        joystickBackground.gameObject.SetActive(false);
        joystickHandle.gameObject.SetActive(false);
    }

    public Vector2 GetInputDirection()
    {
        // 핸들 drag - 핸들 시작값(down)
        return (joystickHandle.anchoredPosition - startHandlePosition).normalized;
    }
}