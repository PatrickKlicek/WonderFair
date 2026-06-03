using UnityEngine;
using Oculus.Interaction;

public class RingConstraint : MonoBehaviour
{
    private Transform _root;

    void Start()
    {
        // Uzmi root buzzwirehandle objekt
        _root = GetComponentInParent<WireConstrainedTransformer>().transform;
    }

    void OnTriggerEnter(Collider other)
    {
        ApplyPenetrationFix(other);
    }

    void OnTriggerStay(Collider other)
    {
        ApplyPenetrationFix(other);
    }

    private void ApplyPenetrationFix(Collider other)
    {
        if (!other.CompareTag("Wire")) return;

        SphereCollider mySphere = GetComponent<SphereCollider>();

        if (Physics.ComputePenetration(
            mySphere, transform.position, transform.rotation,
            other, other.transform.position, other.transform.rotation,
            out Vector3 direction, out float distance))
        {
            _root.position += direction * distance;

            WireConstrainedTransformer transformer =
                _root.GetComponent<WireConstrainedTransformer>();
            transformer?.OnWireTouched();
        }
    }
}