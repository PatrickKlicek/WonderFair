using UnityEngine;

public class MenuTrigger : MonoBehaviour
{
    public GameObject menu;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            menu.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) menu.SetActive(false);
    }
}
