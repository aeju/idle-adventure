using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickTest : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown called.");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp called.");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag called. Delta: " + eventData.delta);
    }
}
