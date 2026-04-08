using UnityEngine;

public class Target : MonoBehaviour
{
    private bool isRaised = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    [ContextMenu("Raise")]
    public void RaiseTarget()
    {
        isRaised = true;
        if (animator == null)
            animator = GetComponent<Animator>();
        animator.SetBool("isRaised", true);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isRaised && collision.collider.CompareTag("Hit"))
        {
            isRaised = false;
            animator.SetBool("isRaised", false);
        }
    }
}
