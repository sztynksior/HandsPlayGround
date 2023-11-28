using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTimeCounter : Counter
{
    public DownTimeCounter(float initialTimerTime, float stoppingCount, bool isRunning) : base(initialTimerTime, stoppingCount, isRunning) { }

    public override void Update()
    {
        if (this.IsRunning()) 
        {
            base._currentCount -= Time.deltaTime;

            if (base._currentCount <= base._stoppingCount)
            {
                base._currentCount = base._stoppingCount;
                this.Stop();
            }
        }
    }
}
