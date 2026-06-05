using UnityEngine;

public class SegmentDigit : MonoBehaviour
{
    public GameObject[] digits = new GameObject[10];

    public void SetDigit(int digit)
    {
        digit = Mathf.Clamp(digit, 0, 9);
        for (int i = 0; i < digits.Length; i++)
            if (digits[i] != null) digits[i].SetActive(i == digit);
    }

    public void SetBlank()
    {
        foreach (var d in digits)
            if (d != null) d.SetActive(false);
    }
}
