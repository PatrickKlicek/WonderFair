using UnityEngine;

public class FallDetect : MonoBehaviour
{
    public CanSmash canSmash;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Can"))
        {
            int index = canSmash.GetCanIndex(other.GetComponent<Rigidbody>());
            if (index >= 0)
                canSmash.OnCanFallen(index);
        }
    }
}
