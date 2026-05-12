using UnityEngine;

public class BetterThrowing : MonoBehaviour
{
    [Header("References")]
    public Rigidbody leftControllerRb;
    public Rigidbody rightControllerRb;

    private Rigidbody controllerRb;
    private Rigidbody objectRb;

    [Header("Throw Settings")]
    public int velocityFrames = 5;

    private Vector3[] velocityHistory;
    private Vector3[] angularVelocityHistory;

    private int frame = 0;

    void Start()
    {
        objectRb = GetComponent<Rigidbody>();

        leftControllerRb = GameObject.Find("LeftControllerAnchor").GetComponent<Rigidbody>();

        rightControllerRb = GameObject.Find("RightControllerAnchor").GetComponent<Rigidbody>();

        velocityHistory = new Vector3[velocityFrames];
        angularVelocityHistory = new Vector3[velocityFrames];
    }

    void FixedUpdate()
    {
        if (controllerRb == null)
            return;

        StoreVelocities();
    }

    void StoreVelocities()
    {
        velocityHistory[frame] = controllerRb.linearVelocity;
        angularVelocityHistory[frame] = controllerRb.angularVelocity;

        frame++;

        if (frame >= velocityFrames)
            frame = 0;
    }

    Vector3 AverageVector(Vector3[] vectors)
    {
        Vector3 total = Vector3.zero;

        foreach (Vector3 v in vectors)
        {
            total += v;
        }

        return total / vectors.Length;
    }

    public void SetLeftHand()
    {
        controllerRb = leftControllerRb;
    }

    public void SetRightHand()
    {
        controllerRb = rightControllerRb;
    }

    public void ThrowObject()
    {
        if (controllerRb == null)
            return;

        Vector3 avgVelocity = AverageVector(velocityHistory);
        Vector3 avgAngularVelocity = AverageVector(angularVelocityHistory);

        Vector3 controllerCOM = controllerRb.worldCenterOfMass;

        Vector3 relativePos = objectRb.worldCenterOfMass - controllerCOM;

        Vector3 rotationalVelocity =
            Vector3.Cross(avgAngularVelocity, relativePos);

        Vector3 finalVelocity =
            avgVelocity + rotationalVelocity;

        objectRb.linearVelocity = finalVelocity;
        objectRb.angularVelocity = avgAngularVelocity;

        controllerRb = null;
    }
}