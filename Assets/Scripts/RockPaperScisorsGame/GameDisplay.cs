using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameDisplay : MonoBehaviour
{
    public StartGameState StartGameState;
    public BreakState BreakState;
    public WinState WinState;
    public DrawState DrawState;
    public LoseState LoseState;
    public RockState RockState;
    public PaperState PaperState;
    public ScissorsState ScissorsState;

    [SerializeField] private float _breakBetweenRounds;
    [SerializeField] private float _timeForPoseChoosing;
    [SerializeField] private float _stateWaitingTime;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private HandPoseManager _handPoseManager;

    private IDisplayState _currentState;

    void Awake()
    {
        this.InitializeStates();
    }

    private void InitializeStates()
    {
        this.StartGameState = new StartGameState(this._handPoseManager);
        this.BreakState = new BreakState(this._handPoseManager, this._timeForPoseChoosing);
        this.WinState = new WinState(this._breakBetweenRounds);
        this.DrawState = new DrawState(this._breakBetweenRounds);
        this.LoseState = new LoseState(this._breakBetweenRounds);
        this.RockState = new RockState(this._handPoseManager);
        this.PaperState = new PaperState(this._handPoseManager);
        this.ScissorsState = new ScissorsState(this._handPoseManager);
        this._currentState = this.StartGameState;
        this._currentState.Enter(this);
    }

    void Update()
    {
        this._currentState.Update(this);
    }

    public void TransitionTo(IDisplayState nextState)
    {
        this._currentState.Exit(this);
        this._currentState = nextState;
        this._currentState.Enter(this);
    }

    public void SetText(string text)
    {
        this._text.text = text;
    }

    public void AddText(string text)
    {
        this._text.text += text;
    }
}
