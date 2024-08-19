using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonSprite : MonoBehaviour
{
    public Sprite soundOn;
    public Sprite soundOff;
    public Image buttonImage;

    bool muted = false;

    public void Start()
    {
        GameObject music = GameObject.FindGameObjectWithTag("music");
        if (music != null)
        {
            SetTo(music.GetComponent<AudioSource>().mute);
        }
    }

    public void ToggleMuteSprite()
    {
        SetTo(!muted);
    }

    private void SetTo(bool state)
    {
        muted = state;
        buttonImage.sprite = muted ? soundOff : soundOn;
    }
}
