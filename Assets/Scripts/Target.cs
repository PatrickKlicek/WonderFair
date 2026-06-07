using System;
using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    public static event Action<int> TargetHit;
    public int points;
    public float duration = 5;
    public GameObject scorePopup;
    [HideInInspector] public float raiseTimestamp = 0;

    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (raiseTimestamp != 0 && Time.time - raiseTimestamp > duration)
            LowerTarget();
    }

    [ContextMenu("Raise")]
    public void RaiseTarget()
    {
        raiseTimestamp = Time.time;
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("isRaised", true);
    }

    public void LowerTarget()
    {
        raiseTimestamp = 0;
        animator.SetBool("isRaised", false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (raiseTimestamp != 0 && collision.collider.CompareTag("Hit"))
        {
            TargetHit.Invoke(points);
            GameObject sp = Instantiate(scorePopup, transform.position, Quaternion.identity);
            sp.GetComponent<FloatingScorePopup>().Init(points);
            audioSource.Play();
            LowerTarget();
        }
    }
}
