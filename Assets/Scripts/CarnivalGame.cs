using Oculus.Interaction.Locomotion;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public abstract class CarnivalGame : MonoBehaviour
{
    [Header("General")]
    public int gameDuration = 120;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI scoreboard;
    public TeleportInteractable teleportScript;
    public AudioSource audioSource;
    public AudioClip startSound;
    public AudioClip endSound;
    public AudioClip notificationSound;
    public AudioClip countdownSound;

    protected TimeSpan gameDurationTimespan;
    protected TimeSpan timeLeft;
    protected bool isRunning = false;
    protected int score = 0;

    public          TimeSpan TimeLeft  => isRunning ? timeLeft : gameDurationTimespan;
    public virtual  bool     IsRunning => isRunning;
    public virtual  int      Score     => score;
    private bool _halfTimeSoundPlayed = false;
    private bool _countdownStarted = false;
    private bool _startSoundPlayed = false;
    private bool _endSoundPlayed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        gameDurationTimespan = new TimeSpan(0, gameDuration / 60, gameDuration % 60);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isRunning)
        {
            if (GameManager.GM.timer.Elapsed > gameDurationTimespan)
            {
                EndGame();
                return;
            }

            timeLeft = gameDurationTimespan - GameManager.GM.timer.Elapsed;

            int secondsLeft = (int)Math.Ceiling(timeLeft.TotalSeconds);
            if (!_halfTimeSoundPlayed && secondsLeft == gameDuration / 2)
            {
                _halfTimeSoundPlayed = true;
                PlayNotificationSound();
            }

            if (!_countdownStarted && secondsLeft == 10)
            {
                _countdownStarted = true;
                StartCountdown(() => { });
            }

            if (timer != null)
            {
                if (timeLeft.TotalSeconds < 10)
                    timer.text = string.Format("{0:0}.{1:0}", timeLeft.Seconds, timeLeft.Milliseconds / 100);
                else if (timeLeft.TotalSeconds < 60)
                    timer.text = string.Format("{0:00}.{1:0}", timeLeft.Seconds, timeLeft.Milliseconds / 100);
                else
                    timer.text = string.Format("{0:0}:{1:00}", timeLeft.Minutes, timeLeft.Seconds);
            }
            if (scoreboard != null) scoreboard.text = score.ToString();
        }
    }

    [ContextMenu("Start game")]
    public void StartGame()
    {
        if (!GameManager.GM.inGame)
        {
            score = 0;
            isRunning = true;
            GameManager.GM.inGame = true;
            GameManager.GM.timer.Restart();
            teleportScript.AllowTeleport = false;
            _halfTimeSoundPlayed = false;
            _countdownStarted = false;
            _startSoundPlayed = false;
            _endSoundPlayed = false;
        }
        PlayStartSound();
        StartGameLogic();
    }
    protected virtual void StartGameLogic()
    {
        return;
    }

    protected virtual void EndGame()
    {
        PlayEndSound();
        isRunning = false;
        GameManager.GM.inGame = false;
        teleportScript.AllowTeleport = true;
        //GameManager.GM.score += score;
    }

    protected void IncreaseScore(int value)
    {
        if (isRunning)
            score += value;
    }

    protected void PlayStartSound()
    {
        if (_startSoundPlayed) return;
        _startSoundPlayed = true;
        if (audioSource != null && startSound != null)
            audioSource.PlayOneShot(startSound);
    }

    protected void PlayEndSound()
    {
        if (_endSoundPlayed) return;
        _endSoundPlayed = true;
        if (audioSource != null && endSound != null)
            audioSource.PlayOneShot(endSound);
    }

    protected void PlayNotificationSound()
    {
        if (audioSource != null && notificationSound != null)
            audioSource.PlayOneShot(notificationSound);
    }

    protected void StartCountdown(Action onCountdownEnd)
    {
        StartCoroutine(CountdownCoroutine(onCountdownEnd));
    }

    private IEnumerator CountdownCoroutine(Action onCountdownEnd)
    {
        for (int i = 10; i > 0; i--)
        {
            if (audioSource != null && countdownSound != null)
                audioSource.PlayOneShot(countdownSound);

            yield return new WaitForSeconds(1f);
        }

        onCountdownEnd?.Invoke();
    }
}
