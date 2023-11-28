using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockState : IDisplayState
{
    private HandPoseManager _handPoseManager;

    public RockState(HandPoseManager handPoseManager)
    {
        this._handPoseManager = handPoseManager;
    }

    public void Enter(GameDisplay gameDisplay)
    {
        gameDisplay.SetText("kamieñ");
    }

    public void Update(GameDisplay gameDisplay)
    {
        string activePose = this._handPoseManager.GetActivePose();

        if (activePose == "rock")
        {
            gameDisplay.TransitionTo(gameDisplay.DrawState);
        }
        else if (activePose == "paper")
        {
            gameDisplay.TransitionTo(gameDisplay.WinState);
        }
        else if (activePose == "scissors")
        {
            gameDisplay.TransitionTo(gameDisplay.LoseState);
        }
    }

    public void Exit(GameDisplay gameDisplay) { }
}
