using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float lastEvent;

    public float intervalBetweenEvents = 10.0f;

    public string message = "TimerTick";

    public TextMeshProUGUI ui;

    // Start is called before the first frame update
    void Start()
    {
        lastEvent = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastEvent > intervalBetweenEvents)
        {
            gameObject.SendMessage(message);
            lastEvent = Time.time;
        }

        ui.SetText("{0:1} s", lastEvent + intervalBetweenEvents - Time.time);
    }
}
