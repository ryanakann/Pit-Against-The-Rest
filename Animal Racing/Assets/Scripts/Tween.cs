using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tween
{
    public AnimationCurve curve;
    
    public float Evaluate(float t)
    {
        return curve.Evaluate(t);
    }
}
