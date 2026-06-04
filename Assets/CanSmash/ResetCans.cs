using UnityEngine;

public class ResetCans : MonoBehaviour
{
    public ResetBall ball;
    public Transform[] cans;

    private Vector3[] canStartPositions;
    private Quaternion[] canStartRotations;
    private Rigidbody[] canRbs;

    void Start()
    {
        canStartPositions = new Vector3[cans.Length];
        canStartRotations = new Quaternion[cans.Length];
        canRbs = new Rigidbody[cans.Length];

        for (int i = 0; i < cans.Length; i++)
        {
            canStartPositions[i] = cans[i].position;
            canStartRotations[i] = cans[i].rotation;
            canRbs[i] = cans[i].GetComponent<Rigidbody>();
        }
    }

    public void ResetAllObjects()
    {
        for (int i = 0; i < cans.Length; i++)
        {
            if (canRbs[i] != null)
            {
                canRbs[i].linearVelocity = Vector3.zero;
                canRbs[i].angularVelocity = Vector3.zero;
            }
            cans[i].position = canStartPositions[i];
            cans[i].rotation = canStartRotations[i];
        }
    }
}
