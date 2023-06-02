using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BezierCurve
{

    public Vector2 P0 { get; set; } = Vector2.zero;
    public Vector2 P1 { get; set; } = Vector2.zero;
    public Vector2 P2 { get; set; } = Vector2.zero;
    public Vector2 P3 { get; set; } = Vector2.zero;

    public Vector2 GetPositionOnCurve(float t)
    {
        Vector2 a = P0 * (-1 * t * t * t + 3 * t * t - 3 * t + 1);
        Vector2 b = P1 * ( 3 * t * t * t - 6 * t * t + 3 * t);
        Vector2 c = P2 * (-3 * t * t * t + 3 * t * t);
        Vector2 d = P3 * ( t * t * t);

        return a + b + c + d;
    }

    public Vector2 GetVelocityOnCurve(float t)
    {
        Vector2 a = P0 * (-3 * t * t +  6 * t - 3);
        Vector2 b = P1 * ( 9 * t * t - 12 * t + 3);
        Vector2 c = P2 * (-9 * t * t +  6 * t);
        Vector2 d = P3 * ( 3 * t * t);
        return a + b + c + d;
    }

    public float GetLength(int steps = 16)
    {
        float length = 0;
        float valueStep = 1 / (float)steps;
        int i = 1;
        while (i < steps)
        {
            length += (GetPositionOnCurve(i * valueStep) - GetPositionOnCurve((i - 1) * valueStep)).magnitude;
            i++;
        }
        return length;
    }

}
