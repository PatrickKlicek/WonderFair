using UnityEngine;

public class MovingTargetSpawner : MonoBehaviour
{
    public Transform spawn;
    public GameObject targetPrefab;
    public float targetSpeed = 3;

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        GameObject t = Instantiate(targetPrefab, spawn.position, transform.rotation, transform);
        t.transform.Rotate(Vector3.forward * 90);
        t.GetComponent<Rigidbody>().linearVelocity = spawn.forward * targetSpeed;
        t.GetComponent<Target>().RaiseTarget();
    }
}
