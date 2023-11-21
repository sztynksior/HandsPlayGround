using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTimer : MonoBehaviour, ITimer
{
    private float _initialTimerTime;
    private float _timerTime;
    private bool _isRunning;

    void Awake()
    {
        this._initialTimerTime = 0;
        this._timerTime = 0;
        this._isRunning = false;
    }


    void Update()
    {
        if (_isRunning)
        {
            this._timerTime -= Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        this._isRunning = true;
    }

    public void StopTimer()
    {
        this._isRunning = false;
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
}
