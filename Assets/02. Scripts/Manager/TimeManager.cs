using UnityEngine;
using System.Collections.Generic;

public class TimeManager : Singleton<TimeManager>
{
    private Dictionary<string, float> timers = new Dictionary<string, float>();
    
    //public float TimeElapsed { get; private set; }
    
    public void UpdateTimer(string timerId, float deltaTime)
    {
        if (!timers.ContainsKey(timerId))
        {
            timers[timerId] = 0;
        }
        timers[timerId] += deltaTime;
    }

    public float GetTime(string timerId)
    {
        return timers.ContainsKey(timerId) ? timers[timerId] : 0;
    }

    public void ResetTimer(string timerId)
    {
        if (timers.ContainsKey(timerId))
        {
            timers[timerId] = 0;
        }
    }
}
