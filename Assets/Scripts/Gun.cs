using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.XR;

public class Gun : MonoBehaviour
{
    public Transform placeholder;
    public GameObject ballPrefab;
    public float ballSpeed;
    public float shootInterval = 0.3f;
    public GrabInteractable grabInteractable;
    [Range(0.01f, 1f)] public float hapticAmplitude = 0.8f;
    public float hapticDuration = 0.1f;

    private float shootTimestamp = 0;

    // Update is called once per frame
    void Update()
    {
        if ((OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) || OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger)) && grabInteractable.State == InteractableState.Select)
        {
            if (Time.time -  shootTimestamp > shootInterval)
            {
                GameObject b = Instantiate(ballPrefab, placeholder.position, Quaternion.identity);
                b.GetComponent<Rigidbody>().linearVelocity = placeholder.forward * ballSpeed;
                SendHaptics();
            }
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
