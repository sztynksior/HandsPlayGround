using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitcher : MonoBehaviour
{
    [SerializeField] protected Material _SwitchOffMaterial;
    [SerializeField] protected Material _SwitchOnMaterial;
    [SerializeField] protected PowerSource _PowerSource;

    protected MeshRenderer switchingObjectMeshRenderer;

    protected bool isSwitchedOn = false;
    protected bool isPowerOn = false;

    protected void Awake()
    {
        this.switchingObjectMeshRenderer = GetComponent<MeshRenderer>();
        this.switchingObjectMeshRenderer.material = this._SwitchOffMaterial;

        if (this._PowerSource != null)
        {
            this._PowerSource.PowerSwitched += this.OnPowerSwitch;
        }
        else
        {
            this._PowerSource = GetComponentInParent<PowerSource>();

            if (this._PowerSource != null)
            {
                this._PowerSource.PowerSwitched += this.OnPowerSwitch;
            }
            else
            {
                this.isPowerOn = true;
            }
        }
    }

    public void Switch()
    {
        if (this.isPowerOn) 
        {
            if (this.isSwitchedOn)
            {
                this.isSwitchedOn = false;
                this.switchingObjectMeshRenderer.material = this._SwitchOffMaterial;
            }
            else
            {
                this.isSwitchedOn = true;
                this.switchingObjectMeshRenderer.material = this._SwitchOnMaterial;
            }
        }
    }

    private void OnPowerSwitch(bool isPowerOn)
    {
        this.isPowerOn = isPowerOn;

        if (!isPowerOn) 
        {
            this.isSwitchedOn = false;
            this.switchingObjectMeshRenderer.material = this._SwitchOffMaterial;
        }
    }

    private void OnDestroy()
    {
        if (this._PowerSource != null) 
        {
            this._PowerSource.PowerSwitched -= this.OnPowerSwitch;
        }
    }
}
