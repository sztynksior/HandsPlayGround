using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

public class RubicCube : MonoBehaviour
{
    [SerializeField] private float minimalProjectionMagnitudeToDetermineRotationAxis = 1.4f;
    [SerializeField] private float rotationSpeedMultipler = 2f;
    [SerializeField] private float aligningSpeed = 5f;
    [SerializeField] private float shufflingSpeed = 10f;
    [SerializeField] private float epsilonForDeterminingIfPointIsLyingOnPlane = 0.001f;

    private int numberOfWallsInSegment = 4;
    private List<Vector3> rotationAxes = new List<Vector3>();

    private List<CubePiece> pieces = new List<CubePiece>();
    private SegmentRotator segmentRotator;
    private CubePiece firstGraspedPiece;
    private CubePiece secondGraspedPiece;

    #region Awake

    private void Awake()
    {
        this.pieces = new List<CubePiece>(GetComponentsInChildren<CubePiece>());
        this.DetermineAlignmentAngles();
        this.DetermineRotationAxis();
        List<float> alignmentAngles = this.DetermineAlignmentAngles();
        this.segmentRotator = new SegmentRotator(alignmentAngles, this.transform, this.rotationSpeedMultipler);
    }

    private List<float> DetermineAlignmentAngles()
    {
        List<float> alignmentAngles = new List<float>();
        float aligmentAnglesInterval = 360f / this.numberOfWallsInSegment;

        for (int i = 0; i < numberOfWallsInSegment; i++)
        {
            alignmentAngles.Add(aligmentAnglesInterval * i);
        }

        return alignmentAngles;
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
        foreach(CubePiece piece in this.pieces) 
        {
            piece.OnGraspBegin += this.DetermineGraspedPiece;
            piece.OnGraspEnd += this.RemoveGraspedPiece;
        }
    }

    private void FixedUpdate()
    {
        if(this.ShufflingOn)
        {
            this.Shuffle();

            if(this.firstGraspedPiece != null || this.secondGraspedPiece != null)
            {
                this.RotateCube();
            }
        }

        if(this.firstGraspedPiece != null)
        {
            if(!this.segmentRotator.SegmentIsRotating()) 
            {
                if (this.secondGraspedPiece != null)
                {
                    this.RotateCube();
                }
                else if (!this.ShufflingOn)
                {
                    this.DetermineRotationSegment();
                }
            }
            else if (!this.ShufflingOn)
            {
                Vector3 pullVector = this.CalculateLocalPullVector();
                this.segmentRotator.RotateSegmentDependingOnPull(this.firstGraspedPiece.transform.localPosition, pullVector);
            }
        }
        else if (this.segmentRotator.SegmentIsRotating() && !this.ShufflingOn)
        {
            this.segmentRotator.AlignRotatedSegment(this.aligningSpeed);
        }
    }

    private void OnDestroy()
    {
        foreach (CubePiece piece in this.pieces)
        {
            piece.OnGraspBegin -= this.DetermineGraspedPiece;
            piece.OnGraspEnd -= this.RemoveGraspedPiece;
        }
    }

    private void DetermineGraspedPiece(CubePiece piece)
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

    private void RemoveGraspedPiece(CubePiece piece)
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
            List<Transform> piecesToRotate = this.FindPiecesOnSameAxis(this.firstGraspedPiece.transform.localPosition, rotationAxis);

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

    private List<float> shuffleAngles = new List<float>() { -180f, -90f, 90f, 180 };
    private int currentAngleIndex = 0;

    private void Shuffle()
    {
        if(!this.segmentRotator.SegmentIsRotating())
        {
            currentAngleIndex = Random.Range(0, this.shuffleAngles.Count);
            int randomPieceIndex = Random.Range(0, this.pieces.Count);
            int randomAxisIndexs = Random.Range(0, this.rotationAxes.Count);
            List<Transform> piecesToRotate = this.FindPiecesOnSameAxis(this.pieces[randomPieceIndex].transform.localPosition, this.rotationAxes[randomAxisIndexs]);
            this.segmentRotator.SetRotatingSegment(piecesToRotate, this.rotationAxes[randomAxisIndexs]);
        }
        else
        {
            this.segmentRotator.RotateSegmetnByAngle(this.shufflingSpeed, shuffleAngles[currentAngleIndex]);
        }
    }

    private List<Transform> FindPiecesOnSameAxis(Vector3 piecePoint, Vector3 rotationPlaneNormal)
    {
        List<Transform> piecesToRotate = new List<Transform>();

        foreach (CubePiece piece in this.pieces)
        {
            if (this.IsPointLyingOnPlane(piece.transform.localPosition, piecePoint, rotationPlaneNormal))
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

    public void ResetCubeState()
    {
        if(this.firstGraspedPiece == null && this.secondGraspedPiece == null)
        {
            foreach (CubePiece piece in this.pieces)
            {
                piece.ResetPiecePositionAndRotation();
            }

            this.segmentRotator.ResetSegment();
            this.ShufflingOn = false;
        }
    }

    private bool ShufflingOn = false;

    public void SwitchShuffling()
    {
        if (this.ShufflingOn)
        {
            this.ShufflingOn = false;
        }
        else if (!this.segmentRotator.SegmentIsRotating())
        {
            this.ShufflingOn = true;
        }
    }
}