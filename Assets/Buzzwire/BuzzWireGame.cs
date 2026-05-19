using UnityEngine;
using System;
using TMPro;
using System.Collections;

public class BuzzWireGame : CarnivalGame
{
    public int buzzCount = 0;
    public bool IsRunning => isRunning;

    public float baseScore = 13200f;
    public float penaltyPerBuzz = 0.2f;
    public TextMeshProUGUI finalScoreText;

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
        if (isRunning)
        {
            TimeSpan elapsed = GameManager.GM.timer.Elapsed;

            if (elapsed.TotalSeconds < 10)
                timer.text = string.Format("{0:0}.{1:0}", elapsed.Seconds, elapsed.Milliseconds / 100);
            else if (elapsed.TotalSeconds < 60)
                timer.text = string.Format("{0:00}.{1:0}", elapsed.Seconds, elapsed.Milliseconds / 100);
            else
                timer.text = string.Format("{0:0}:{1:00}", elapsed.Minutes, elapsed.Seconds);

            scoreboard.text = buzzCount.ToString();
        }
    }

    public void OnRingEnteredStart()
    {
        if (!isRunning)
        {
            buzzCount = 0;
            if (finalScoreText != null)
            {
                finalScoreText.gameObject.SetActive(false);
            }
            StartGame();
            Debug.Log("Start game");
        }
    }

    public void OnRingReachedEnd()
    {
        if (isRunning)
        {
            float timeSeconds = (float)GameManager.GM.timer.Elapsed.TotalSeconds;
            float penaltyMultiplier = 1f / (1f + buzzCount * penaltyPerBuzz);
            int finalScore = Mathf.RoundToInt((baseScore / timeSeconds) * penaltyMultiplier);
            EndGame();
            finalScoreText.gameObject.SetActive(true);
            finalScoreText.text = $"{finalScore}";
            Debug.Log("End game");
            StartCoroutine(ResetAfterDelay());
        }
    }

    public void RegisterBuzz()
    {
        if (isRunning)
            buzzCount++;
    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);

        if (finalScoreText != null)
        {
            finalScoreText.gameObject.SetActive(false);
        }

        buzzCount = 0;
        scoreboard.text = "";
        timer.text = "";

        if (handle != null)
        {
            handle.localPosition = _initialLocalPosition;
            handle.localRotation = _initialLocalRotation;
        }
    }
}