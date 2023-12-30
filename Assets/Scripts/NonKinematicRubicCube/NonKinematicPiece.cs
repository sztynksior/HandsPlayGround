using Leap.Unity.Interaction;
using System;
using UnityEngine;

public class NonKinematicPiece : MonoBehaviour
{
    public event Action<NonKinematicPiece> OnGraspBegin;
    public event Action<Vector3, Vector3> OnGraspStay;
    public event Action<NonKinematicPiece> OnGraspEnd;

    private InteractionBehaviour interactionBehaviour;
    private Vector3 graspPoint;
    private Vector3 startingPosition;
    private Quaternion StartingRotation;

    public Vector3 GraspPoint 
    {
        get { return graspPoint; }  
    }

    private void Awake()
    {
        this.interactionBehaviour = GetComponent<InteractionBehaviour>();
        this.graspPoint = transform.position;
        this.startingPosition = transform.localPosition;
        this.StartingRotation = transform.localRotation;
    }

    private void Start()
    {
        this.interactionBehaviour.OnGraspBegin += this.InvokeGraspBegin;
        this.interactionBehaviour.OnGraspStay += this.DetermineGraspingPoint;
        this.interactionBehaviour.OnGraspEnd += this.InvokeGraspEnd;
    }

    private void OnDestroy()
    {
        this.interactionBehaviour.OnGraspBegin -= this.InvokeGraspBegin;
        this.interactionBehaviour.OnGraspStay -= this.DetermineGraspingPoint;
        this.interactionBehaviour.OnGraspEnd -= this.InvokeGraspEnd;
    }

    private void InvokeGraspBegin()
    {
        this.graspPoint = this.interactionBehaviour.GetGraspPoint(this.interactionBehaviour.graspingController);
        this.OnGraspBegin?.Invoke(this);
    }

    private void DetermineGraspingPoint()
    {
        this.graspPoint = this.interactionBehaviour.GetGraspPoint(this.interactionBehaviour.graspingController);
    }

    private void InvokeGraspEnd()
    {
        this.graspPoint = transform.position;
        this.OnGraspEnd?.Invoke(this);
    }

    public void ResetPiecePositionAndRotation()
    {
        this.transform.SetLocalPositionAndRotation(this.startingPosition, this.StartingRotation);
    }
}
