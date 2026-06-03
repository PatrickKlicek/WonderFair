using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine;
using System.Collections;

public class WireConstrainedTransformer : MonoBehaviour, ITransformer
{
    public WireSpline wire;
    public float ringRadius = 0.01f;
    public Transform ringCenter;
    public AudioSource buzzSound;

    [Range(0f, 1f)] public float hapticAmplitude = 0.8f;
    public float hapticDuration = 0.1f;

    [Range(0f, 1f)]
    public float rotationAlignSpeed = 0.15f;

    private BuzzWireGame _buzzWireGame;
    private IGrabbable _grabbable;
    private bool _isTouching = false;
    private bool _isCooldown = false;
    private Pose _previousGrabPose;

    [Header("Constraint Settings")]
    [SerializeField] private float maxStepSize = 0.005f;
    [SerializeField] private int maxSubsteps = 10;
    [SerializeField] private float snapThreshold = 0.15f;

    public void Initialize(IGrabbable grabbable)
    {
        _grabbable = grabbable;
    }

    public void BeginTransform()
    {
        var grabPoint = _grabbable.GrabPoints[0];
        _previousGrabPose = new Pose(grabPoint.position, grabPoint.rotation);

        if (_buzzWireGame == null)
            _buzzWireGame = GameObject.FindFirstObjectByType<BuzzWireGame>();
    }

    public void UpdateTransform()
    {
        if (wire == null) return;

        var grabPoint = _grabbable.GrabPoints[0];
        Pose currentGrabPose = new Pose(grabPoint.position, grabPoint.rotation);

        float jumpDist = Vector3.Distance(currentGrabPose.position, _previousGrabPose.position);
        if (jumpDist > snapThreshold)
        {
            _previousGrabPose = currentGrabPose;
            return;
        }

        Vector3 positionDelta = currentGrabPose.position - _previousGrabPose.position;
        Quaternion rotationDelta = currentGrabPose.rotation * Quaternion.Inverse(_previousGrabPose.rotation);

        int steps = Mathf.Clamp(
            Mathf.CeilToInt(positionDelta.magnitude / maxStepSize),
        1, maxSubsteps
        );

        for (int i = 0; i < steps; i++)
        {
            transform.position += positionDelta / steps;
            transform.rotation = rotationDelta * transform.rotation;
            ApplyConstraint();
        }

        _previousGrabPose = currentGrabPose;
    }

    private void ApplyConstraint()
    {
        Vector3 centerPos = ringCenter.position;
        Vector3 closest = wire.GetClosestPoint(centerPos);

        Vector3 wireTangent = GetWireTangent(closest);
        if (wireTangent == Vector3.zero) wireTangent = Vector3.up;

        Vector3 toCenter = centerPos - closest;
        Vector3 toCenter_alongWire = Vector3.Project(toCenter, wireTangent);
        Vector3 toCenter_radial = toCenter - toCenter_alongWire;
        float radialDist = toCenter_radial.magnitude;

        float effectiveWireRadius = wire.wireRadius > 0.001f ? wire.wireRadius : 0.005f;
        float maxAllowedDist = ringRadius - effectiveWireRadius;
        if (maxAllowedDist <= 0) return;

        if (radialDist > maxAllowedDist)
        {
            Vector3 radialDir = radialDist > 0.0001f
                ? toCenter_radial / radialDist
                : GetPerpendicularToTangent(wireTangent);

            Vector3 correctedCenter = closest + toCenter_alongWire + radialDir * maxAllowedDist;
            transform.position += correctedCenter - centerPos;

            OnWireTouched();
        }
    }

    private Vector3 GetPerpendicularToTangent(Vector3 tangent)
    {
        Vector3 candidate = Mathf.Abs(tangent.y) < 0.9f ? Vector3.up : Vector3.right;
        return Vector3.Cross(tangent, candidate).normalized;
    }

    private Vector3 GetWireTangent(Vector3 closestPoint)
    {
        if (wire.points == null || wire.points.Length < 2)
            return Vector3.zero;

        float minDist = float.MaxValue;
        Vector3 tangent = Vector3.zero;

        for (int i = 0; i < wire.points.Length - 1; i++)
        {
            Vector3 a = wire.transform.TransformPoint(wire.points[i]);
            Vector3 b = wire.transform.TransformPoint(wire.points[i + 1]);

            float distToSeg = Vector3.Distance(
                closestPoint,
                ClosestPointOnSegment(a, b, closestPoint)
            );

            if (distToSeg < minDist)
            {
                minDist = distToSeg;
                tangent = (b - a).normalized;
            }
        }

        return tangent;
    }

    private Vector3 ClosestPointOnSegment(Vector3 a, Vector3 b, Vector3 p)
    {
        Vector3 ab = b - a;
        float t = Vector3.Dot(p - a, ab) / Vector3.Dot(ab, ab);
        return a + Mathf.Clamp01(t) * ab;
    }

    public void EndTransform() { }

    public void OnWireTouched()
    {
        if (_isCooldown) return;
        if (!BuzzWireGame.GameActive) return;

        _isTouching = true;
        _isCooldown = true;

        if (_buzzWireGame == null)
            _buzzWireGame = GameObject.FindFirstObjectByType<BuzzWireGame>();

        if (buzzSound != null && !buzzSound.isPlaying)
        {
            buzzSound.Play();
            SendHaptics();
            _buzzWireGame.buzzCount++;
        }

        StartCoroutine(ResetTouching());
    }

    private IEnumerator ResetTouching()
    {
        yield return new WaitForSeconds(0.05f);
        _isTouching = false;
        _isCooldown = true;
        yield return new WaitForSeconds(0.1f);
        _isCooldown = false;

        if (!_isTouching && buzzSound != null)
            buzzSound.Stop();
    }

    void SendHaptics()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(
            InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.HeldInHand, devices);
        foreach (var device in devices)
            device.SendHapticImpulse(0, hapticAmplitude, hapticDuration);
    }
}