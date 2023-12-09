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
                    this._dipsplay.SetText("Kamieñ\n\nRemis");
                }
                else if (computerHandPose == HandPose.OpenPalm)
                {
                    this._dipsplay.SetText("Papier\n\nPrzegra³eœ");
                }
                else if (computerHandPose == HandPose.Scissors)
                {
                    this._dipsplay.SetText("No¿yce\n\nWygra³eœ");
                }
            }
            else if (playerHandPose == HandPose.OpenPalm)
            {
                if (computerHandPose == HandPose.Fist)
                {
                    this._dipsplay.SetText("Kamieñ\n\nWygra³eœ");
                }
                else if (computerHandPose == HandPose.OpenPalm)
                {
                    this._dipsplay.SetText("Papier\n\nRemis");
                }
                else if (computerHandPose == HandPose.Scissors)
                {
                    this._dipsplay.SetText("No¿yce\n\nPrzegra³eœ");
                }
            }
            else if (playerHandPose == HandPose.Scissors)
            {
                if (computerHandPose == HandPose.Fist)
                {
                    this._dipsplay.SetText("Kamieñ\n\nPrzegra³eœ");
                }
                else if (computerHandPose == HandPose.OpenPalm)
                {
                    this._dipsplay.SetText("Papier\n\nWygra³eœ");
                }
                else if (computerHandPose == HandPose.Scissors)
                {
                    this._dipsplay.SetText("No¿yce\n\nRemis");
                }
            }

            stateManager.TransitionTo(stateManager.BreakeState);
            return;
        }
        else
        {
            playerHandPose = this._handPoseManager.GetPose();
        }

        this._dipsplay.SetText("Kamieñ Papier No¿yce");
    }

    public void Exit(GameStateManager stateManager)
    {

    }
}
