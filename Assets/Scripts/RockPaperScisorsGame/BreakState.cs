using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakState : IDisplayState
{
    private HandPoseManager _handPoseManager;
    private Counter _downTimer;

    public BreakState(HandPoseManager handPoseManager, float breakBetweenRounds) 
    {
        this._handPoseManager = handPoseManager;
        this._downTimer = new DownTimeCounter(breakBetweenRounds, 0, false);
    }

    public void Enter(GameDisplay gameDisplay)
    {
        this._downTimer.Start();
    }

    public void Update(GameDisplay gameDisplay)
    {
        this._downTimer.Update();
        gameDisplay.SetText(this._downTimer.GetCurrentCount().ToString());

        if (this._downTimer.IsRunning() == false)
        {
            if (this._handPoseManager.PoseIsActive())
            {
                int random = new System.Random().Next(0, 3);

                switch (random)
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
    }

    public void Exit(GameDisplay gameDisplay)
    {
        this._downTimer.Reset();
        gameDisplay.SetText("");
    }
}
