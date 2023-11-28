using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScissorsState : IDisplayState
{
    private HandPoseManager _handPoseManager;

    public ScissorsState(HandPoseManager handPoseManager)
    {
        this._handPoseManager = handPoseManager;
    }

    public void Enter(GameDisplay gameDisplay)
    {
        gameDisplay.SetText("no¿yce");
    }

    public void Update(GameDisplay gameDisplay)
    {
        string activePose = this._handPoseManager.GetActivePose();

        if (activePose == "rock")
        {
            gameDisplay.TransitionTo(gameDisplay.WinState);
        }
        else if (activePose == "paper")
        {
            gameDisplay.TransitionTo(gameDisplay.LoseState);
        }
        else if (activePose == "scissors")
        {
            gameDisplay.TransitionTo(gameDisplay.DrawState);
        }
    }

    public void Exit(GameDisplay gameDisplay) { }
}
