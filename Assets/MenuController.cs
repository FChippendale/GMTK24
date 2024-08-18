using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Button playButton;
    public Button helpButton;
    public Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));
        // TODO helpButton
        quitButton.onClick.AddListener(() => Application.Quit());
    }
}
