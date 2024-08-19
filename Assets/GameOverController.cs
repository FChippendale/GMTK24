using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
struct LeaderboardEntry
{
    public string name;
    public int score;
}

[Serializable]
struct Leaderboard
{
    public List<LeaderboardEntry> entries;
}

public class GameOverController : MonoBehaviour
{

    public Button playButton;
    public Button quitButton;
    public TMP_InputField inputField;
    public TextMeshProUGUI scoreField;
    public List<TextMeshProUGUI> leaderboardNames;
    public List<TextMeshProUGUI> leaderboardScores;

    private Leaderboard leaderboard = new();
    private int score = 0;

    public static readonly string lastScoreKey = "LastScore";
    public static readonly string leaderboardKey = "Leaderboard";

    // Start is called before the first frame update
    void Start()
    {
        score = PlayerPrefs.GetInt(lastScoreKey, 0);

        scoreField.text = $"{score:n0}";

        if (PlayerPrefs.HasKey(leaderboardKey))
        {
            string serialized = PlayerPrefs.GetString(leaderboardKey);
            Debug.Log($"Loading leaderboard: {serialized}");
            leaderboard = JsonUtility.FromJson<Leaderboard>(serialized);
        } else {
            leaderboard.entries = new List<LeaderboardEntry>();
        }

        foreach (var n in leaderboardNames)
        {
            n.text = "...";
        }

        foreach (var s in leaderboardScores)
        {
            s.text = "...";
        }

        int entries = Math.Min(leaderboardNames.Count,
            Math.Min(leaderboardScores.Count, leaderboard.entries.Count));
        for (int i = 0; i < entries; ++i)
        {
            leaderboardScores[i].text = $"{leaderboard.entries[i].score:n0}";
            leaderboardNames[i].text = leaderboard.entries[i].name;
        }

        playButton.onClick.AddListener(() =>
        {
            AddToLeaderboard();
            SceneManager.LoadScene("SampleScene");
        });

        quitButton.onClick.AddListener(() =>
        {
            AddToLeaderboard();
            Application.Quit();
        });
    }

    void AddToLeaderboard()
    {
        LeaderboardEntry entry = new()
        {
            name = inputField.text,
            score = score
        };

        leaderboard.entries.Add(entry);
        leaderboard.entries.Sort(Comparer<LeaderboardEntry>.Create(
            (LeaderboardEntry lhs, LeaderboardEntry rhs) =>
                {
                    return rhs.score - lhs.score;
                }));

        string serialized = JsonUtility.ToJson(leaderboard);
        PlayerPrefs.SetString(leaderboardKey, serialized);
        PlayerPrefs.Save();
        Debug.Log($"Saved leaderboard: {serialized} (from {leaderboard})");
    }
}
