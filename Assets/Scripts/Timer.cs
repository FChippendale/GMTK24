using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float lastEvent;

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
        if (Time.time - lastEvent > intervalBetweenEvents)
        {
            gameObject.SendMessage(message);
            Reset();
        }

        ui.SetText("{0:1} s", lastEvent + intervalBetweenEvents - Time.time);
    }

    public void Reset()
    {
        lastEvent = Time.time;
    }
}
