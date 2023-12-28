using Leap.Unity;
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
    private float epsilonForDeterminingIfPointIsLyingOnPlane = 0.000001f;

    private void Awake()
    {
        this.pieces = new List<NonKinematicPiece>(GetComponentsInChildren<NonKinematicPiece>());
    }

    private Vector3 rotationAxis = Vector3.zero;
    private List<NonKinematicPiece> piecesToRotate = new List<NonKinematicPiece>();

    public void RotateWall(Vector3 pulledPiecePosition, Vector3 graspPosition)
    {
        Vector3 pullVector = graspPosition - pulledPiecePosition;

        if (rotationAxis == Vector3.zero)
        {
            this.rotationAxis = this.DetermineRotationAxis(pullVector);
        }
        else
        {
            Vector3 rotationPlaneNormal = this.GetRotationPlaneNormal();

            if (this.piecesToRotate.Count == 0) 
            {
                this.piecesToRotate = this.FindPiecesToRotate(pulledPiecePosition, rotationPlaneNormal);
            }

            Vector3 vectorFromPieceToCenter = this.transform.position - pulledPiecePosition;
            Vector3 rotationVector = Vector3.ProjectOnPlane(pullVector, rotationPlaneNormal);
            Vector3 radiusVector = Vector3.ProjectOnPlane(vectorFromPieceToCenter, rotationPlaneNormal);
            float rotationSpeed = this.CalculateRotationSpeed(rotationVector, radiusVector, rotationPlaneNormal);

            foreach(NonKinematicPiece piece in this.piecesToRotate) 
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
        this.rotationAxis = Vector3.zero;
        this.piecesToRotate.Clear();
    }
}
