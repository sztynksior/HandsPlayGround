using Leap.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NonKinematicRubicCube : MonoBehaviour
{
    private List<NonKinematicPiece> pieces = new List<NonKinematicPiece>();
    private float minimalProjectionMagnitudeToDetermineRotationAxis = 2f;
    private float rotationSpeedMultipler = 2f;
    private float aligningSpeed = 5f;
    private float epsilonForDeterminingIfPointIsLyingOnPlane = 0.0001f;
    private int numberOfWalls = 4;
    private List<float> alignmentAngles = new List<float>();
    private List<Vector3> rotationAxes = new List<Vector3>();

    #region Awake

    private void Awake()
    {
        this.pieces = new List<NonKinematicPiece>(GetComponentsInChildren<NonKinematicPiece>());
        this.DetermineAlignmentAngles();
        this.DetermineRotationAxis();

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

    private void FixedUpdate()
    {
        if (this.aligningOn)
        {
            this.AlignRotatedSegment();
        }
    }

    private int numberOfGrasps = 0;

    public void OnPieceGrasp(Vector3 piecePosition, Vector3 graspPosition)
    {
        Vector3 localGraspPosition = transform.InverseTransformPoint(graspPosition);

        if (numberOfGrasps < 1 && !aligningOn) 
        {
            Vector3 pullVector = localGraspPosition - piecePosition;

            if (!this.RotationAxisDetermined())
            {
                this.DetermineRotationAxis(pullVector);
            }
            else
            {
                if (this.rotatingSegment.Count == 0)
                {
                    this.FindPiecesOfRotatingSegment(piecePosition, this.currentRotaionAxis);
                }

                Vector3 vectorFromPieceToCenter = this.transform.localPosition - piecePosition;
                Vector3 rotationVector = Vector3.ProjectOnPlane(pullVector, this.currentRotaionAxis);
                Vector3 radiusVector = Vector3.ProjectOnPlane(vectorFromPieceToCenter, this.currentRotaionAxis);
                float rotationSpeed = this.CalculateRotationSpeed(rotationVector, radiusVector, this.currentRotaionAxis);
                this.RotateSegment(rotationSpeed);
            }
        }
    }

    private float CalculateRotationSpeed(Vector3 rotationVector, Vector3 radiusVector, Vector3 rotationPlaneNormal)
    {
        float rotationSpeed = rotationVector.magnitude * Mathf.Sin(Vector3.SignedAngle(rotationVector, radiusVector, rotationPlaneNormal) * Mathf.Deg2Rad) * this.rotationSpeedMultipler;

        return rotationSpeed;
    }

    #region Current Rotation Axis

    private Vector3 currentRotaionAxis = Vector3.zero;

    private bool RotationAxisDetermined()
    {
        bool isDetermined = false;

        if (this.currentRotaionAxis != Vector3.zero)
        {
            isDetermined = true;
        }

        return isDetermined;
    }

    private void DetermineRotationAxis(Vector3 pullVector)
    {
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
            this.currentRotaionAxis = this.rotationAxes[largestProjectionIndex];
        }
    }

    #endregion

    #region Managing Rotating Segment

    private List<NonKinematicPiece> rotatingSegment = new List<NonKinematicPiece>();
    private float rotationOfSegment = 0f;
    private bool aligningOn = false;

    private void FindPiecesOfRotatingSegment(Vector3 pulledPiecePosition, Vector3 rotationPlaneNormal)
    {
        foreach (NonKinematicPiece piece in this.pieces)
        {
            if (this.IsPointLyingOnPlane(piece.transform.localPosition, pulledPiecePosition, rotationPlaneNormal))
            {
                this.rotatingSegment.Add(piece);
            }
        }
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

    private void RotateSegment(float rotationSpeed)
    {
        foreach (NonKinematicPiece piece in this.rotatingSegment)
        {
            piece.transform.RotateAround(this.transform.position, transform.TransformDirection(this.currentRotaionAxis), rotationSpeed);
        }

        this.rotationOfSegment = (this.rotationOfSegment + rotationSpeed) - 360f * Mathf.Floor((this.rotationOfSegment + rotationSpeed) / 360f);
        Debug.Log("rotation of segment" + this.rotationOfSegment);
    }

    private void AlignRotatedSegment()
    {
        float differenceToAlignmentAngle = this.CalculateDifferenceToNearestAlignmentAngle();
        if (differenceToAlignmentAngle != 0)
        { 
            if (Mathf.Abs(differenceToAlignmentAngle) > this.aligningSpeed)
            {
                this.RotateSegment(Mathf.Sign(differenceToAlignmentAngle) * this.aligningSpeed);
            }
            else
            {
                this.RotateSegment(differenceToAlignmentAngle);
            }
        }
        else
        {
            this.ResetSegment();
        }
    }

    private float CalculateDifferenceToNearestAlignmentAngle()
    {
        float smallestDistance = 180f;
        float smallestDifference = 0f;

        foreach (float angle in this.alignmentAngles)
        {
            float difference = angle - this.rotationOfSegment;

            if (difference < -180f)
            {
                difference += 360f;
            }
            else if (difference > 180f)
            {
                difference -= 360f;
            }

            float distanse = Mathf.Abs(difference);

            if (distanse < smallestDistance)
            {
                smallestDistance = distanse;
                smallestDifference = difference;
            }
        }

        return smallestDifference;
    }

    private void ResetSegment()
    {
        this.aligningOn = false;
        this.rotatingSegment.Clear();
        this.currentRotaionAxis = Vector3.zero;
        this.rotationOfSegment = 0f;
    }

    #endregion

    public void ResetWallRotation()
    {
        this.aligningOn = true;
        this.numberOfGrasps -= 1;
    }
}


