using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
public class Bonker : MonoBehaviour
{
    public TextMeshProUGUI debugText;
    public Transform hammerHead;
    public float velocityThreshold = 1f;
    [Range(0f, 1f)] public float velocityMeasurePeriod = 0.5f;

    [Header("Haptics")]
    [Range(0.01f, 1f)] public float hapticAmplitude = 0.8f;
    public float hapticDuration = 0.1f;

    private float verticalVelocity;
    private float previousHeight;
    private float swingTimestamp = 0;

    void Start()
    {
        previousHeight = hammerHead.position.y;
    }

    void FixedUpdate()
    {
        verticalVelocity = (hammerHead.position.y - previousHeight) / Time.deltaTime;
        previousHeight = hammerHead.position.y;

        if (verticalVelocity < -velocityThreshold) Debug.Log(verticalVelocity);
        if (verticalVelocity < -velocityThreshold) swingTimestamp = Time.time;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Target") || Time.time - swingTimestamp > velocityMeasurePeriod) return;

        var mole = other.GetComponentInParent<MoleController>();

        if (mole != null && mole.IsUp && !mole.IsWhacked)
        {
            Debug.Log("Hit mole!");
            mole.Whack();
            SendHaptics();
            if (debugText != null) debugText.text = "Hit!";
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