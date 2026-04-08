using System.Diagnostics;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    public int score = 0;
    [HideInInspector] public bool inGame = false;
    [HideInInspector] public Stopwatch timer = new();

    void Awake()
    {
        if (GameManager.GM != null)
            Destroy(gameObject);
        else
        {
            GameManager.GM = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
