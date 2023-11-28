using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WaitingForPoseState : IDisplayState
{
    private HandPoseManager _handPoseManager;

    public WaitingForPoseState(HandPoseManager handPoseManager)
    {
        this._handPoseManager = handPoseManager;
    }

    public void Enter(GameDisplay gameDisplay)
    {
        gameDisplay.SetText("kamieñ, papier czy no¿yce?");
    }

    public void Update(GameDisplay gameDisplay)
    {
        if (this._handPoseManager.PoseIsActive()) 
        { 
            int random = new System.Random().Next(0, 3);
            
            switch(random) 
            { 
                case 0:
                    gameDisplay.TransitionTo(gameDisplay.RockState);
                    break;
                case 1:
                    gameDisplay.TransitionTo(gameDisplay.PaperState);
                    break;
                case 2:
                    gameDisplay.TransitionTo(gameDisplay.ScissorsState);
                    break;
            }
        }
    }

    public void Exit(GameDisplay gameDisplay)
    {
        gameDisplay.SetText("");
    }
}
