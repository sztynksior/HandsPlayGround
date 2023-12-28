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
    private float minimalProjectionMagnitudeToDetermineRotationAxis = 0.12f;
    private float rotationSpeedMultipler = 20f;
    private float aligningSpeed = 5f;
    private float epsilonForDeterminingIfPointIsLyingOnPlane = 0.0001f;
    private int numberOfWalls = 4;
    private List<float> alignmentAngles = new List<float>();

    private void Awake()
    {
        this.pieces = new List<NonKinematicPiece>(GetComponentsInChildren<NonKinematicPiece>());
        this.alignmentAngles = this.DetermineAlignmentAngles();
    }

    private void FixedUpdate()
    {
        if (this.aligningOn)
        {
            this.AlignSegment();
        }
    }

    private List<float> DetermineAlignmentAngles()
    {
        List<float> aligningAngles = new List<float>();
        float aligmentAnglesInterval = 360f / this.numberOfWalls;

        for (int i = 0; i < numberOfWalls; i++)
        {
            aligningAngles.Add(aligmentAnglesInterval * i);
        }

        return aligningAngles;
    }

    private Vector3 rotationAxis = Vector3.zero;
    private List<NonKinematicPiece> rotatingSegment = new List<NonKinematicPiece>();

    public void RotateSegment(Vector3 pulledPiecePosition, Vector3 graspPosition)
    {
        Vector3 pullVector = graspPosition - pulledPiecePosition;

        if (rotationAxis == Vector3.zero)
        {
            this.rotationAxis = this.DetermineRotationAxis(pullVector);
        }
        else
        {
            Vector3 rotationPlaneNormal = this.GetRotationPlaneNormal();

            if (this.rotatingSegment.Count == 0)
            {
                this.rotatingSegment = this.FindPiecesToRotate(pulledPiecePosition, rotationPlaneNormal);
            }

            Vector3 vectorFromPieceToCenter = this.transform.position - pulledPiecePosition;
            Vector3 rotationVector = Vector3.ProjectOnPlane(pullVector, rotationPlaneNormal);
            Vector3 radiusVector = Vector3.ProjectOnPlane(vectorFromPieceToCenter, rotationPlaneNormal);
            float rotationSpeed = this.CalculateRotationSpeed(rotationVector, radiusVector, rotationPlaneNormal);

            foreach(NonKinematicPiece piece in this.rotatingSegment) 
            {
                piece.transform.RotateAround(this.transform.position, rotationPlaneNormal, rotationSpeed);
            }
        }
    }

    private Vector3 DetermineRotationAxis(Vector3 pullVector)
    {
        Vector3 rotationAxis = Vector3.zero;
        Vector3 projectionOnYZ = Vector3.ProjectOnPlane(pullVector, this.transform.right);
        Vector3 projectionOnXZ = Vector3.ProjectOnPlane(pullVector, this.transform.up);
        Vector3 projectionOnXY = Vector3.ProjectOnPlane(pullVector, this.transform.forward);
        float maximumRotationMagnitude = Mathf.Max(
            projectionOnYZ.magnitude,
            projectionOnXZ.magnitude,
            projectionOnXY.magnitude);

        if (maximumRotationMagnitude > this.minimalProjectionMagnitudeToDetermineRotationAxis)
        {
            if (maximumRotationMagnitude == projectionOnYZ.magnitude)
            {
                rotationAxis = new Vector3(1f, 0f, 0f);
            }
            else if (maximumRotationMagnitude == projectionOnXZ.magnitude)
            {
                rotationAxis = new Vector3(0f, 1f, 0f);
            }
            else if (maximumRotationMagnitude == projectionOnXY.magnitude)
            {
                rotationAxis = new Vector3(0f, 0f, 1f);
            }
        }

        return rotationAxis;
    }

    private Vector3 GetRotationPlaneNormal()
    {
        Vector3 rotationPlaneNormal = Vector3.zero;

        if (this.rotationAxis == new Vector3(1f, 0f, 0f))
        {
            rotationPlaneNormal = this.transform.right;
        }
        else if (this.rotationAxis == new Vector3(0f, 1f, 0f))
        {
            rotationPlaneNormal = this.transform.up;
        }
        else if (this.rotationAxis == new Vector3(0f, 0f, 1f))
        {
            rotationPlaneNormal = this.transform.forward;
        }

        return rotationPlaneNormal;
    }

    private float CalculateRotationSpeed(Vector3 rotationVector, Vector3 radiusVector, Vector3 rotationPlaneNormal)
    {
        float rotationSpeed = rotationVector.magnitude * Mathf.Sin(Vector3.SignedAngle(rotationVector, radiusVector, rotationPlaneNormal) * Mathf.Deg2Rad) * this.rotationSpeedMultipler;

        return rotationSpeed;
    }

    private List<NonKinematicPiece> FindPiecesToRotate(Vector3 pulledPiecePosition, Vector3 rotationPlaneNormal)
    {
        List<NonKinematicPiece> piecesToRotate = new List<NonKinematicPiece>();

        foreach (NonKinematicPiece piece in this.pieces)
        {
            if (this.IsPointLyingOnPlane(piece.transform.position, pulledPiecePosition, rotationPlaneNormal))
            {
                piecesToRotate.Add(piece);
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

    public void ResetWallRotation()
    {
        this.aligningOn = true;
    }

    private bool aligningOn = false;
    private bool piecesAligned = false;
    private void AlignSegment()
    {
        if(!piecesAligned)
        {
            Vector3 rotationPlaneNormal = this.GetRotationPlaneNormal();
            Vector3 rotationZeroVector = Vector3.ProjectOnPlane(this.transform.right + this.transform.up + this.transform.forward, rotationPlaneNormal);

            foreach (NonKinematicPiece piece in this.rotatingSegment)
            {
                Vector3 rotationVectorOfPiece = Vector3.ProjectOnPlane(piece.transform.right + piece.transform.up + piece.transform.forward, rotationPlaneNormal);
                float differenceToAlignmentAngle = this.CalculateDifferenceToAlignmentAngle(rotationVectorOfPiece, rotationZeroVector, rotationPlaneNormal);
                if (Mathf.Abs(differenceToAlignmentAngle) > this.aligningSpeed)
                {
                    piece.transform.RotateAround(this.transform.position, rotationPlaneNormal, Mathf.Sign(differenceToAlignmentAngle) * this.aligningSpeed);
                }
                else
                {
                    piece.transform.RotateAround(this.transform.position, rotationPlaneNormal, differenceToAlignmentAngle);
                    this.piecesAligned = true;                
                }
            }
        }
        else
        {
            this.aligningOn = false;
            this.rotatingSegment.Clear();
            this.rotationAxis = Vector3.zero;
            this.piecesAligned = false;
        }

    }

    private float CalculateDifferenceToAlignmentAngle(Vector3 rotationVector, Vector3 rotationZeroVector, Vector3 rotationPlaneNormal)
    {
        float rotationAngle = Vector3.SignedAngle(rotationZeroVector, rotationVector, rotationPlaneNormal);
        float normalizedRotationAngle = 360f + rotationAngle - 360f * MathF.Floor((360f + rotationAngle) / 360f);
        float smallestDistance = 180f;
        float smallestDifference = 0f;

        foreach (float angle in this.alignmentAngles)
        {
            float difference = angle - normalizedRotationAngle;

            if(difference < -180f)
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
}
