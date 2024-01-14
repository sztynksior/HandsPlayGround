using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;
using UnityEngine.UIElements;

public class SegmentRotatorTests
{
    Transform cubeTranform = new GameObject().transform;
    Vector3 rotationAxis = new Vector3(1f, 0f, 0f);
    float rotationSpeedMultipler = 2f;

    SegmentRotator segmentRotatorUnderTest;
    List<Transform> piecesTranforms;

    [SetUp]
    public void Setup()
    {
        List<float> alignAngles = new List<float>() { 0, 90, 180, 270 };
        this.segmentRotatorUnderTest = new SegmentRotator(alignAngles, this.cubeTranform, this.rotationSpeedMultipler);
        Transform pieceTranform = new GameObject().transform;
        pieceTranform.position = Vector3.one;
        this.piecesTranforms = new List<Transform>() { pieceTranform };
    }

    [TearDown] 
    public void Teardown() 
    { 
        this.segmentRotatorUnderTest = null;
        this.piecesTranforms = null;
    }

    [Test]
    [TestCase(float.MaxValue)]
    [TestCase(0.000001f)]
    [TestCase(float.MinValue)]
    public void RotateSegmentRotatesCorrectly(float rotationSpeed)
    {      
        this.segmentRotatorUnderTest.SetRotatingSegment(this.piecesTranforms, this.rotationAxis);

        segmentRotatorUnderTest.RotateSegment(rotationSpeed);

        Transform desiredPosition = new GameObject().transform;
        desiredPosition.position = Vector3.one;
        desiredPosition.RotateAround(cubeTranform.position, cubeTranform.TransformDirection(this.rotationAxis), rotationSpeed);
        Assert.That(this.piecesTranforms[0].position, Is.EqualTo(desiredPosition.position).Using(Vector3EqualityComparer.Instance));
        Assert.That(this.piecesTranforms[0].rotation, Is.EqualTo(desiredPosition.rotation).Using(QuaternionEqualityComparer.Instance));
    }

    [Test]
    [TestCase(-1f, -1f, -1f, 1f, -1f, -1f)]
    [TestCase(-1000f, -1000f, -1000f, 1000f, -1000f, -1000f)]
    [TestCase(-1000000f, -1000000f, -1000000f, 1000000f, -1000000f, -1000000f)]
    public void RotateSegmentDependingOnPullRotatesCorrectly(float x1, float y1, float z1, float x2, float y2, float z2)
    {
        this.segmentRotatorUnderTest.SetRotatingSegment(this.piecesTranforms, this.rotationAxis);
        Vector3 pulledPiecePosition = new Vector3(x1, y1, z1);
        Vector3 grabPosition = new Vector3(x2, y2, z2);
        Vector3 pullVector = grabPosition - pulledPiecePosition;

        this.segmentRotatorUnderTest.RotateSegmentDependingOnPull(pulledPiecePosition, pullVector);

        Transform desiredPosition = new GameObject().transform;
        desiredPosition.position = Vector3.one;
        Vector3 vectorFromPieceToCenter = this.cubeTranform.localPosition - pulledPiecePosition;
        Vector3 rotationVector = Vector3.ProjectOnPlane(pullVector, this.rotationAxis);
        Vector3 radiusVector = Vector3.ProjectOnPlane(vectorFromPieceToCenter, this.rotationAxis);
        float rotationSpeed = rotationVector.magnitude * Mathf.Sin(Vector3.SignedAngle(rotationVector, radiusVector, this.rotationAxis) * Mathf.Deg2Rad) * this.rotationSpeedMultipler;
        desiredPosition.RotateAround(cubeTranform.position, cubeTranform.TransformDirection(rotationAxis), rotationSpeed);
        Assert.That(this.piecesTranforms[0].position, Is.EqualTo(desiredPosition.position).Using(Vector3EqualityComparer.Instance));
        Assert.That(this.piecesTranforms[0].rotation, Is.EqualTo(desiredPosition.rotation).Using(QuaternionEqualityComparer.Instance));
    }

