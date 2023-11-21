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
    [SerializeField]
    private float v1 = 0f;
    [SerializeField]
    private float v2 = -0.13f;
    [SerializeField]
    private float v3 = -0.1f;
    [SerializeField]
    private float v4 = 0.15f;

    public override void ProcessFrame(ref Frame inputFrame)
    {
        if (headTransform == null)
        {
            headTransform = Camera.main.transform;
        }

        Vector3 headPos = headTransform.position;

        Quaternion shoulderBasis = Quaternion.LookRotation(Vector3.ProjectOnPlane(headTransform.forward, Vector3.up), Vector3.up);

        foreach (Hand hand in inputFrame.Hands) 
        {
            // Approximate shoulder posiotion with magic values.
            Vector3 shoulderPos = headPos + (shoulderBasis * (new Vector3(v1, v2, v3) + Vector3.left * v4 * (hand.IsLeft ? 1f : -1f)));

            //Calculate the projection of the hand if it extends beyond the handMergeDistance.
            Vector3 shoulderToHand = hand.PalmPosition - shoulderPos;
            float handShoulderDist = shoulderToHand.magnitude;
            float projectionDistance = Mathf.Max(0f, handShoulderDist - handMergedDistance);
            float projectonAmount = 1f + (projectionDistance * projectionScale);

            hand.SetTransform(shoulderPos + shoulderToHand * projectonAmount, hand.Rotation);
        }
    }
}
