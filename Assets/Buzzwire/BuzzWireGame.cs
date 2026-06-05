using UnityEngine;
using System;
using TMPro;
using System.Collections;

public class BuzzWireGame : CarnivalGame
{
    [Header("Game specific")]
    public int buzzCount = 0;
    public static bool GameActive = false;
    public float baseScore = 13200f;
    public float penaltyPerBuzz = 0.2f;
    public TextMeshProUGUI finalScoreText;
    public Scoreboard Scoreboard;

    public Transform handle;
    public float resetDelay = 10f;
    private Vector3 _initialLocalPosition;
    private Quaternion _initialLocalRotation;

    protected override void Start()
    {
        base.Start();
        if (handle != null)
        {
            _initialLocalPosition = handle.localPosition;
            _initialLocalRotation = handle.localRotation;
        }

        if (finalScoreText != null)
        {
            finalScoreText.gameObject.SetActive(false);
        }
    }

    protected override void Update()
    {
        //Debug.Log($"Update: GameActive:{BuzzWireGame.GameActive}, buzzCount:{buzzCount}");
        if (GameActive)
        {
            TimeSpan elapsed = GameManager.GM.timer.Elapsed;

            if (timer != null)
            {
                if (elapsed.TotalSeconds < 10)
                    timer.text = string.Format("{0:0}.{1:0}", elapsed.Seconds, elapsed.Milliseconds / 100);
                else if (elapsed.TotalSeconds < 60)
                    timer.text = string.Format("{0:00}.{1:0}", elapsed.Seconds, elapsed.Milliseconds / 100);
                else
                    timer.text = string.Format("{0:0}:{1:00}", elapsed.Minutes, elapsed.Seconds);
            }
            if (scoreboard != null) scoreboard.text = buzzCount.ToString();
        }
    }

    public void OnRingEnteredStart()
    {
        if (!GameActive)
        {
            GameActive = true;
            GameManager.GM.inGame = false;
            buzzCount = 0;
            if (finalScoreText != null)
                finalScoreText.gameObject.SetActive(false);
            PlayStartSound();
            StartGame();
        }
    }

    public void OnRingReachedEnd()
    {
        if (GameActive)
        {
            GameActive = false;
            float timeSeconds = (float)GameManager.GM.timer.Elapsed.TotalSeconds;
            float penaltyMultiplier = 1f / (1f + buzzCount * penaltyPerBuzz);
            int finalScore = Mathf.RoundToInt((baseScore / timeSeconds) * penaltyMultiplier);
            score = finalScore;
            EndGame();
            PlayEndSound();
            HighscoreManager.Instance.SubmitScore(HighscoreManager.BUZZWIRE, finalScore);
            Scoreboard?.DisplayScores();
            if (finalScoreText != null)
            {
                finalScoreText.gameObject.SetActive(true);
                finalScoreText.text = $"{finalScore}";
            }
            StartCoroutine(ResetAfterDelay());
        }
    }

    public void RegisterBuzz()
    {
        Debug.Log($"RegisterBuzz: GameActive:{GameActive}, buzzCount:{buzzCount}");
        if (GameActive)
            buzzCount++;
    }

    public override bool IsRunning => GameActive;
    public int           BuzzCount => buzzCount;

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        if (finalScoreText != null)
            finalScoreText.gameObject.SetActive(false);
        buzzCount = 0;
        if (scoreboard != null) scoreboard.text = "";
        if (timer != null) timer.text = "";
        GameManager.GM.inGame = false;
        GameActive = false;
        if (handle != null)
        {
            handle.localPosition = _initialLocalPosition;
            handle.localRotation = _initialLocalRotation;
        }
    }
}