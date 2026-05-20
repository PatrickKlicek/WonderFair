using Oculus.Interaction;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform placeholder;
    public GameObject ballPrefab;
    public float ballSpeed;
    public float shootInterval = 0.3f;
    public GrabInteractable grabInteractable;

    private float shootTimestamp = 0;

    // Update is called once per frame
    void Update()
    {
        if ((OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger)) && grabInteractable.State == InteractableState.Select)
        {
            if (Time.time -  shootTimestamp > shootInterval)
            {
                GameObject b = Instantiate(ballPrefab, placeholder.position, Quaternion.identity);
                b.GetComponent<Rigidbody>().linearVelocity = placeholder.forward * ballSpeed;
            }
        }
    }
}
