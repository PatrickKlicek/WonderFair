using UnityEngine;
using System.Collections.Generic;
using Oculus.Interaction;

public class ThrowableTransformer : MonoBehaviour, ITransformer
{
    [SerializeField] private float velocity = 1000f;

    private List<Vector3> trackingPos = new List<Vector3>();
    private IGrabbable _grabbable;
    private Rigidbody _rb;

    public void Initialize(IGrabbable grabbable)
    {
        _grabbable = grabbable;
        _rb = grabbable.Transform.GetComponentInParent<Rigidbody>();

    }

    public void BeginTransform()
    {
        trackingPos.Clear();
        _rb.GetComponent<Collider>().isTrigger = true;
    }

    public void UpdateTransform()
    {
        var grabPoint = _grabbable.GrabPoints[0];

        _grabbable.Transform.position = grabPoint.position;
        _grabbable.Transform.rotation = grabPoint.rotation;

        if (trackingPos.Count > 15)
            trackingPos.RemoveAt(0);

        trackingPos.Add(_grabbable.Transform.position);
    }

    public void EndTransform()
    {
        Debug.Log("EndTransform pozvan, trackingPos count: " + trackingPos.Count);

        if (trackingPos.Count < 2) return;

        Vector3 direction = trackingPos[trackingPos.Count - 1] - trackingPos[0];

        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.AddForce(direction * velocity);
        _rb.GetComponent<Collider>().isTrigger = false;
    }
}