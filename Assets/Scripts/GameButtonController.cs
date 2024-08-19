using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtonController : MonoBehaviour
{
    public AudioSource audioSource;

    public void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ToggleMute()
    {
        audioSource.mute = !audioSource.mute;
    }
}
