using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
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

    private Animator   _animator;
    private Coroutine  _fallCoroutine;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // called by MoleSpawner
    public void Activate(float activeTime)
    {
        IsWhacked = false;
        IsUp = true;
        _animator.SetTrigger(HashRise);

        // automatic retreat after activeTime
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
        MoleHit?.Invoke(points);
    }

    public void OnRiseStart()  { }

    public void OnRiseEnd()    { IsUp = true;  _animator.SetBool(HashIsUp, true); }

    public void OnFallStart()  { IsUp = false; _animator.SetBool(HashIsUp, false); }

    public void OnFallEnd()    { IsWhacked = false; }

    IEnumerator AutoFall(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!IsWhacked)
            _animator.SetTrigger(HashFall);
    }
}