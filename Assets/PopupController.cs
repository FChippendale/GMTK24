using TMPro;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float duration = 2.0f;

    private float removalTime = 0.0f;
    private readonly string[] messages = new[] { "Nice!", "Well done!", "Super!",
                                        "Awesome!", "Keep it up!", "Great!" };

    void Start()
    {
        text.enabled = false;
    }

    void Update()
    {
        text.enabled = Time.time < removalTime;
    }

    public void BreakingTiles((int, int) info)
    {
        var (_, encirclement_count) = info;

        if (encirclement_count >= 8 && removalTime < Time.time)
        {
            removalTime = Time.time + duration;
            text.text = messages[Random.Range(0, messages.Length)];
        }

    }
}
