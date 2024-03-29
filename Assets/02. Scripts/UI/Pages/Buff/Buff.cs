using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuffType
{
    Coin,
    Exp
}


public abstract class Buff : MonoBehaviour
{
    [SerializeField] public string buffName; // 버프 이름
    [SerializeField] public Sprite buffIconSprite; // 버프 이미지
    [SerializeField] public string buffEffect; // 버프 효과
    [SerializeField] public int IncreasePercentage = 20; // 증가율
    [SerializeField] [Tooltip("버프 지속 시간 (분)")] public float durationMinute; // 버프 지속 시간

    public BuffType buffType;
    
    // 버프 활성화
    public void Activate()
    {
        Debug.Log($"Buff activated at: {Time.time} seconds");
        OnActivate();
        
        // BuffManager를 통해 코루틴 호출 (지속 시간)
        BuffManager.Instance.ActivateBuff(this, durationMinute); 
    }

    // 버프 활성화 (추상 메소드)
    protected abstract void OnActivate();

    protected abstract void UpdateUI();
}
