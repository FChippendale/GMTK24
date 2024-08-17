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

    public GameObject MoneyParticleSystemObj;
    ParticleSystem moneyEmitter;

    public GameObject TaxParticleSystemObj;
    ParticleSystem taxEmitter;

    float lastAngle = 0;
    public float MaxAngleChangePerFrame = 1f;

    // Start is called before the first frame update
    void Start()
    {
        moneyEmitter = MoneyParticleSystemObj.GetComponent<ParticleSystem>();
        taxEmitter = TaxParticleSystemObj.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        float diff = lhs - rhs;
        float normalizedDiff = diff / (Math.Min(Math.Abs(lhs), Math.Abs(rhs)) + 1);
        float requestAngle = (float)(180 * Math.Atan(normalizedDiff / Stickiness) / Math.PI);

        if (Math.Abs(requestAngle - lastAngle) > MaxAngleChangePerFrame) {
            lastAngle += Math.Sign(requestAngle - lastAngle) * MaxAngleChangePerFrame;
        } else {
            lastAngle = requestAngle;
        }

        Beam.localEulerAngles = new Vector3(0, 0, lastAngle);

        BucketRHS.position = BeamRHS.position;
        TaxParticleSystemObj.transform.position = BeamRHS.position;

        BucketLHS.position = BeamLHS.position;
        MoneyParticleSystemObj.transform.position = BeamLHS.position;
    }

    public void FactoryScoreUpdate(int amount)
    {
        moneyEmitter.Emit(amount);
    }

    public void AddTax(int amount)
    {
        taxEmitter.Emit(amount);
    }

    public void MoneyCollected((bool, int) state)
    {
        var (isTaxMan, amount) = state;
        if (isTaxMan)
        {
            rhs += amount;

        }
        else
        {
            lhs += amount;

        }
    }
}
