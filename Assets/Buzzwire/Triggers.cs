using UnityEngine;

public class Triggers : MonoBehaviour
{
    public enum ZoneType { Start, End }
    public ZoneType zoneType;
    public BuzzWireGame game;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (zoneType == ZoneType.Start) game.OnRingEnteredStart();
        else game.OnRingReachedEnd();
    }
}
