using UnityEngine;

public class TableBlock : MonoBehaviour
{
    [SerializeField] private float checkRadius = 0.05f;
    [SerializeField] private LayerMask tableLayer;
    private WireConstrainedTransformer _transformer;

    void Start()
    {
        _transformer = GetComponentInParent<WireConstrainedTransformer>();
    }

    void LateUpdate()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, checkRadius, tableLayer);

        foreach (var hit in hits)
        {
            if (Physics.ComputePenetration(
                GetComponent<Collider>(), transform.position, transform.rotation,
                hit, hit.transform.position, hit.transform.rotation,
                out Vector3 direction, out float distance))
            {
                _transformer.transform.position += direction * distance;
            }
        }
    }
}
