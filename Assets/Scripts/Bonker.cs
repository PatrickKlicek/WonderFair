using UnityEngine;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class Bonker : MonoBehaviour
{
    public TextMeshProUGUI debugText;


    void OnTriggerEnter(Collider other)
    {
        var mole = other.GetComponent<MoleController>();
        if (mole == null)
            mole = other.GetComponentInParent<MoleController>();

        if (mole != null && mole.IsUp && !mole.IsWhacked)
        {
            Debug.Log("Hit mole!");
            debugText.text = "Hit!";
            mole.Whack();
        }
    }

}