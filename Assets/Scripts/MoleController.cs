using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class MoleController : MonoBehaviour
{
    static readonly int HashRise  = Animator.StringToHash("Rise");
    static readonly int HashFall  = Animator.StringToHash("Fall");
    static readonly int HashWhack = Animator.StringToHash("Whack");
    static readonly int HashIsUp  = Animator.StringToHash("IsUp");

    public bool IsUp      { get; private set; }
    public bool IsWhacked { get; private set; }

    public static event Action<int> MoleHit;
    public int points;

    [Header("Sounds")]
    public AudioClip          spawnSound;
    public AudioClip          whackSound;

    [Header("Hit Feedback")]
    public ParticleSystem     hitParticles;
    public FloatingScorePopup scorePopup;
    public Vector3            popupOffset = new Vector3(0, 0.3f, 0);

    private Animator    _animator;
    private AudioSource _audio;
    private Coroutine   _fallCoroutine;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _audio    = GetComponent<AudioSource>();
    }

    // called by MoleSpawner
    public void Activate(float activeTime)
    {
        IsWhacked = false;
        IsUp = true;
        _animator.SetTrigger(HashRise);

        if (spawnSound != null)
            _audio.PlayOneShot(spawnSound);
        else
            Debug.LogWarning("MoleController: spawnSound is not assigned!", this);
        Debug.Log($"MoleController Activate — AudioSource volume: {_audio.volume}, mute: {_audio.mute}, clip: {spawnSound}", this);

        if (_fallCoroutine != null) StopCoroutine(_fallCoroutine);
        _fallCoroutine = StartCoroutine(AutoFall(activeTime));
    }

    // called by Bonker
    public void Whack()
    {
        if (!IsUp || IsWhacked) return;

        IsWhacked = true;
        IsUp      = false;

        if (_fallCoroutine != null)
        {
            StopCoroutine(_fallCoroutine);
            _fallCoroutine = null;
        }

        _animator.SetTrigger(HashWhack);

        if (whackSound != null)
            _audio.PlayOneShot(whackSound);
        else
            Debug.LogWarning("MoleController: whackSound is not assigned!", this);
        Debug.Log($"MoleController Whack — AudioSource volume: {_audio.volume}, mute: {_audio.mute}, clip: {whackSound}", this);

        if (hitParticles != null)
            hitParticles.Play();

        if (scorePopup != null)
        {
            var popup = Instantiate(scorePopup, transform.position + popupOffset, Quaternion.identity);
            popup.Init(points);
        }

        MoleHit?.Invoke(points);
    }

    public void OnRiseStart() { }

    public void OnRiseEnd()   { if (!IsWhacked) { IsUp = true; _animator.SetBool(HashIsUp, true); } }

    public void OnFallStart() { IsUp = false; _animator.SetBool(HashIsUp, false); }

    public void OnFallEnd()   { IsWhacked = false; }

    IEnumerator AutoFall(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!IsWhacked)
            _animator.SetTrigger(HashFall);
    }
}