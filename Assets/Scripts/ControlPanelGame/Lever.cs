using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Lever : MonoBehaviour
{
    [SerializeField] private UnityEvent _leverUp = new UnityEvent();
    [SerializeField] private UnityEvent _leverDown = new UnityEvent();
    [SerializeField] private Vector2 _onDegreesRange;
    [SerializeField] private Vector2 _offDegreesRange;

    private bool wasLeverUp = false;
    private Transform leverTransform;

    private void Awake()
    {
        this.leverTransform = GetComponent<Transform>();
    }
    private void Update()
    {
        float xRotation = this.leverTransform.localRotation.eulerAngles.x;
        Vector2 onDegreesRange = new Vector2(NormalizeDegrees(this._onDegreesRange.x), NormalizeDegrees(this._onDegreesRange.y));
        Vector2 offDegreesRange = new Vector2(NormalizeDegrees(this._offDegreesRange.x), NormalizeDegrees(this._offDegreesRange.y));

        if (!this.wasLeverUp && xRotation > onDegreesRange.x && xRotation < onDegreesRange.y) 
        { 
            this.OnLeverUp();
            this.wasLeverUp = true;
        }
        else if (wasLeverUp && xRotation > offDegreesRange.x && xRotation < offDegreesRange.y)
        {
            this.OnLeverDown();
            this.wasLeverUp = false;
        }
    }

    private void OnLeverUp()
    {
        this._leverUp.Invoke();
    }

    private void OnLeverDown()
    {
        this._leverDown.Invoke();
    }

    public static float NormalizeDegrees(float degrees)
    {
        float normalizedDegrees = degrees % 360;

        if (normalizedDegrees < 0)
        {
            normalizedDegrees += 360;
        }

        return normalizedDegrees;
    }
}
