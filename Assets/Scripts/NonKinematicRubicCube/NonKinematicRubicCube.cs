using Leap.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NonKinematicRubicCube : MonoBehaviour
{
    private float minimalProjectionMagnitudeToDetermineRotationAxis = 1.4f;
    private float rotationSpeedMultipler = 2f;
    private float aligningSpeed = 5f;
    private float epsilonForDeterminingIfPointIsLyingOnPlane = 0.001f;

    private int numberOfWalls = 4;
    private List<float> alignmentAngles = new List<float>();
    private List<Vector3> rotationAxes = new List<Vector3>();

    private List<NonKinematicPiece> pieces = new List<NonKinematicPiece>();
    private List<NonKinematicPiece> graspedPieces = new List<NonKinematicPiece>();
    private SegmentRotator segmentRotator;
    private NonKinematicPiece firstGraspedPiece;
    private NonKinematicPiece secondGraspedPiece;

    #region Awake

    private void Awake()
    {
        this.pieces = new List<NonKinematicPiece>(GetComponentsInChildren<NonKinematicPiece>());
        this.DetermineAlignmentAngles();
        this.DetermineRotationAxis();
        this.segmentRotator = new SegmentRotator(this.alignmentAngles, this.transform, this.rotationSpeedMultipler);
    }

    private void DetermineAlignmentAngles()
    {
        float aligmentAnglesInterval = 360f / this.numberOfWalls;

        for (int i = 0; i < numberOfWalls; i++)
        {
            this.alignmentAngles.Add(aligmentAnglesInterval * i);
        }
    }

    private void DetermineRotationAxis()
    {
        this.rotationAxes.Add(Vector3.right);
        this.rotationAxes.Add(Vector3.up);
        this.rotationAxes.Add(Vector3.forward);
    }

    #endregion

    private void Start()
    {
        foreach(NonKinematicPiece piece in this.pieces) 
        {
            piece.OnGraspBegin += this.DetermineGraspedPiece;
            piece.OnGraspEnd += this.RemoveGraspedPiece;
        }
    }

    private void FixedUpdate()
    {
        if(this.firstGraspedPiece != null)
        {
            if(!this.segmentRotator.SegmentIsRotating()) 
            {
                if (this.secondGraspedPiece != null)
                {
                    this.RotateCube();
                }
                else
                {
                    this.DetermineRotationSegment();
                }
            }
            else
            {
                Vector3 pullVector = this.CalculateLocalPullVector();
                this.segmentRotator.RotateSegmentDependingOnPull(this.firstGraspedPiece.transform.localPosition, pullVector);
            }
        }
        else if (this.segmentRotator.SegmentIsRotating())
        {
            this.segmentRotator.AlignRotatedSegment(this.aligningSpeed);
        }
    }

    private void OnDestroy()
    {
        foreach (NonKinematicPiece piece in this.pieces)
        {
            piece.OnGraspBegin -= this.DetermineGraspedPiece;
            piece.OnGraspEnd -= this.RemoveGraspedPiece;
        }
    }

    private void DetermineGraspedPiece(NonKinematicPiece piece)
    {
        if (!this.segmentRotator.SegmentIsRotating())
        {
            if (this.firstGraspedPiece == null)
            {
                this.firstGraspedPiece = piece;
            }
            else if (this.secondGraspedPiece == null)
            {
                this.secondGraspedPiece = piece;
            }
        }
    }

    private void RemoveGraspedPiece(NonKinematicPiece piece)
    {
        if (this.firstGraspedPiece == piece)
        {
            this.firstGraspedPiece = null;
        }
        else if (this.secondGraspedPiece == piece)
        {
            this.secondGraspedPiece = null;
        }

        this.crossVectorFromPreviousFrame = Vector3.zero;
    }

    Vector3 crossVectorFromPreviousFrame = Vector3.zero;

    private void RotateCube()
    {
        Vector3 corssVector = Vector3.Cross(this.firstGraspedPiece.GraspPoint - this.transform.position, this.secondGraspedPiece.GraspPoint - this.transform.position).normalized;
        Quaternion rotationDifference = Quaternion.FromToRotation(this.crossVectorFromPreviousFrame, corssVector);
        this.transform.rotation = rotationDifference * transform.rotation;
        this.crossVectorFromPreviousFrame = corssVector;
    }

    private Vector3 CalculateLocalPullVector()
    {
        Vector3 localGraspPosition = transform.InverseTransformPoint(this.firstGraspedPiece.GraspPoint);
        Vector3 pullVector = localGraspPosition - this.firstGraspedPiece.transform.localPosition;

        return pullVector;
    }


    private void DetermineRotationSegment()
    {
        Vector3 rotationAxis = this.DetermineRotationAxisForSegment();
        
        if (rotationAxis != Vector3.zero)
        {
            List<Transform> piecesToRotate = this.FindPiecesToRotate(rotationAxis);

            this.segmentRotator.SetRotatingSegment(piecesToRotate, rotationAxis);
        }

    }

    private Vector3 DetermineRotationAxisForSegment()
    {
        Vector3 pullVector = this.CalculateLocalPullVector();
        Vector3 rotationAxis = Vector3.zero;
        List<Vector3> projections = new List<Vector3>();

        foreach (Vector3 axis in this.rotationAxes)
        {
            projections.Add(Vector3.ProjectOnPlane(pullVector, axis));
        }

        int largestProjectionIndex = 0;

        for (int i = 0; i < projections.Count; i++)
        {
            if (projections[i].magnitude > projections[largestProjectionIndex].magnitude)
            {
                largestProjectionIndex = i;
            }
        }

        if (projections[largestProjectionIndex].magnitude > this.minimalProjectionMagnitudeToDetermineRotationAxis)
        {
            rotationAxis = this.rotationAxes[largestProjectionIndex];
        }

        return rotationAxis;
    }

    private List<Transform> FindPiecesToRotate(Vector3 rotationPlaneNormal)
    {
        List<Transform> piecesToRotate = new List<Transform>();

        foreach (NonKinematicPiece piece in this.pieces)
        {
            if (this.IsPointLyingOnPlane(piece.transform.localPosition, this.firstGraspedPiece.transform.localPosition, rotationPlaneNormal))
            {
                piecesToRotate.Add(piece.transform);
            }
        }

        return piecesToRotate;
    }

    private bool IsPointLyingOnPlane(Vector3 anyPoint, Vector3 pointOfPlane, Vector3 planeNormal)
    {
        bool isLying = false;
        float pointNormalEquationResult = Vector3.Dot(planeNormal, anyPoint - pointOfPlane);

        if (Mathf.Abs(0f - pointNormalEquationResult) < this.epsilonForDeterminingIfPointIsLyingOnPlane)
        {
            isLying = true;
        }

        return isLying;
    }
}