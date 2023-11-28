using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PaperState : IDisplayState
{
    private HandPoseManager _handPoseManager;

    public PaperState(HandPoseManager handPoseManager)
    {
        this._handPoseManager = handPoseManager;
    }

    public void Enter(GameDisplay gameDisplay)
    {
        gameDisplay.SetText("papier");
    }

    public void Update(GameDisplay gameDisplay)
    {
        string activePose = this._handPoseManager.GetActivePose();

        if (activePose == "rock")
        {
            gameDisplay.TransitionTo(gameDisplay.LoseState);
        }
        else if (activePose == "paper")
        {
            gameDisplay.TransitionTo(gameDisplay.DrawState);
        }
        else if (activePose == "scissors")
        {
            gameDisplay.TransitionTo(gameDisplay.WinState);
        }
    }

    public void Exit(GameDisplay gameDisplay) { }
}
