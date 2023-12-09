using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStateManager : MonoBehaviour
{
    public StartGameState StartGameState;
    public GetReadyState GetReadyState;
    public ResolveGameState ResolveGameState;
    public BreakeState BreakeState;

    [SerializeField] private float _breakBetweenRounds;
    [SerializeField] private float _timeForPoseChoosing;
    [SerializeField] private HandPoseManager _handPoseManager;
    [SerializeField] private Display _display;
    private IGameState _currentState;

    void Awake()
    {
        this.InitializeStates();
    }

    private void InitializeStates()
    {
        this.StartGameState = new StartGameState(this._handPoseManager, this._display);
        this.GetReadyState = new GetReadyState(this._timeForPoseChoosing, this._display);
        this.ResolveGameState = new ResolveGameState(this._handPoseManager, this._display);
        this.BreakeState = new BreakeState(this._breakBetweenRounds);
        this._currentState = this.StartGameState;
        this._currentState.Enter(this);
    }

    void Update()
    {
        this._currentState.Update(this);
    }

    public void TransitionTo(IGameState nextState)
    {
        this._currentState.Exit(this);
        this._currentState = nextState;
        this._currentState.Enter(this);
    }
}
