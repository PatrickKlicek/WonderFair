using Oculus.Interaction;
using UnityEngine;

public class CameraRaiseLower : MonoBehaviour
{
    public Transform anchorLeft;
    public Transform anchorRight;
    public Transform anchorCenter;
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
        UpdateTransformHeight(anchorLeft, diff);
        UpdateTransformHeight(anchorRight, diff);
        UpdateTransformHeight(anchorCenter, diff);
    }

    void UpdateTransformHeight(Transform t, float d)
    {
        t.position = new Vector3(
            t.position.x,
            Mathf.Clamp(t.position.y + d, minHeight, maxHeight),
            t.position.z
            );
    }
}
