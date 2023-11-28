using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoseManager : MonoBehaviour
{
    private string _activePose;
    private bool _isActive;
    private void Awake()
    {
        this._activePose = "";
    }
    public void SetPose(string pose)
    {
        this._isActive = true;
        this._activePose = pose;
    }

    public void ResetPose()
    {
        this._activePose = "";
    }

    public string GetActivePose()
    {
        this._isActive = false;
        return this._activePose;
    }

    public bool PoseIsActive()
    {
        return this._isActive;
    }
}
