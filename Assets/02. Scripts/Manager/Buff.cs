using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    protected float duration; // 버프 지속 시간

    // 버프 활성화
    public void Activate()
    {
        OnActivate();
        StartCoroutine(DeactivateAfterDuration());
    }

    // 버프 비활성화
    protected abstract void Deactivate();

    // 버프 활성화
    protected abstract void OnActivate();

    // 지정된 시간(duration) 후에 버프를 비활성화
    private IEnumerator DeactivateAfterDuration()
    {
        yield return new WaitForSeconds(duration * 60);
        Deactivate();
    }
}
