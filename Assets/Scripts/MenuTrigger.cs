using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    public GameObject menuCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            menuCanvas.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) menuCanvas.SetActive(false);
    }
}
