using System.Collections.Generic;

public class TimeManager : Singleton<TimeManager>
{
    private Dictionary<Definitions.TimeId, float> timers = new Dictionary<Definitions.TimeId, float>();
    
    public void UpdateTimer(Definitions.TimeId timerId, float deltaTime)
    {
        if (!timers.ContainsKey(timerId))
        {
            timers[timerId] = 0;
        }
        timers[timerId] += deltaTime;
    }
    
    public float GetTime(Definitions.TimeId timerId)
    {
        return timers.ContainsKey(timerId) ? timers[timerId] : 0;
    }
    
    public void ResetTimer(Definitions.TimeId timerId)
    {
        if (timers.ContainsKey(timerId))
        {
            timers[timerId] = 0;
        }
    }
}
