using UnityEngine;
using Oculus.Interaction;
using System.Collections;

public class WireConstrainedTransformer : MonoBehaviour, ITransformer
{
    public WireSpline wire;
    public float ringRadius = 0.02f;
    public Transform ringCenter;
    public AudioSource buzzSound;
    private Vector3 _smoothedTangent = Vector3.up;

    [Range(0f, 1f)]
    public float rotationAlignSpeed = 0.15f;

    private IGrabbable _grabbable;
    private bool _isTouching = false;
    private bool _isCooldown = false;
    private Pose _previousGrabPose;

    public void Initialize(IGrabbable grabbable)
    {
        _grabbable = grabbable;
    }

    public void BeginTransform()
    {
        var grabPoint = _grabbable.GrabPoints[0];
        _previousGrabPose = new Pose(grabPoint.position, grabPoint.rotation);
    }

    public void UpdateTransform()
    {
        var grabPoint = _grabbable.GrabPoints[0];
        Pose currentGrabPose = new Pose(grabPoint.position, grabPoint.rotation);

        Quaternion rotationDelta = currentGrabPose.rotation * Quaternion.Inverse(_previousGrabPose.rotation);
        Vector3 positionDelta = currentGrabPose.position - _previousGrabPose.position;
        transform.rotation = rotationDelta * transform.rotation;
        transform.position += positionDelta;

        _previousGrabPose = currentGrabPose;

        if (wire == null) return;

        float stepSize = wire.wireRadius * 0.5f;
        float totalDelta = positionDelta.magnitude;
        int steps = Mathf.Max(1, Mathf.CeilToInt(totalDelta / stepSize));

        for (int step = 0; step < steps; step++)
        {
            ApplyConstraint();
        }
    }

    private void ApplyConstraint()
    {
        Vector3 centerPos = ringCenter.position;
        Vector3 closest = wire.GetClosestPoint(centerPos);
        float dist = Vector3.Distance(centerPos, closest);
        float maxDist = wire.wireRadius + ringRadius;

        bool touching = dist > maxDist * 0.95f;

        if (touching)
        {
            Vector3 radialDir = (centerPos - closest).normalized;

            if (dist < 0.001f)
                radialDir = transform.up;

            Vector3 targetCenter = closest + radialDir * maxDist;
            transform.position += targetCenter - centerPos;

            Vector3 wireTangent = GetWireTangent(closest);
            _smoothedTangent = Vector3.Slerp(_smoothedTangent, wireTangent, 0.3f);

            if (_smoothedTangent != Vector3.zero)
            {
                Vector3 currentUp = transform.up;
                Vector3 rotAxis = Vector3.Cross(currentUp, _smoothedTangent);

                if (rotAxis.sqrMagnitude > 0.0001f)
                {
                    float angle = Vector3.SignedAngle(currentUp, _smoothedTangent, rotAxis);
                    transform.RotateAround(ringCenter.position, rotAxis, angle * rotationAlignSpeed);
                }
            }

            if (!_isTouching && !_isCooldown)
                OnWireTouched();
        }
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
        _isTouching = true;

        if (buzzSound != null && !buzzSound.isPlaying)
            buzzSound.Play();

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
}