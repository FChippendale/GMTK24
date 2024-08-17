using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBoxTrigger : MonoBehaviour
{
    public bool isTaxMan = true;

    private ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    void OnParticleTrigger()
    {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, new List<ParticleSystem.Particle>());
        gameObject.SendMessageUpwards("MoneyCollected", (isTaxMan, numEnter));
    }
}
