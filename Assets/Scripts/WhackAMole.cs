using UnityEngine;
using System.Collections.Generic;

public class WhackAMole : CarnivalGame
{
    private MoleSpawner _moleSpawner;
    public Scoreboard Scoreboard;

    protected override void Start()
    {
        base.Start();
        MoleController.MoleHit += IncreaseScore;
    }

    private void OnDestroy()
    {
        MoleController.MoleHit -= IncreaseScore;
    }

    protected override void StartGameLogic()
    {
        _moleSpawner = GetComponentInChildren<MoleSpawner>();
        if (_moleSpawner == null)
        {
            Debug.LogError("WhackAMole: no MoleSpawner found in children.", this);
            return;
        }
        _moleSpawner.StartGame();
    }

    protected override void EndGame()
    {
        base.EndGame();
        if (_moleSpawner != null)
            _moleSpawner.StopGame();
            HighscoreManager.Instance.SubmitScore(HighscoreManager.WHACK_A_MOLE, score);
            Scoreboard?.DisplayScores();
    }

    protected override void Update()
    {
        base.Update();
    }

}
