using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class IEnforceSubject : MonoBehaviour
{
    private readonly ArrayList observers = new ArrayList();

    // 옵저버 객체 목록에 옵저버 객체 추가
    public void Attach(IEnforceObserver observer)
    {
        observers.Add(observer);
    }
    
    // 옵저버 객체 목록에 옵저버 객체 제거
    public void Detach(IEnforceObserver observer)
    {
        observers.Remove(observer);
    }

    // 옵저버 객체 목록 루프하면서 Notify() 호출
    public void NotifyObservers()
    {
        foreach (IEnforceObserver observer in observers)
        {
            observer.Notify(this);
        }
    }
}
