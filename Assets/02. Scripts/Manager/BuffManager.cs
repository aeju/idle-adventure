using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }

    public GameObject BuffActiveIcon;

    public float CoinMultiplier { get; set; } = 1; // 코인 획득량에 적용할 배수

    public bool IsBuffActive { get; private set; }

    // 버프 활성화 및 비활성화 이벤트
    public event Action OnBuffActivated;
    public event Action OnBuffDeactivated;
    
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
    
    // 버프를 활성화하는 메서드
    public void ActivateBuff()
    {
        if (!IsBuffActive) // 비활성화된 상태
        {
            IsBuffActive = true;
            OnBuffActivated?.Invoke();
        }
    }

    // 버프를 비활성화하는 메서드
    public void DeactivateBuff()
    {
        if (IsBuffActive) // 활성화된 상태
        {
            IsBuffActive = false;
            OnBuffDeactivated?.Invoke();
        }
    }
}
