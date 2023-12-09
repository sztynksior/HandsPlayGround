using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameState : IGameState
{
    private HandPoseManager _handPoseManager;
    private Display _display;

    public StartGameState(HandPoseManager handPoseManager, Display display)
    {
        this._handPoseManager = handPoseManager;
        this._display = display;
    }
    public void Enter(GameStateManager stateManager)
    {

    }

    public void Update(GameStateManager stateManager)
    {
        if (this._handPoseManager.GetPose() == HandPose.ThumbsUp)
        {
            stateManager.TransitionTo(stateManager.GetReadyState);
            return;
        }
        this._display.SetText("Poka¿ kciuk w górê aby rozpocz¹æ grê.");
    }

    public void Exit(GameStateManager stateManager)
    {

    }
}
