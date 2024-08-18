using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button playButton;
    public Button helpButton;
    public Button quitButton;

    void Start()
    {
        // With the null checks, we can reuse this code in both the main menu
        // and the help screen.
        if (playButton != null)
        {
            playButton.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));
        }

        if (helpButton != null)
        {
            helpButton.onClick.AddListener(() => SceneManager.LoadScene("Help"));
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(() => Application.Quit());
        }
    }
}
