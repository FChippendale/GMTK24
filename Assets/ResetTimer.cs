using UnityEngine;

public class ResetTimer : MonoBehaviour
{
    public Timer timer;

    public void TilesAdded()
    {
        timer.Reset();
    }
}
