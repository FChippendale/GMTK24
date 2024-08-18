using TMPro;
using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TriggerSFX triggerSFX;

    float lastEvent;
    float lastTick = 0f;

    public float intervalBetweenEvents = 8.0f;

    public string message = "TimerTick";

    public TextMeshProUGUI ui;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {

        if (Math.Floor(Time.time) > Math.Floor(lastTick) && Time.time - lastEvent > intervalBetweenEvents - 3.0f)
        {
            triggerSFX.PlaySound(TriggerSFX.SoundType.timer_tick);
        }
        lastTick = Time.time;

        if (Time.time - lastEvent > intervalBetweenEvents)
        {
            gameObject.SendMessage(message);
            triggerSFX.PlaySound(TriggerSFX.SoundType.timer_zero);
            Reset();
        }

        ui.SetText("{0:1} s", lastEvent + intervalBetweenEvents - Time.time);
    }

    public void Reset()
    {
        lastEvent = Time.time;
    }
}
