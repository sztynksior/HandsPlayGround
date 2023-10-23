using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class HandDataManager : MonoBehaviour
{
    public LeapProvider leapProvider;

    private void OnEnable()
    {
        leapProvider.OnUpdateFrame += OnUpdateFrame;
    }

    private void OnDisable()
    {
        leapProvider.OnUpdateFrame -= OnUpdateFrame;
    }

    private void OnUpdateFrame(Frame frame)
    {
        Hand leftHand = frame.GetHand(Chirality.Left);

        if (leftHand != null )
        {
            OnUpdateLeftHand(leftHand);
        }
    }

    private void OnUpdateLeftHand(Hand leftHand)
    {
        Finger thumb = leftHand.GetThumb();
        Bone thumbDistalBone = thumb.Bone(Bone.BoneType.TYPE_DISTAL);
        Bone thumbIntermediateBone = thumb.Bone(Bone.BoneType.TYPE_INTERMEDIATE);
        Bone thumbProximalBone = thumb.Bone(Bone.BoneType.TYPE_PROXIMAL);
        Bone thumbMetarcapalBone = thumb.Bone(Bone.BoneType.TYPE_METACARPAL);
    }
}
