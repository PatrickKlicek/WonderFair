using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public TextMeshProUGUI scoreboardText;
    public GameType gameType;

    public enum GameType
    {
        DuckShooting,
        Buzzwire,
        CanSmash,
        WhackAMole
    }

    private void Start()
    {
        StartCoroutine(WaitAndDisplay());
    }

    private IEnumerator WaitAndDisplay()
    {
        yield return new WaitUntil(() => HighscoreManager.Instance != null && HighscoreManager.Instance.IsLoaded);
        DisplayScores();
    }

    public void DisplayScores()
    {
        string gameName = gameType switch
        {
            GameType.DuckShooting => HighscoreManager.DUCK_SHOOTING,
            GameType.Buzzwire => HighscoreManager.BUZZWIRE,
            GameType.CanSmash => HighscoreManager.CAN_SMASH,
            GameType.WhackAMole => HighscoreManager.WHACK_A_MOLE,
            _ => ""
        };

        List<HighscoreEntry> scores = HighscoreManager.Instance.GetHighscores(gameName);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("Highscore:");

        if (scores.Count != 0)
        {
            for (int i = 0; i < scores.Count; i++)
            {
                sb.AppendLine($"{i + 1}. {scores[i].score}");
            }
        }
        scoreboardText.text = sb.ToString();
    }
}