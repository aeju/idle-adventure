using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }

    public GameObject BuffActiveIcon;
    
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

    private void Start()
    {
        BuffIconOff();
    }

    private void BuffIconOn()
    {
        
    }
    
    private void BuffIconOff()
    {
        BuffActiveIcon.SetActive(false);
    }
}
