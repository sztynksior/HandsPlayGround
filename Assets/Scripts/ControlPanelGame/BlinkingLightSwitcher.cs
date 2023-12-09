using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLightSwitcher : LightSwitcher
{
    [SerializeField] private float _frequency;

    private bool lightIsOn = false;
    private float timeToChange = 0;

    private void Update()
    {
        if (base.isPowerOn)
        {
            this.timeToChange += Time.deltaTime;

            if (base.isSwitchedOn)
            {
                if (this.timeToChange > this._frequency)
                {
                    if (this.lightIsOn)
                    {
                        base.switchingObjectMeshRenderer.material = _SwitchOffMaterial;
                        this.lightIsOn = false;
                    }
                    else
                    {
                        base.switchingObjectMeshRenderer.material = _SwitchOnMaterial;
                        this.lightIsOn = true;
                    }

                    this.timeToChange = 0;
                }
            }
        }
    }
}
