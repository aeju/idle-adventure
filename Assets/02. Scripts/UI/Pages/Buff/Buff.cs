using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    public static event Action<Buff> OnBuffActivated;    
    public static event Action<Buff> OnBuffDeactivated;
    
    [SerializeField] public string buffName; // 버프 이름
    [SerializeField] public Sprite buffIconSprite; // 버프 이미지
    [SerializeField] public string buffEffect; // 버프 효과
    [SerializeField] public int IncreasePercentage = 20; // 증가율
    [SerializeField] [Tooltip("버프 지속 시간 (분)")] public float durationMinute; // 버프 지속 시간
    
    
    // 버프 활성화
    protected void Activate()
    {
        Debug.Log($"Buff activated at: {Time.time} seconds");
        OnActivate();
        OnBuffActivated?.Invoke(this);
        StartCoroutine(DeactivateAfterDuration());
    }
    
    // 버프 비활성화 (추상 메소드)
    protected abstract void Deactivate();

    // 버프 활성화 (추상 메소드)
    protected abstract void OnActivate();

    protected abstract void UpdateUI();

    // 지정된 시간(duration) 후에 버프를 비활성화
    private IEnumerator DeactivateAfterDuration()
    {
        float seconds = Utilities.MinutesToSeconds(durationMinute);
        yield return new WaitForSeconds(seconds);
        Deactivate();
        OnBuffDeactivated?.Invoke(this);
        Debug.Log($"Buff deactivated at: {Time.time} seconds");
    }
}
