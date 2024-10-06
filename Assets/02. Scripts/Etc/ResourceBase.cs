using System;
using UnityEngine;

// 리소스 관리 추상 클래스 (당장: 포션, 이후 - 코인 ...)
public abstract class ResourceBase<T> : Singleton<T> where T : MonoBehaviour
{
    // 리소스가 업데이트될 때 발생하는 이벤트 
    public event Action OnResourcesUpdated;

    // 현재 리소스 개수 
    protected int currentResource = 0;

    // 리소스 추가 
    protected virtual void AddResource(int amount)
    {
        currentResource += amount;
        OnResourcesUpdated?.Invoke(); // 리소스 추가 이벤트 발생 
    }

    // 리소스 사용 
    protected virtual void UseResource(int amount)
    {
        if (currentResource >= amount)
        {
            currentResource -= amount;
            OnResourcesUpdated?.Invoke(); // 리소스 감소 이벤트 발생 
        }
    }

    // 현재 리소스 개수 반환 
    public int GetCurrentResource() => currentResource;
}
