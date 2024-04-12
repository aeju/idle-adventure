using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// HH:mm:ss (절전 모드)
public class TimeTextHourMinSec : TimeTextUpdater
{
    protected override string GetTimeString()
    {
        return Utilities.GetCurrentTimeKST().ToString("HH:mm:ss");
    }
}
