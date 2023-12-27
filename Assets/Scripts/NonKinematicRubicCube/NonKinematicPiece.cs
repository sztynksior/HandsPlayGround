using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        this.interactionBehaviour.OnGraspStay += this.CalculateRotation;
        this.interactionBehaviour.OnGraspEnd += this.NotifyAboutGraspStop;
    }

    private void OnDestroy()
    {
        this.interactionBehaviour.OnGraspStay -= this.CalculateRotation;
        this.interactionBehaviour.OnGraspEnd -= this.NotifyAboutGraspStop;
    }

    private void CalculateRotation()
    {
        this.rubicCube.RotateWall(this.transform.localPosition, this.transform.position, this.interactionBehaviour.GetGraspPoint(this.interactionBehaviour.graspingController) - this.transform.position);
    }

    private void NotifyAboutGraspStop()
    {
        this.rubicCube.ResetWallRotation();
    }
}
