using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button playButton;
    public Button leaderboardButton;
    public Button quitButton;

    public Button easyButton;
    public Button hardButton;

    void Start()
    {
        // With the null checks, we can reuse this code in both the main menu
        // and the help screen.
        if (playButton != null)
        {
            playButton.onClick.AddListener(() => SceneManager.LoadScene("Help"));
        }

        if (leaderboardButton != null)
        {
            leaderboardButton.onClick.AddListener(() => SceneManager.LoadScene("Leaderboard"));
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(() => Application.Quit());
        }

        if (easyButton != null)
        {
            PlayerPrefs.SetInt("number_of_colors", 3);
            easyButton.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));
        }
        if (hardButton != null)
        {
            PlayerPrefs.SetInt("number_of_colors", 4);
            hardButton.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));
        }
    }
}
