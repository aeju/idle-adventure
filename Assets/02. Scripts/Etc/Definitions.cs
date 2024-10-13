using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Definitions : MonoBehaviour
{
    public enum CardRarity
    {
        Normal,
        Epic,
        Unique,
        Legend
    }

    // 타이머 ID (TimeManager)
    public enum TimeId
    {
        GamePlayTimer,
        IdleModeTimer,
    }
}
