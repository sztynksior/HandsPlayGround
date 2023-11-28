using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseState : IDisplayState
{
    private Counter _downTimer;

    public LoseState(float breakeBetweenRounds)
    {
        this._downTimer = new DownTimeCounter(breakeBetweenRounds, 0, false);
    }

    public void Enter(GameDisplay gameDisplay)
    {
        this._downTimer.Start();
        gameDisplay.AddText("\nPrzegra³eœ!");
    }

    public void Update(GameDisplay gameDisplay)
    {
        this._downTimer.Update();

        if (this._downTimer.IsRunning() == false)
        {
            gameDisplay.TransitionTo(gameDisplay.StartGameState);
        }
    }

    public void Exit(GameDisplay gameDisplay)
    {
        this._downTimer.Reset();
        gameDisplay.SetText("");
    }
}
