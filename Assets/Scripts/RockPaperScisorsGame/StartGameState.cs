using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameState : IDisplayState
{
    private HandPoseManager _handPoseManager;

    public StartGameState(HandPoseManager handPoseManager)
    {
        this._handPoseManager = handPoseManager;
    }
    public void Enter(GameDisplay gameDisplay)
    {
        gameDisplay.SetText("kciuk w górê aby rozpocz¹æ grê");
    }

    public void Update(GameDisplay gameDisplay)
    {
        if (this._handPoseManager.GetActivePose() == "thumbsup")
        {
            gameDisplay.TransitionTo(gameDisplay.BreakState);
        }
    }

    public void Exit(GameDisplay gameDisplay)
    {
        gameDisplay.SetText("");
    }
}
