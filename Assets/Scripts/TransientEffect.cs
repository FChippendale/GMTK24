using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TransientEffect : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1.0f);
    }
}
