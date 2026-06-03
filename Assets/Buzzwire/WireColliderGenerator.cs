using UnityEngine;

public class WireColliderGenerator : MonoBehaviour
{
    public WireSpline wire;

    void Start()
    {
        GenerateCapsuleColliders();
    }

    void GenerateCapsuleColliders()
    {
        // Ukloni stari Mesh Collider
        MeshCollider old = wire.GetComponent<MeshCollider>();
        if (old != null) old.enabled = false;

        for (int i = 0; i < wire.points.Length - 1; i++)
        {
            Vector3 a = wire.transform.TransformPoint(wire.points[i]);
            Vector3 b = wire.transform.TransformPoint(wire.points[i + 1]);

            GameObject seg = new GameObject($"WireCollider_{i}");
            seg.transform.parent = wire.transform;
            seg.layer = LayerMask.NameToLayer("Wire");
            seg.tag = "Wire";

            CapsuleCollider cap = seg.AddComponent<CapsuleCollider>();
            cap.radius = wire.wireRadius;
            cap.direction = 1; // Y os

            // Postavi poziciju i rotaciju izmeðu dvije toèke
            Vector3 mid = (a + b) / 2f;
            seg.transform.position = mid;
            seg.transform.up = (b - a).normalized;
            cap.height = Vector3.Distance(a, b) + wire.wireRadius * 2f;
        }
    }
}
