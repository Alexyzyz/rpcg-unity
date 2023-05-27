using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilEasing
{

    public static float EaseOutCubic(float start, float end, float value) {
        value--;
        end -= start;
        return end * (value * value * value + 1) + start;
    }

}
