using UnityEngine;
using Oculus.Interaction;

public class RingConstraint : MonoBehaviour
{
    public AudioSource buzzSound;
    private WireConstrainedTransformer wireTransformer;
    private bool isGrabbed = false;

    void Start()
    {
        wireTransformer = GetComponent<WireConstrainedTransformer>();
        var grabbable = GetComponent<Grabbable>();
        if (grabbable != null)
            grabbable.WhenPointerEventRaised += OnPointerEvent;
    }

    void OnPointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select) isGrabbed = true;
        if (evt.Type == PointerEventType.Unselect) isGrabbed = false;
    }

    public void OnWireTouched()
    {
        if (!isGrabbed) return;
        wireTransformer?.OnWireTouched();
    }
}