using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public float TimeElapsed { get; private set; }
    
    void Update()
    {
        TimeElapsed += Time.deltaTime;
    }
    
    public void ResetTime()
    {
        TimeElapsed = 0;
    }
}
