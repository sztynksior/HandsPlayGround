using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Counter
{
    private float _initialCount;
    private bool _isRunning;

    protected float _currentCount;
    protected float _stoppingCount;

    protected Counter(float initialCount, float stoppingCount, bool isRunning)
    {
        this._initialCount = initialCount;
        this._currentCount = initialCount;
        this._stoppingCount = stoppingCount;
        this._isRunning = isRunning;
    }

    public float GetCurrentCount()
    {
        return this._currentCount;
    }

    public bool IsRunning()
    {
        return this._isRunning;
    }

    public void Reset()
    {
        this._currentCount = this._initialCount;
    }

    public void Start()
    {
        this._isRunning = true;
    }

    public void Stop()
    {
        this._isRunning = false;
    }

    abstract public void Update();
}
