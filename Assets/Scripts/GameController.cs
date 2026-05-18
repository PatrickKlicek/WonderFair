using Oculus.Interaction;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Grabbable _grabbable;
    [SerializeField] private WhackAMole _whackAMole;

    private void Start()
    {
        _grabbable.WhenPointerEventRaised += OnPointerEvent;
    }

    private void OnDestroy()
    {
        if (_grabbable != null)
            _grabbable.WhenPointerEventRaised -= OnPointerEvent;
    }

    private void OnPointerEvent(PointerEvent evt)
    {
        if (evt.Type == PointerEventType.Select)
            _whackAMole.StartGame();
    }
}
