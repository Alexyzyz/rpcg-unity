using System;
using System.Collections;
using UnityEngine;

public static class UtilCoroutine
{

    /// <summary>
    /// Stops a coroutine safely.
    /// </summary>
    public static void EnsureCoroutineStopped(this MonoBehaviour value, ref Coroutine routine)
    {
        if (routine != null)
        {
            value.StopCoroutine(routine);
            routine = null;
        }
    }

    /// <summary>
    /// For creating a simple timer.
    /// </summary>
    public static Coroutine CreateTimerRoutine(this MonoBehaviour value, float duration, Action onComplete = null)
    {
        return value.StartCoroutine(TimerRoutine(duration, onComplete));
    }

    /// <summary>
    /// For creating a simple routine-based animation.
    /// </summary>
    public static Coroutine CreateAnimationRoutine(this MonoBehaviour value, float duration, Action<float> transFunction, Action onComplete = null)
    {
        return value.StartCoroutine(AnimationRoutine(duration, transFunction, onComplete));
    }

    private static IEnumerator TimerRoutine(float duration, Action onComplete)
    {
        yield return new WaitForSeconds(duration);
        onComplete?.Invoke();
    }

    private static IEnumerator AnimationRoutine(float duration, Action<float> transFunction, Action onComplete)
    {
        float elapsedTime = 0;
        float progress = 0;
        while (progress <= 1)
        {
            transFunction(progress);
            elapsedTime += Time.deltaTime;
            progress = elapsedTime / duration;
            yield return null;
        }
        transFunction(1);
        onComplete?.Invoke();
    }

}