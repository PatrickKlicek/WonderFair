using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class FloatingScorePopup : MonoBehaviour
{
    public float riseSpeed = 0.6f;
    public float lifetime  = 0.9f;
    public Color textColor = Color.black;

    private TextMeshPro _text;

    void Awake()
    {
        _text = GetComponent<TextMeshPro>();
    }

    public void Init(int points)
    {
        _text.text  = "+" + points;
        _text.color = textColor;
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        float   elapsed  = 0f;
        Vector3 startPos = transform.position;
        Camera  cam      = Camera.main;

        while (elapsed < lifetime)
        {
            float t = elapsed / lifetime;

            transform.position = startPos + Vector3.up * (riseSpeed * elapsed);

            if (cam != null)
                transform.forward = transform.position - cam.transform.position;

            float alpha = t < 0.5f ? 1f : 1f - ((t - 0.5f) / 0.5f);
            _text.color = new Color(textColor.r, textColor.g, textColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
