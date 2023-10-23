using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using Leap;

public class CustomPostProcessProvider : PostProcessProvider
{
    [SerializeField]
    private Transform headTransform;
    [SerializeField]
    private float projectionScale = 10f;
    [SerializeField]
    private float handMergedDistance = 0.35f;

    public override void ProcessFrame(ref Frame inputFrame)
    {
        throw new System.NotImplementedException();
    }
}
