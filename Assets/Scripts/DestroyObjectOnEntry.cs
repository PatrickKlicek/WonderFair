using UnityEngine;

public class DestroyObjectOnEntry : MonoBehaviour
{
    public string targetTag = null;

    void OnTriggerEnter(Collider collider)
    {
        if (targetTag == null)
            Destroy(collider.gameObject);
        else if (collider.CompareTag(targetTag))
        {
            Transform target = collider.transform;
            while (target.parent.CompareTag(targetTag))
            {
                target = target.parent;
            }
            Destroy(target.gameObject);
        }
    }
}
