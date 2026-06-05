using UnityEngine;

public class SegmentBuzzCountDisplay : MonoBehaviour
{
    public SegmentDigit[] digits;
    public BuzzWireGame game;

    void Start()
    {
        if (game == null)
            game = GetComponentInParent<BuzzWireGame>();
    }

    void Update()
    {
        if (game == null || digits == null || digits.Length == 0) return;

        int buzzes = game.BuzzCount;
        for (int i = digits.Length - 1; i >= 0; i--)
        {
            if (digits[i] != null)
                digits[i].SetDigit(buzzes % 10);
            buzzes /= 10;
        }
    }
}
