using UnityEngine;

public class ResetObjectPosition : MonoBehaviour
{
    public GameObject obj;

    private Rigidbody rb;
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        rb = obj.GetComponent<Rigidbody>();
        startPosition = obj.transform.position;
        startRotation = obj.transform.rotation;
    }

    public void ResetPosition()
    {
        if (!rb.isKinematic)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            obj.transform.SetPositionAndRotation(startPosition + Vector3.up * 0.075f, startRotation);
        }
    }
}
