using UnityEngine;

public class FallDetect : MonoBehaviour
{
    public CanSmash canSmash;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER HIT: " + other.name);

        if (other.CompareTag("Can"))
        {
            Debug.Log("CAN DETECTED!");

            Rigidbody rb = other.GetComponent<Rigidbody>();
            int index = canSmash.GetCanIndex(rb);

            Debug.Log("INDEX: " + index);

            if (index >= 0)
            {
                Vector3 pos = other.transform.position;
                canSmash.OnCanFallen(index, pos);
            }
        }
    }
}
