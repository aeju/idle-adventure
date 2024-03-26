using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 버프 적용 중 : 다른 버프 X
public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }

    public GameObject BuffActiveIcon;

    public float CoinMultiplier { get; set; } = 1; // 코인 획득량에 적용할 배수
    public float ExpMultiplier { get; set; } = 1; // 경험치 획득량에 적용할 배수

    public bool IsBuffActive { get; private set; }

    // 버프 활성화 / 비활성화 이벤트
    public event Action<Buff> OnBuffActivated;
    public event Action<Buff> OnBuffDeactivated;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        BuffIconOff();
    }
    
    public void BuffIconOn()
    {
        BuffActiveIcon.SetActive(true);
    }
    
    public void BuffIconOff()
    {
        BuffActiveIcon.SetActive(false);
    }
    
    // 버프를 활성화하는 메서드, 버프 객체(인자로 전달)와 함께 이벤트 발생
    public void ActivateBuff(Buff buff)
    {
        if (!IsBuffActive) // 비활성화된 상태
        {
            IsBuffActive = true;
            OnBuffActivated?.Invoke(buff);
        }
    }

    // 버프를 비활성화하는 메서드
    public void DeactivateBuff(Buff buff)
    {
        if (IsBuffActive) // 활성화된 상태
        {
            IsBuffActive = false;
            OnBuffDeactivated?.Invoke(buff);
        }
    }
}
