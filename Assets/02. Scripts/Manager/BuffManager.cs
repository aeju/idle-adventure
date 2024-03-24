using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }

    public GameObject BuffActiveIcon;

    public float CoinMultiplier { get; set; } = 1; // 코인 획득량에 적용할 배수

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
}
