using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

struct LeaderboardEntry
{
    public string name;
    public int score;
}

public class GameOverController : MonoBehaviour
{

    public Button playButton;
    public Button quitButton;
    public TMP_InputField inputField;
    public TextMeshProUGUI scoreField;
    public List<TextMeshProUGUI> leaderboardNames;
    public List<TextMeshProUGUI> leaderboardScores;

    private List<LeaderboardEntry> leaderboard = new();
    private readonly int score = 0;

    private static readonly string lastScoreKey = "LastScore";
    private static readonly string leaderboardKey = "Leaderboard";

    // Start is called before the first frame update
    void Start()
    {
        int score = PlayerPrefs.GetInt(lastScoreKey, 0);

        scoreField.text = score.ToString();

        if (PlayerPrefs.HasKey(leaderboardKey))
        {
            string serialized = PlayerPrefs.GetString(leaderboardKey);
            leaderboard = JsonUtility.FromJson<List<LeaderboardEntry>>(serialized);
        }

        int entries = Math.Min(leaderboardNames.Count,
            Math.Min(leaderboardScores.Count, leaderboard.Count));
        for (int i = 0; i < entries; ++i)
        {
            leaderboardScores[i].text = leaderboard[i].score.ToString();
            leaderboardNames[i].text = leaderboard[i].name;
        }

        playButton.onClick.AddListener(() =>
        {
            AddToLeaderboard();
            SceneManager.LoadScene(leaderboardKey);
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

        leaderboard.Add(entry);
        leaderboard.Sort(Comparer<LeaderboardEntry>.Create(
            (LeaderboardEntry lhs, LeaderboardEntry rhs) =>
                {
                    return lhs.score - rhs.score;
                }));

        PlayerPrefs.SetString(leaderboardKey, JsonUtility.ToJson(leaderboard));
        PlayerPrefs.Save();
    }
}
