using UnityEngine;

public class ResetTimer : MonoBehaviour
{
    public Timer timer;

    public void FactoryAdded()
    {
        timer.Reset();
    }
}
