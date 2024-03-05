using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEnforceObserver : MonoBehaviour
{
    // 옵저버 클래스 -> Observer 클래스 상속받아, 서브젝트를 파라미터로 받는 Notify() 추상 메서드 구현해야
    public abstract void Notify(IEnforceSubject subject);
}
