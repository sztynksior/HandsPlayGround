using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSource : MonoBehaviour
{
    public event Action<bool> PowerSwitched;

    private bool isPowerOn = false;

    public void SwitchPower()
    {
        if (this.isPowerOn) 
        {
            this.isPowerOn = false;
        }
        else
        {
            this.isPowerOn = true;
        } 
        
        this.PowerSwitched?.Invoke(this.isPowerOn);
    }
}
