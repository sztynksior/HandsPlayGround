using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetReadyState : IGameState
{
    private Counter _timer;
    private Display _display;

    public GetReadyState(float  stateTime, Display display)
    {
        this._timer = new DownTimeCounter(stateTime, 0, false);
        this._display = display;
    }

    public void Enter(GameStateManager stateManager)
    {
        this._timer.Start();
    }

    public void Update(GameStateManager stateManager)
    {
        this._timer.Update();
        this._display.SetMainDisplayText(this._timer.GetCurrentCount().ToString());

        if (this._timer.IsRunning() == false)
        {
            stateManager.TransitionTo(stateManager.ResolveGameState);
        }
    }

    public void Exit(GameStateManager stateManager)
    {
        this._timer.Reset();
    }
}
