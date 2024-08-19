using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectScore : MonoBehaviour
{
    public TextMeshProUGUI text;

    [SerializeField]
    private int score = 0;

    public float pulseRate = 1.0f;
    public float bigScorePulseSize = 2.0f;
    public float smallScorePulseSize = 1.2f;

    public float targetSize = 1.0f;
    public float currentSize = 1.0f;


    private void Start()
    {
        UpdateScore();
    }

    private void Update()
    {
        currentSize = Mathf.MoveTowards(currentSize,
            targetSize, pulseRate * Time.deltaTime);
        text.transform.localScale = Vector3.one * currentSize;
    }

    private void UpdateScore()
    {
        text.text = $"{score:n0}";
    }

    public void BreakingTiles((int, int) info)
    {
        var (count, encirclement_count) = info;
        score += 1000 * (count + encirclement_count * encirclement_count);
        UpdateScore();

        if (encirclement_count > 5)
        {
            currentSize = bigScorePulseSize;
            pulseRate = 2;
        }
        else
        {
            currentSize = smallScorePulseSize;
            pulseRate = 1;
        }
    }

    public void TilesAdded(int count)
    {
        score += 100 * count;
        UpdateScore();
        currentSize = smallScorePulseSize;
        pulseRate = 1;
    }

    private void TransitionToGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void Dead()
    {
        PlayerPrefs.SetInt(GameOverController.lastScoreKey, score);
        PlayerPrefs.Save();
        Debug.Log($"Saved score {score}");

        Invoke(nameof(TransitionToGameOver), 0.5f);
    }
}
