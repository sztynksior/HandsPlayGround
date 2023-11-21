using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public interface ITimer
{
    public void StartTimer();

    public void StopTimer();

    public float GetTime();

    public void ResetTimer();

    public void SetInitialTime(float initialTime);
}