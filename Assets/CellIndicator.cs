using Unity.VisualScripting;
using UnityEngine;

public class CellIndicator : MonoBehaviour
{
    enum State
    {
        free,
        invalid_animation_1,
        invalid_animation_2,
    }

    public float invalidTime1 = 0.4f;
    public float invalidTime2 = 0.3f;

    public static readonly Color invalidColour = new Color32(229, 80, 57, 255); // Mandarin red
    private Color freeColour = Color.black;
    private Vector3 position = new Vector3(200, 200, 0);
    private State state = State.free;
    private float timeInState = 0.0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.transform.position = position;

        switch (state)
        {
            case State.free:
                {
                    spriteRenderer.material.color = freeColour;
                    break;
                }
            case State.invalid_animation_1:
                {
                    float alpha = Mathf.Lerp(1.0f, 0.0f, timeInState / invalidTime1);
                    spriteRenderer.material.color = invalidColour.WithAlpha(alpha);

                    timeInState += Time.deltaTime;
                    if (timeInState >= invalidTime1)
                    {
                        timeInState = 0.0f;
                        state = State.invalid_animation_2;
                    }

                    break;
                }
            case State.invalid_animation_2:
                {
                    float alpha = Mathf.Lerp(0.0f, 1.0f, timeInState / invalidTime2);
                    spriteRenderer.material.color = freeColour.WithAlpha(alpha);

                    timeInState += Time.deltaTime;
                    if (timeInState >= invalidTime2)
                    {
                        timeInState = 0.0f;
                        state = State.free;
                    }

                    break;
                }
        }
    }

    public void Reposition(Vector3 position, Color colour)
    {
        freeColour = colour;

        // Move sprite back a bit so it renders on the camera
        this.position = position + new Vector3(0, 0, 1);
    }

    public void StartInvalidAnimation()
    {
        if (state != State.invalid_animation_1)
        {
            state = State.invalid_animation_1;
            timeInState = 0.0f;
        }
    }
}
