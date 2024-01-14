using System.Collections.Generic;
using UnityEngine;

public class SegmentRotator
{
    private Transform cubeTranform;
    private List<float> alignmentAngles;
    private float rotationSpeedMultipler;

    private List<Transform> segmentPieces;
    private Vector3 rotationAxis;
    private float rotationAngle;

    public SegmentRotator(List<float> alignAngles, Transform cubeTranform, float rotationSpeedMultipler)
    {
        this.alignmentAngles = alignAngles;
        this.segmentPieces = new List<Transform>();
        this.rotationAngle = 0;
        this.cubeTranform = cubeTranform;
        this.rotationAxis = Vector3.zero;
        this.rotationSpeedMultipler = rotationSpeedMultipler;
    }

    public bool SegmentIsRotating()
    {
        bool isRotating = false;
        
        if(this.segmentPieces.Count > 0)
        {
            isRotating = true;
        }

        return isRotating;
    }

    public void SetRotatingSegment(List<Transform> pieces, Vector3 rotationAxis)
    {
        this.segmentPieces = new List<Transform>(pieces);
        this.rotationAxis = rotationAxis;
    }

    public void RotateSegment(float rotationSpeed)
    {
        if (this.segmentPieces.Count != 0 && rotationAxis != Vector3.zero)
        {
            foreach (Transform piece in this.segmentPieces)
            {
                piece.RotateAround(this.cubeTranform.position, this.cubeTranform.TransformDirection(this.rotationAxis), rotationSpeed);
            }

            this.rotationAngle = (this.rotationAngle + rotationSpeed) - 360f * Mathf.Floor((this.rotationAngle + rotationSpeed) / 360f);
        }
    }
    public void RotateSegmentDependingOnPull(Vector3 pulledPiecePosition, Vector3 pullVector)
    {
        Vector3 vectorFromPieceToCenter = this.cubeTranform.localPosition - pulledPiecePosition;
        Vector3 rotationVector = Vector3.ProjectOnPlane(pullVector, this.rotationAxis);
        Vector3 radiusVector = Vector3.ProjectOnPlane(vectorFromPieceToCenter, this.rotationAxis);
        float rotationSpeed = rotationVector.magnitude * Mathf.Sin(Vector3.SignedAngle(rotationVector, radiusVector, this.rotationAxis) * Mathf.Deg2Rad) * this.rotationSpeedMultipler;

        this.RotateSegment(rotationSpeed);
    }

    public void RotateSegmetnByAngle(float rotationSpeed, float angle)
    {
        float differenceToAngle = this.CalculateSmalerDifference(angle, this.rotationAngle);
        if (differenceToAngle != 0)
        {
            if (Mathf.Abs(differenceToAngle) > Mathf.Abs(rotationSpeed))
            {
                this.RotateSegment(Mathf.Sign(differenceToAngle) * rotationSpeed);
            }
            else
            {
                this.RotateSegment(differenceToAngle);
            }
        }
        else
        {
            this.ResetSegment();
        }
    }

    public void AlignRotatedSegment(float alignigSpeed)
    {
        float differenceToAlignmentAngle = this.CalculateDifferenceToNearestAlignmentAngle();
        if (differenceToAlignmentAngle != 0)
        {
            if (Mathf.Abs(differenceToAlignmentAngle) > Mathf.Abs(alignigSpeed))
            {
                this.RotateSegment(Mathf.Sign(differenceToAlignmentAngle) * alignigSpeed);
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
            float difference = this.CalculateSmalerDifference(angle, this.rotationAngle);

            float distanse = Mathf.Abs(difference);

            if (distanse < smallestDistance)
            {
                smallestDistance = distanse;
                smallestDifference = difference;
            }
        }

        return smallestDifference;
    }

    private float CalculateSmalerDifference(float firstAngle, float secondAngle)
    {
        float difference = firstAngle - secondAngle;

        if (difference < -180f)
        {
            difference += 360f;
        }
        else if (difference > 180f)
        {
            difference -= 360f;
        }

        return difference;
    }

    public void ResetSegment()
    {
        this.segmentPieces.Clear();
        this.rotationAxis = Vector3.zero;
        this.rotationAngle = 0f;
    }
}
