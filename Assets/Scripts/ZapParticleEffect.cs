using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using System.Collections;

public class ZapParticleEffect : MonoBehaviour
{
    private Vector3 normalPosition;

    float timeToHidden = 0.0f;
    public float ZapDuration = 1.0f;


    IEnumerator zapper(Vector3 position)
    {
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.01f);
        transform.position = position;
    }

    public void ZapTo(Vector3 position)
    {
        timeToHidden = ZapDuration;
        StartCoroutine(zapper(position));
    }

    public void Start()
    {
        normalPosition = transform.position;
        GetComponent<ParticleSystem>().Stop();
    }

    public void Update()
    {
        timeToHidden -= Time.deltaTime;
        if (timeToHidden < 0)
        {
            GetComponent<ParticleSystem>().Stop();
            transform.position = normalPosition;
        }
    }
}
