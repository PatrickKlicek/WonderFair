using UnityEngine;

public class BallHitSound : MonoBehaviour
{
    public CanSmash canSmash;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        Rigidbody limenkaRb = collision.gameObject.GetComponent<Rigidbody>();

        if (limenkaRb == null) return;

        int canIndex = canSmash.GetCanIndex(limenkaRb);

        if (canIndex != -1 && !canSmash._fallenCans.Contains(canIndex))
        {
            audioSource.Play();
        }
    }
}
