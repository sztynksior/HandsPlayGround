using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NonKinematicRubicCube : MonoBehaviour
{
    private List<NonKinematicPiece> pieces = new List<NonKinematicPiece>();
    private float minimumRotationMagnitudeToDetermineRotationAxis = 0.12f;
    private float rotationSpeedMultipler = 20f;

    private void Awake()
    {
        this.pieces = new List<NonKinematicPiece>(GetComponentsInChildren<NonKinematicPiece>());
    }

    private Vector3 rotationAxis = Vector3.zero;
    private List<NonKinematicPiece> piecesToRotate = new List<NonKinematicPiece>();

    public void RotateWall(Vector3 pulledPieceLocalPosition, Vector3 pulledPiecePosition, Vector3 pullVector)
    {
        Vector3 relativePullVector = this.CalculateRelativePullVector(pullVector);
        Vector3 vectorFromPieceToCenter = this.CalculateVectorFromPieceToCenter(pulledPiecePosition);
        Debug.Log("RelativePullVector: " +  relativePullVector);
        Debug.Log("vectorFromPieceToCenter: " + vectorFromPieceToCenter);

        if (rotationAxis == Vector3.zero)
        {
            this.rotationAxis = this.DetermineRotationAxis(pullVector);
        }
        else
        {
            if (this.piecesToRotate.Count == 0) 
            {
                this.piecesToRotate = this.FindPiecesToRotate(pulledPieceLocalPosition, rotationAxis);
            }

            Vector3 rotationVector =  this.CalculateRotationVector(relativePullVector, this.rotationAxis);
            Vector3 radiusVector = this.CalculateRadiusVector(vectorFromPieceToCenter, this.rotationAxis);
            Vector3 crossProduc = Vector3.Cross(rotationVector, radiusVector.normalized);
            Debug.Log("rotationAxis: " + rotationAxis);
            Debug.Log("rotationVector: " + rotationVector);
            Debug.Log("radiusVector: " + radiusVector);
            Debug.Log("crossProduc: " + crossProduc);

            foreach(NonKinematicPiece piece in this.piecesToRotate) 
            {
                if (rotationAxis == new Vector3(1f, 0f, 0f))
                {
                    piece.transform.RotateAround(this.transform.position, this.transform.right, rotationSpeedMultipler * crossProduc.x);
                }
                else if (rotationAxis == new Vector3(0f, 1f, 0f))
                {
                    piece.transform.RotateAround(this.transform.position, this.transform.up, rotationSpeedMultipler * crossProduc.y);
                }
                else if (rotationAxis == new Vector3(0f, 0f, 1f))
                {
                    piece.transform.RotateAround(this.transform.position, this.transform.forward, rotationSpeedMultipler * crossProduc.z);
                }
            }
        }
    }

    private Vector3 CalculateRelativePullVector(Vector3 pullVector)
    {
        Vector3 relativePullVector = new Vector3(
            Vector3.Dot(pullVector, this.transform.right),
            Vector3.Dot(pullVector, this.transform.up),
            Vector3.Dot(pullVector, this.transform.forward));

        return relativePullVector;
    }

    private Vector3 CalculateVectorFromPieceToCenter(Vector3 pulledPiecePosition)
    {
        Vector3 vectorFromPieceToCenter = this.transform.position - pulledPiecePosition;

        return vectorFromPieceToCenter;
    }

    private Vector3 DetermineRotationAxis(Vector3 relativePullVector)
    {
        Vector3 rotationAxis = Vector3.zero;
        Vector3 rotationOnX = CalculateRotationVector(relativePullVector, new Vector3(1f, 0f, 0f));
        Vector3 rotationOnY = CalculateRotationVector(relativePullVector, new Vector3(0f, 1f, 0f));
        Vector3 rotationOnZ = CalculateRotationVector(relativePullVector, new Vector3(0f, 0f, 1f));
        float maximumRotationMagnitude = Mathf.Max(
            rotationOnX.magnitude,
            rotationOnY.magnitude,
            rotationOnZ.magnitude);

        if (maximumRotationMagnitude > this.minimumRotationMagnitudeToDetermineRotationAxis)
        {
            if (maximumRotationMagnitude == rotationOnX.magnitude)
            {
                rotationAxis = new Vector3(1f, 0f, 0f);
            }
            else if (maximumRotationMagnitude == rotationOnY.magnitude)
            {
                rotationAxis = new Vector3(0f, 1f, 0f);
            }
            else if (maximumRotationMagnitude == rotationOnZ.magnitude)
            {
                rotationAxis = new Vector3(0f, 0f, 1f);
            }
        }

        return rotationAxis;
    }

    private Vector3 CalculateRotationVector(Vector3 relativePullVector, Vector3 rotationAxis)
    {
        Vector3 rotationVector = Vector3.zero;

        if (rotationAxis == new Vector3(1f, 0f, 0f))
        {
            rotationVector = new Vector3(0f, relativePullVector.y, relativePullVector.z);
        }
        else if (rotationAxis == new Vector3(0f, 1f, 0f))
        {
            rotationVector = new Vector3(relativePullVector.x, 0f, relativePullVector.z);
        }
        else if (rotationAxis == new Vector3(0f, 0f, 1f))
        {
            rotationVector = new Vector3(relativePullVector.x, relativePullVector.y, 0f);
        }

        return rotationVector;
    }

    private Vector3 CalculateRadiusVector(Vector3 vectorFromPieceToCenter, Vector3 rotationAxis)
    {
        Vector3 radiusVector = Vector3.zero;

        if (rotationAxis == new Vector3(1f, 0f, 0f))
        {
            radiusVector = new Vector3(0f, vectorFromPieceToCenter.y, vectorFromPieceToCenter.z);
        }
        else if (rotationAxis == new Vector3(0f, 1f, 0f))
        {
            radiusVector = new Vector3(vectorFromPieceToCenter.x, 0f, vectorFromPieceToCenter.z);
        }
        else if (rotationAxis == new Vector3(0f, 0f, 1f))
        {
            radiusVector = new Vector3(vectorFromPieceToCenter.x, vectorFromPieceToCenter.y, 0f);
        }

        return radiusVector;
    }

    private List<NonKinematicPiece> FindPiecesToRotate(Vector3 pulledPieceLocalPosition, Vector3 rotationAxis)
    {
        List<NonKinematicPiece> piecesToRotate = new List<NonKinematicPiece>(); 
        Vector3 pulledPieceAxisPositionInCube = new Vector3(
            pulledPieceLocalPosition.x * rotationAxis.x,
            pulledPieceLocalPosition.y * rotationAxis.y,
            pulledPieceLocalPosition.z * rotationAxis.z);
        Vector3 piecePositionInCube;

        foreach (NonKinematicPiece piece in this.pieces)
        {
            piecePositionInCube = piece.transform.localPosition;
            piecePositionInCube = new Vector3(
                piecePositionInCube.x * rotationAxis.x,
                piecePositionInCube.y * rotationAxis.y,
                piecePositionInCube.z * rotationAxis.z);

            if (pulledPieceAxisPositionInCube == piecePositionInCube)
            {
                piecesToRotate.Add(piece);
            }
        }

        return piecesToRotate;
    }

    public void ResetWallRotation()
    {
        this.rotationAxis = Vector3.zero;
    }
}