    [Test]
    [TestCase(float.MaxValue, 180f)]
    [TestCase(float.MinValue, -180f)]
    [TestCase(float.MaxValue, -180f)]
    [TestCase(float.MinValue, 180f)]
    public void RotateSegmetnByAngleDoNotOvershoot(float rotationSpeed, float angle)
    {
        this.segmentRotatorUnderTest.SetRotatingSegment(this.piecesTranforms, this.rotationAxis);

        this.segmentRotatorUnderTest.RotateSegmetnByAngle(rotationSpeed, angle);

        Transform desiredPosition = new GameObject().transform;
        desiredPosition.position = Vector3.one;
        desiredPosition.RotateAround(cubeTranform.position, cubeTranform.TransformDirection(this.rotationAxis), angle);
        Assert.That(this.piecesTranforms[0].position, Is.EqualTo(desiredPosition.position).Using(Vector3EqualityComparer.Instance));
        Assert.That(this.piecesTranforms[0].rotation, Is.EqualTo(desiredPosition.rotation).Using(QuaternionEqualityComparer.Instance));
    }

    [Test]
    [TestCase(float.MaxValue, 190f, 180f)]
    [TestCase(float.MinValue, -10f, 0)]
    [TestCase(float.MaxValue, 250f, 270f)]
    [TestCase(float.MinValue, 91f, 90f)]
    public void AlignRotatedSegmentRotatesToCorrectAngle(float rotationSpeed, float startingAngle, float endAngle)
    {
        this.segmentRotatorUnderTest.SetRotatingSegment(this.piecesTranforms, this.rotationAxis);

        this.segmentRotatorUnderTest.RotateSegment(startingAngle);
        this.segmentRotatorUnderTest.AlignRotatedSegment(rotationSpeed);

        Transform desiredPosition = new GameObject().transform;
        desiredPosition.position = Vector3.one;
        desiredPosition.RotateAround(cubeTranform.position, cubeTranform.TransformDirection(this.rotationAxis), endAngle);
        Assert.That(this.piecesTranforms[0].position, Is.EqualTo(desiredPosition.position).Using(Vector3EqualityComparer.Instance));
        Assert.That(this.piecesTranforms[0].rotation, Is.EqualTo(desiredPosition.rotation).Using(QuaternionEqualityComparer.Instance));
    }

    [Test]
    public void ResetSegmentWorks()
    {
        this.segmentRotatorUnderTest.SetRotatingSegment(this.piecesTranforms, this.rotationAxis);
        float rotationSpeed = 90;

        this.segmentRotatorUnderTest.ResetSegment();
        this.segmentRotatorUnderTest.RotateSegment(rotationSpeed);

        Transform desiredPosition = new GameObject().transform;
        desiredPosition.position = Vector3.one;
        Assert.That(this.piecesTranforms[0].position, Is.EqualTo(desiredPosition.position).Using(Vector3EqualityComparer.Instance));
        Assert.That(this.piecesTranforms[0].rotation, Is.EqualTo(desiredPosition.rotation).Using(QuaternionEqualityComparer.Instance));
    }

    [Test]
    public void SegmentIsRotatingReturnsTrueAfterSetRotatingSegment()
    {
        this.segmentRotatorUnderTest.SetRotatingSegment(this.piecesTranforms, this.rotationAxis);

        Assert.That(this.segmentRotatorUnderTest.SegmentIsRotating, Is.True);
    }

    [Test]
    public void SegmentIsRotatingReturnsFalseAfterResetSegment()
    {
        this.segmentRotatorUnderTest.SetRotatingSegment(this.piecesTranforms, this.rotationAxis);
        this.segmentRotatorUnderTest.ResetSegment();

        Assert.That(this.segmentRotatorUnderTest.SegmentIsRotating, Is.False);
    }
}
