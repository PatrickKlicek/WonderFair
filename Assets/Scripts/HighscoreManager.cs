using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class HighscoreEntry
{
    public int score;
}

[System.Serializable]
public class HighscoreData
{
    public List<HighscoreEntry> entries = new List<HighscoreEntry>();
}

public class HighscoreManager : MonoBehaviour
{
    public static HighscoreManager Instance { get; private set; }

    public const string DUCK_SHOOTING = "DuckShooting";
    public const string BUZZWIRE = "Buzzwire";
    public const string CAN_SMASH = "CanSmash";
    public const string WHACK_A_MOLE = "WhackAMole";

    private const int MAX_ENTRIES = 10;

    private Dictionary<string, HighscoreData> _cache = new Dictionary<string, HighscoreData>();

    public bool IsLoaded { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadAll();
    }

    // ----------------------------------------------------------------
    // LOAD
    // ----------------------------------------------------------------

    private void LoadAll()
    {
        string[] games = { DUCK_SHOOTING, BUZZWIRE, CAN_SMASH, WHACK_A_MOLE };

        foreach (string game in games)
            LoadHighscore(game);

        IsLoaded = true;
        Debug.Log("Svi highscoreovi uèitani.");
    }

    private void LoadHighscore(string gameName)
    {
        string path = GetPath(gameName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            _cache[gameName] = JsonUtility.FromJson<HighscoreData>(json);
            Debug.Log($"Uèitan highscore za {gameName}: {_cache[gameName].entries.Count} unosa");
        }
        else
        {
            //File jos ne postoji, kreiraj prazan i odmah spremi
            _cache[gameName] = new HighscoreData();
            SaveHighscore(gameName);
            Debug.Log($"Kreiran novi highscore file za {gameName}");
        }
    }

    // ----------------------------------------------------------------
    // GET
    // ----------------------------------------------------------------

    public List<HighscoreEntry> GetHighscores(string gameName)
    {
        if (_cache.ContainsKey(gameName))
            return _cache[gameName].entries;

        return new List<HighscoreEntry>();
    }

    public bool Qualifies(string gameName, int newScore)
    {
        if (!_cache.ContainsKey(gameName)) return true;

        List<HighscoreEntry> entries = _cache[gameName].entries;
        return entries.Count < MAX_ENTRIES ||
               newScore > entries[entries.Count - 1].score;
    }

    // ----------------------------------------------------------------
    // SUBMIT
    // ----------------------------------------------------------------

    public bool SubmitScore(string gameName, int newScore)
    {
        if (!_cache.ContainsKey(gameName))
            _cache[gameName] = new HighscoreData();

        HighscoreData data = _cache[gameName];

        bool qualifies = data.entries.Count < MAX_ENTRIES ||
                         newScore > data.entries[data.entries.Count - 1].score;

        if (!qualifies) return false;

        data.entries.Add(new HighscoreEntry { score = newScore });
        data.entries.Sort((a, b) => b.score.CompareTo(a.score));

        if (data.entries.Count > MAX_ENTRIES)
            data.entries.RemoveRange(MAX_ENTRIES, data.entries.Count - MAX_ENTRIES);

        SaveHighscore(gameName);
        return true;
    }

    // ----------------------------------------------------------------
    // SAVE
    // ----------------------------------------------------------------

    private void SaveHighscore(string gameName)
    {
        string folder = Path.Combine(Application.persistentDataPath, "Highscore");
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        string path = GetPath(gameName);
        string json = JsonUtility.ToJson(_cache[gameName], true);
        File.WriteAllText(path, json);

        Debug.Log($"Highscore za {gameName} spremljen.");
    }

    // ----------------------------------------------------------------
    // HELPER
    // ----------------------------------------------------------------

    private string GetPath(string gameName)
    {
        return Path.Combine(Application.persistentDataPath, "Highscore", gameName + ".json");
    }
}