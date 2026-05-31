using Oculus.Interaction.Locomotion;
using System;
using TMPro;
using UnityEngine;

public abstract class CarnivalGame : MonoBehaviour
{
    [Header("General")]
    public int gameDuration = 120;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI scoreboard;
    public TeleportInteractable teleportScript;

    protected TimeSpan gameDurationTimespan;
    protected TimeSpan timeLeft;
    protected bool isRunning = false;
    protected int score = 0;

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
            if (timeLeft.TotalSeconds < 10)
                timer.text = string.Format("{0:0}.{1:0}", timeLeft.Seconds, timeLeft.Milliseconds / 100);
            else if (timeLeft.TotalSeconds < 60)
                timer.text = string.Format("{0:00}.{1:0}", timeLeft.Seconds, timeLeft.Milliseconds / 100);
            else
                timer.text = string.Format("{0:0}:{1:00}", timeLeft.Minutes, timeLeft.Seconds);
            scoreboard.text = score.ToString();
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
        }

        StartGameLogic();
    }
    protected virtual void StartGameLogic()
    {
        return;
    }

    protected virtual void EndGame()
    {
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
}
