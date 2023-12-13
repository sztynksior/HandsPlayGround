using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolveGameState : IGameState
{
    private HandPoseManager _handPoseManager;
    private Display _dipsplay;

    public ResolveGameState(HandPoseManager handPoseManager, Display display)
    {
        this._handPoseManager = handPoseManager;
        this._dipsplay = display;
    }

    public void Enter(GameStateManager stateManager)
    {

    }

    public void Update(GameStateManager stateManager)
    {
        HandPose playerHandPose = this._handPoseManager.GetPose();
        HandPose computerHandPose;

        if (playerHandPose == HandPose.Fist || playerHandPose == HandPose.OpenPalm || playerHandPose == HandPose.Scissors)
        {
            computerHandPose = HandPoseManager.GetRandomPoseFromList(new List<HandPose>() { HandPose.Fist, HandPose.OpenPalm, HandPose.Scissors });

            if (playerHandPose == HandPose.Fist)
            {
                if (computerHandPose == HandPose.Fist)
                {
                    this._dipsplay.DisplayRock();
                    this._dipsplay.SetOutcomeDisplayText("REMIS");
                }
                else if (computerHandPose == HandPose.OpenPalm)
                {
                    this._dipsplay.DisplayPaper();
                    this._dipsplay.SetOutcomeDisplayText("PRZEGRA£EŒ");
                }
                else if (computerHandPose == HandPose.Scissors)
                {
                    this._dipsplay.DisplayScissors();
                    this._dipsplay.SetOutcomeDisplayText("WYGRA£EŒ");
                }
            }
            else if (playerHandPose == HandPose.OpenPalm)
            {
                if (computerHandPose == HandPose.Fist)
                {
                    this._dipsplay.DisplayRock();
                    this._dipsplay.SetOutcomeDisplayText("WYGRA£EŒ");
                }
                else if (computerHandPose == HandPose.OpenPalm)
                {
                    this._dipsplay.DisplayPaper();
                    this._dipsplay.SetOutcomeDisplayText("REMIS");
                }
                else if (computerHandPose == HandPose.Scissors)
                {
                    this._dipsplay.DisplayScissors();
                    this._dipsplay.SetOutcomeDisplayText("PRZEGRA£EŒ");
                }
            }
            else if (playerHandPose == HandPose.Scissors)
            {
                if (computerHandPose == HandPose.Fist)
                {
                    this._dipsplay.DisplayRock();
                    this._dipsplay.SetOutcomeDisplayText("PRZEGRA£EŒ");
                }
                else if (computerHandPose == HandPose.OpenPalm)
                {
                    this._dipsplay.DisplayPaper();
                    this._dipsplay.SetOutcomeDisplayText("WYGRA£EŒ");
                }
                else if (computerHandPose == HandPose.Scissors)
                {
                    this._dipsplay.DisplayScissors();
                    this._dipsplay.SetOutcomeDisplayText("REMIS");
                }
            }
            this._dipsplay.SetMainDisplayText("");
            this._dipsplay.TurnOnFigureDisplay();
            stateManager.TransitionTo(stateManager.BreakeState);
            return;
        }
        else
        {
            playerHandPose = this._handPoseManager.GetPose();
        }

        this._dipsplay.SetMainDisplayText("Kamieñ Papier No¿yce");
    }

    public void Exit(GameStateManager stateManager)
    {

    }
}
