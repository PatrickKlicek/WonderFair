using UnityEngine;

public class RingColliderChild : MonoBehaviour
{
    private WireConstrainedTransformer wireTransformer;
    public GameObject wireMesh;

    void Start()
    {
        wireTransformer = GetComponentInParent<WireConstrainedTransformer>();
    }

    //Kada se dodirne zica pokrreni funkciju za pustanjje zvuka i cooldown
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject == wireMesh) {
            wireTransformer?.OnWireTouched();
        }
    }
}