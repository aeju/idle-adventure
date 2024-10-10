using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public interface IOptionComponent
{
    void Init();
    void LoadState();
    void SaveState();
}

public abstract class OptionUI : MonoBehaviour, IOptionComponent
{
    protected const int DEFAULT_VALUE = 1;

    public abstract void Init();
    public abstract void SaveState();
    public abstract void LoadState();
    
    protected virtual void Start()
    {
        Init();
        LoadState();
    }

    protected virtual void OnDestroy()
    {
        SaveState();
    }
}
