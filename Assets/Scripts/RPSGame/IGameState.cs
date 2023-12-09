using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameState
{
    public void Enter(GameStateManager stateManager);

    public void Update(GameStateManager stateManager);

    public void Exit(GameStateManager stateManager);
}
