using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilMath
{
	
    /// <summary>
    /// Rotate a vector by radian amount.
    /// </summary>
	public static Vector2 RotateVector(Vector2 vector, float radian)
    {
        return new Vector2(
            vector.x * Mathf.Cos(radian) - vector.y * Mathf.Sin(radian),
            vector.x * Mathf.Sin(radian) + vector.y * Mathf.Cos(radian)
        );
    }

}
