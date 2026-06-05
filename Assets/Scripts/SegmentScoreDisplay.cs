using UnityEngine;

public class SegmentScoreDisplay : MonoBehaviour
{
    [Tooltip("Digit displays ordered left to right, most significant first.")]
    public SegmentDigit[] digits;
    public CarnivalGame game;

    void Start()
    {
        if (game == null)
            game = GetComponentInParent<CarnivalGame>();
    }

    void Update()
    {
        if (game == null || digits == null || digits.Length == 0) return;

        int score = game.Score;
        for (int i = digits.Length - 1; i >= 0; i--)
        {
            if (digits[i] != null)
                digits[i].SetDigit(score % 10);
            score /= 10;
        }
    }
}
