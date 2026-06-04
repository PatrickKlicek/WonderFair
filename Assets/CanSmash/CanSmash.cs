using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CanSmash : CarnivalGame
{
    [Header("Game specific")]
    public Transform canParent;
    public Rigidbody[] cans;

    public Rigidbody[] balls;
    private Vector3[] _ballInitialPos;
    private Quaternion[] _ballInitialRot;

    public float xVariance = 0.5f;
    public float yVariance = 0.5f;

    public TextMeshProUGUI finalScoreText;
    public Scoreboard Scoreboard;

    public bool IsRunning => isRunning;
    public FloatingScorePopup scorePopup;
    public Vector3 popupOffset = new Vector3(0, 0.3f, 0);

    private int _ballsThrown;
    private int _cansKnocked;
    private int _ballsOnGround;

    private Vector3[] _canInitialLocalPos;
    private Quaternion[] _canInitialLocalRot;
    private Vector3 _setupInitialLocalPos;

    private bool _isResetting = false;
    public HashSet<int> _fallenCans = new HashSet<int>();

    protected override void Start()
    {
        base.Start();

        _canInitialLocalPos = new Vector3[cans.Length];
        _canInitialLocalRot = new Quaternion[cans.Length];
        for (int i = 0; i < cans.Length; i++)
        {
            _canInitialLocalPos[i] = cans[i].transform.localPosition;
            _canInitialLocalRot[i] = cans[i].transform.localRotation;
        }

        _ballInitialPos = new Vector3[balls.Length];
        _ballInitialRot = new Quaternion[balls.Length];
        for (int i = 0; i < balls.Length; i++)
        {
            _ballInitialPos[i] = balls[i].transform.position;
            _ballInitialRot[i] = balls[i].transform.rotation;
        }

        _setupInitialLocalPos = canParent.localPosition;

        if (finalScoreText != null)
            finalScoreText.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        if (isRunning)
            scoreboard.text = _cansKnocked.ToString();
    }

    protected override void StartGameLogic()
    {
        if (isRunning) return;

        _ballsThrown = 0;
        _cansKnocked = 0;
        _ballsOnGround = 0;

        if (finalScoreText != null)
            finalScoreText.gameObject.SetActive(false);

        ResetCans(randomOffset: false);
    }

    public void OnCanFallen(int canIndex, Vector3 pos)
    {
        if (!isRunning || _isResetting) return;

        if (_fallenCans.Contains(canIndex)) return;

        _fallenCans.Add(canIndex);
        _cansKnocked++;
        if (scorePopup != null)
        {
            var popup = Instantiate(scorePopup, pos + popupOffset, Quaternion.identity);
            popup.Init(1);
        }

        if (_fallenCans.Count >= cans.Length)
            StartCoroutine(ResetRound(resetBalls: true));
    }

    public void OnBallHitGround()
    {
        if (!isRunning || _isResetting) return;

        _ballsOnGround++;

        if (_ballsOnGround >= balls.Length)
            StartCoroutine(ResetRound(resetBalls: true));
    }

    private IEnumerator ResetRound(bool resetBalls)
    {
        _isResetting = true;
        yield return new WaitForSeconds(3f);

        if (!isRunning)
        {
            _isResetting = false;
            yield break;
        }

        ResetCans(randomOffset: true);

        _fallenCans.Clear();
        _isResetting = false;
    }

    private void ResetCans(bool randomOffset)
    {
        if (randomOffset)
        {
            canParent.localPosition = _setupInitialLocalPos + new Vector3(
                Random.Range(-xVariance, xVariance),
                Random.Range(-yVariance, yVariance),
                0f
            );
        }
        else
        {
            canParent.localPosition = _setupInitialLocalPos;
        }

        for (int i = 0; i < cans.Length; i++)
        {
            cans[i].linearVelocity = Vector3.zero;
            cans[i].angularVelocity = Vector3.zero;
            cans[i].transform.localPosition = _canInitialLocalPos[i];
            cans[i].transform.localRotation = _canInitialLocalRot[i];
        }
    }

    private void ResetBalls()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].linearVelocity = Vector3.zero;
            balls[i].angularVelocity = Vector3.zero;

            balls[i].Sleep();

            balls[i].transform.position = _ballInitialPos[i];
            balls[i].transform.rotation = _ballInitialRot[i];

            balls[i].WakeUp();
            balls[i].isKinematic = true;
            balls[i].isKinematic = false;
            balls[i].useGravity = false;
        }
    }

    protected override void EndGame()
    {
        base.EndGame();
        HighscoreManager.Instance.SubmitScore(HighscoreManager.CAN_SMASH, _cansKnocked);
        Scoreboard?.DisplayScores();

        if (finalScoreText != null)
        {
            finalScoreText.gameObject.SetActive(true);
            finalScoreText.text = $"{_cansKnocked}";
        }

        StartCoroutine(AutoReset());
    }

    private IEnumerator AutoReset()
    {
        yield return new WaitForSeconds(10f);

        if (finalScoreText != null)
            finalScoreText.gameObject.SetActive(false);

        ResetCans(randomOffset: false);
        _ballsThrown = 0;
        _cansKnocked = 0;
        _ballsOnGround = 0;
        _fallenCans.Clear();
        _isResetting = false;
        scoreboard.text = "0";
    }

    public int GetCanIndex(Rigidbody rb)
    {
        for (int i = 0; i < cans.Length; i++)
            if (cans[i] == rb) return i;
        return -1;
    }
}