using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransientEffect : MonoBehaviour
{
    public float ttl = 1.0f;
    
    void Start()
    {
        Destroy(gameObject, ttl);
    }
}
