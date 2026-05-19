using UnityEngine;

public class BallFallDetect : MonoBehaviour
{
    public CanSmash canSmash;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
            canSmash.OnBallHitGround();
    }
}
