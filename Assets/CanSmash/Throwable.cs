using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Throwable : MonoBehaviour
{
    List<Vector3> trackingPos = new List<Vector3>();
    public float velocity = 1000f;

    bool pickedUp = false;
    GameObject parentHand;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (pickedUp == true)
        {
            rb.useGravity = false;
            rb.isKinematic = true;

            transform.position = parentHand.transform.position;
            transform.rotation = parentHand.transform.rotation;

            if (trackingPos.Count > 15)
            {
                trackingPos.RemoveAt(0);
            }
            trackingPos.Add(transform.position);

            float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);

            if (triggerRight < 0.1f)
            {
                pickedUp = false;

                Vector3 direction = trackingPos[trackingPos.Count - 1] - trackingPos[0];

                rb.isKinematic = false;
                rb.useGravity = true;

                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                rb.AddForce(direction * velocity);
                GetComponent<Collider>().isTrigger = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        float triggerRight = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger);

        if (other.gameObject.tag == "Hand" && triggerRight > 0.9f)
        {
            pickedUp = true;
            parentHand = other.gameObject;
        }
    }
}