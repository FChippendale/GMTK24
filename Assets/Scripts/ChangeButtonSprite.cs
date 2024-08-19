using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonSprite : MonoBehaviour
{
    public Sprite soundOn;
    public Sprite soundOff;
    public Image buttonImage;

    bool muted = false;
    
    public void ToggleMuteSprite()
    {
        muted = !muted;
        if (muted)
        {
            buttonImage.sprite = soundOff;
        }
        else
        {
            buttonImage.sprite = soundOn;
        }
    }
}
