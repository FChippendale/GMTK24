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

    public float targetSize = 1.0f;
    public float currentSize = 1.0f;
    public float pulseRate = 1.0f;

    public string message = "TimerTick";

    public TextMeshProUGUI ui;

    private float timeSinceStart = 0;

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
            currentSize = 1.5f;
            triggerSFX.PlaySound(TriggerSFX.SoundType.timer_tick);
            ui.color = new Color32(229, 80, 57, 255);
        }
        lastTick = Time.time;

        if (Time.time - lastEvent > intervalBetweenEvents)
        {
            gameObject.SendMessage(message);
            triggerSFX.PlaySound(TriggerSFX.SoundType.timer_zero);
            Reset();
        }

        ui.SetText("{0:1} s", lastEvent + intervalBetweenEvents - Time.time);

        currentSize = Mathf.MoveTowards(currentSize,
            targetSize, pulseRate * Time.deltaTime);
        ui.transform.localScale = Vector3.one * currentSize;

        timeSinceStart += Time.deltaTime;
    }

    public void Reset()
    {
        lastEvent = Time.time;
        currentSize = 1.0f;
        ui.color = Color.black;
        intervalBetweenEvents = 4 + 4 * Mathf.Exp(-0.005f * timeSinceStart);
    }
}
