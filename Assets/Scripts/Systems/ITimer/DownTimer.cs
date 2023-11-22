using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTimer : ITimer
{
    private float _initialTimerTime;
    private float _timerTime;
    private bool _isRunning;

    public DownTimer()
    {
        this._initialTimerTime = 0;
        this._timerTime = 0;
        this._isRunning = false;
    }

    public DownTimer(float initialTimerTime, float timerTime, bool isRunning)
    {
        this._initialTimerTime = initialTimerTime;
        this._timerTime = timerTime;
        this._isRunning = isRunning;
    }

    public float GetTime()
    {
        return this._timerTime;
    }

    public void ResetTimer()
    {
        this._timerTime = this._initialTimerTime;
    }

    public void SetInitialTime(float initialTime)
    {
        this._initialTimerTime = initialTime;
    }

    public void StartTimer()
    {
        this._isRunning = true;
    }

    public void StopTimer()
    {
        this._isRunning = false;
    }

    public void UpdateTime()
    {
        if (_isRunning)
        {
            this._timerTime -= Time.deltaTime;
        }
    }
}
