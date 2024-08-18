using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float duration = 2.0f;

    private float removalTime = 0.0f;
    private readonly string[] goodMessages = new[] { "Cool", "Nice", "Super", "Great!" };

    private readonly string[] greatMessages = new[] { "Wow!", "Massive!", "Awesome!", "Spectacular!" };
    void Start()
    {
        text.enabled = false;
    }

    void Update()
    {
        text.enabled = Time.time < removalTime;
        if (removalTime - Time.time < 0.5 * duration) {
            text.color = text.color.WithAlpha((removalTime - Time.time) / (0.5f * duration));
        }
    }

    public void BreakingTiles((int, int) info)
    {
        var (_, encirclement_count) = info;

        if (encirclement_count >= 8 && removalTime < Time.time)
        {
            removalTime = Time.time + duration;
            text.text = goodMessages[Random.Range(0, goodMessages.Length)];
        }

        if (encirclement_count >= 20 && removalTime < Time.time)
        {
            removalTime = Time.time + duration;
            text.text = greatMessages[Random.Range(0, greatMessages.Length)];
        }
    }
}
