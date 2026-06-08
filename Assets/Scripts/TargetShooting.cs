using UnityEngine;
using System.Collections.Generic;

public class TargetShooting : CarnivalGame
{
    [Header("Game specific")]
    public List<Transform> rows = new();
    public List<MovingTargetSpawner> movingTargetSpawners = new();
    public float targetRaiseIntervalStart = 3;
    public float targetSpeedupInterval = 15;
    public float targetSpeedupIncrement = 0.25f;
    public float movingTargetStartTime = 50;
    public float movingTargetRaiseIntervalStart = 5;
    public float movingTargetSpeedupInterval = 10;
    public float movingTargetSpeedupIncrement = 0.5f;
    public Scoreboard Scoreboard;

    private List<Target> targets = new();
    private float targetRaiseInterval;
    private float targetRaiseTimestamp = 0;
    private float targetSpeedupTimestamp = 0;
    private float movingTargetRaiseInterval;
    private float movingTargetRaiseTimestamp = 0;
    private float movingTargetSpeedupTimestamp = 0;

    protected override void Start()
    {
        base.Start();

        foreach (Transform row in rows)
        {
            for (int i = 1; i < row.childCount; i++)
            {
                targets.Add(row.GetChild(i).GetComponent<Target>());
            }
        }

        Target.TargetHit += IncreaseScore;
    }

    protected override void Update()
    {
        base.Update();

        if (isRunning)
        {
            // Increment speed at which targets appear as game goes on
            if (Time.time - targetSpeedupTimestamp > targetSpeedupInterval)
            {
                targetSpeedupTimestamp = Time.time;
                targetRaiseInterval -= targetSpeedupIncrement;
            }
            if (GameManager.GM.timer.ElapsedMilliseconds / 1000 > movingTargetStartTime &&
                Time.time - movingTargetSpeedupTimestamp > movingTargetSpeedupInterval)
            {
                movingTargetSpeedupTimestamp = Time.time;
                movingTargetRaiseInterval -= movingTargetSpeedupIncrement;
            }

            // Raise random available target according to target raise interval
            if (Time.time - targetRaiseTimestamp > targetRaiseInterval)
            {
                targetRaiseTimestamp = Time.time;
                List<Target> availableTargets = targets.FindAll(t => t.raiseTimestamp == 0);
                availableTargets[Random.Range(0, availableTargets.Count)].RaiseTarget();
            }

            // After moving target start time, spawn random moving target according to moving target spawn interval
            if (GameManager.GM.timer.ElapsedMilliseconds / 1000 > movingTargetStartTime &&
                Time.time - movingTargetRaiseTimestamp > movingTargetRaiseInterval)
            {
                movingTargetRaiseTimestamp = Time.time;
                movingTargetSpawners[Random.Range(0, movingTargetSpawners.Count)].Spawn();
            }
        }
    }

    protected override void StartGameLogic()
    {
        targetRaiseInterval = targetRaiseIntervalStart;
        movingTargetRaiseInterval = movingTargetRaiseIntervalStart;
        targetSpeedupTimestamp = Time.time;
        movingTargetSpeedupTimestamp = Time.time;
    }

    protected override void EndGame()
    {
        base.EndGame();
        HighscoreManager.Instance.SubmitScore(HighscoreManager.DUCK_SHOOTING, score);
        Scoreboard?.DisplayScores();

        foreach (Target target in targets)
        {
            target.LowerTarget();
        }
        foreach (MovingTargetSpawner mtSpawner in movingTargetSpawners)
        {
            for (int i = 3; i < mtSpawner.transform.childCount; i++)
            {
                mtSpawner.transform.GetChild(i).GetComponent<Target>().LowerTarget();
            }
        }
    }

    void OnDestroy()
    {
        Target.TargetHit -= IncreaseScore;
    }
}
