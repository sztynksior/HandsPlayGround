using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NonKinematicPiece : MonoBehaviour
{
    private InteractionBehaviour interactionBehaviour;
    private NonKinematicRubicCube rubicCube;

    private void Awake()
    {
        this.interactionBehaviour = GetComponent<InteractionBehaviour>();
        this.rubicCube = GetComponentInParent<NonKinematicRubicCube>();
    }

    private void Start()
    {
        this.interactionBehaviour.OnGraspStay += this.NotifyAboutGraspStay;
        this.interactionBehaviour.OnGraspEnd += this.NotifyAboutGraspStop;
    }

    private void OnDestroy()
    {
        this.interactionBehaviour.OnGraspStay -= this.NotifyAboutGraspStay;
        this.interactionBehaviour.OnGraspEnd -= this.NotifyAboutGraspStop;
    }

    private void NotifyAboutGraspStay()
    {
        this.rubicCube.OnPieceGrasp(this.transform.localPosition, this.interactionBehaviour.GetGraspPoint(this.interactionBehaviour.graspingController));
    }

    private void NotifyAboutGraspStop()
    {
        this.rubicCube.ResetWallRotation();
    }
}
