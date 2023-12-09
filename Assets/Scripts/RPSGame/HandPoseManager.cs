using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public enum HandPose
{
    None,
    Fist,
    ThumbsUp,
    Scissors,
    OpenPalm
}

public class HandPoseManager : MonoBehaviour
{

    private HandPose _activePose;
    private bool _isActive;
    private Dictionary<string, HandPose> _stringToHandPose;

    private void Awake()
    {
        this._activePose = HandPose.None;
        this.InitializeStringToHandPoses();
    }

    private void InitializeStringToHandPoses()
    {
        this._stringToHandPose = new Dictionary<string, HandPose>()
        {
            {"Fist", HandPose.Fist},
            {"ThumbsUp", HandPose.ThumbsUp},
            {"Scissors", HandPose.Scissors},
            {"OpenPalm", HandPose.OpenPalm}
        };
    }

    public void SetPose(string pose)
    {
        if (this._stringToHandPose.ContainsKey(pose))
        {
            this._activePose = this._stringToHandPose[pose];
        }
        else
        {
            this._activePose = HandPose.None;
        }
    }

    public void ResetPose()
    {
        this._activePose = HandPose.None;
    }

    public HandPose GetPose()
    {
        return this._activePose;
    }

    public static HandPose GetRandomPoseFromList(List<HandPose> handPosesList)
    {
        if (handPosesList == null || handPosesList.Count == 0)
        {
            return HandPose.None;
        }
        else
        {
            System.Random random = new System.Random();
            int randomNumber = random.Next(0, handPosesList.Count);

            return handPosesList[randomNumber];
        }
    }
}
