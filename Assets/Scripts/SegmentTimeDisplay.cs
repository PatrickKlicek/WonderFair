using UnityEngine;

public class SegmentTimeDisplay : MonoBehaviour
{
    [Tooltip("Digit displays ordered left to right.")]
    public SegmentDigit[] digits;
    public CarnivalGame game;
    [Tooltip("Show elapsed time instead of time remaining (for Buzzwire)")]
    public bool elapsedMode = false;

    void Start()
    {
        if (game == null)
            game = GetComponentInParent<CarnivalGame>();
    }

    void Update()
    {
        if (game == null || digits == null || digits.Length < 4) return;
        if (elapsedMode && !game.IsRunning) return;

        int totalSeconds = elapsedMode
            ? (int)GameManager.GM.timer.Elapsed.TotalSeconds
            : Mathf.Max(0, (int)game.TimeLeft.TotalSeconds);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        digits[0].SetDigit(minutes / 10);
        digits[1].SetDigit(minutes % 10);
        digits[2].SetDigit(seconds / 10);
        digits[3].SetDigit(seconds % 10);

        for (int i = 4; i < digits.Length; i++)
            digits[i].SetBlank();
    }
}
