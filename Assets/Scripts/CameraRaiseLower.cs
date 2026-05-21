using Oculus.Interaction;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine;

public class CameraRaiseLower : MonoBehaviour
{
    public CameraOffset cameraOffset;
    public float minHeight;
    public float maxHeight;
    public float heightIncrement = 0.05f;

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X)) UpdateCameraHeight(-heightIncrement);
        else if (OVRInput.GetDown(OVRInput.RawButton.Y)) UpdateCameraHeight(heightIncrement);
    }

    void UpdateCameraHeight(float diff)
    {
        cameraOffset.cameraYOffset = Mathf.Clamp(cameraOffset.cameraYOffset + diff, minHeight, maxHeight);
    }
}
