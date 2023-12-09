using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakeState : IGameState
{
    private Counter _timer;

    public BreakeState(float breakTime)
    {
        this._timer = new DownTimeCounter(breakTime, 0, false);
    }

    public void Enter(GameStateManager stateManager)
    {
        this._timer.Start();
    }

    public void Update(GameStateManager stateManager)
    {
        this._timer.Update();

        if (this._timer.IsRunning() == false)
        {
            stateManager.TransitionTo(stateManager.StartGameState);
        }
    }

    public void Exit(GameStateManager stateManager)
    {
        this._timer.Reset();
    }
}
