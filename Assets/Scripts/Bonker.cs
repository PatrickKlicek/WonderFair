using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class Bonker : MonoBehaviour
{
    public TextMeshProUGUI debugText;

    [Header("Haptics")]
    [Range(0f, 1f)] public float hapticAmplitude = 0.8f;
    public float hapticDuration = 0.1f;

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
            SendHaptics();
        }
    }

    void SendHaptics()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(
            InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.HeldInHand, devices);
        foreach (var device in devices)
            device.SendHapticImpulse(0, hapticAmplitude, hapticDuration);
    }
}