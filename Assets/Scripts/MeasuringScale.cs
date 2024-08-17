using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MeasuringScale : MonoBehaviour
{
    public float lhs = 1;
    public float rhs = 1;
    public float Stickiness = 1;

    public Transform Beam;
    public Transform BeamRHS;
    public Transform BeamLHS;

    public Transform BucketRHS;
    public Transform BucketLHS;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float diff = lhs - rhs;
        float normalizedDiff = diff / (Math.Min(Math.Abs(lhs), Math.Abs(rhs)) + 1);
        float angle = (float)(180 * Math.Atan(normalizedDiff / Stickiness) / Math.PI);

        Beam.localEulerAngles = new Vector3(0, 0, angle);
        BucketRHS.position = BeamRHS.position;
        BucketLHS.position = BeamLHS.position;
    }
}
