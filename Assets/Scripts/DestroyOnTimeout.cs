using UnityEngine;

public class DestroyOnTimeout : MonoBehaviour
{
    public float lifetime = 30;

    private float timestamp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timestamp = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - timestamp > lifetime)
            Destroy(gameObject);
    }
}
