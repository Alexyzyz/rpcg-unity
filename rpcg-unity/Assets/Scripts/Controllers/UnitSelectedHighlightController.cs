using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedHighlightController : MonoBehaviour
{

    private Coroutine coroutineAnimateSpinning;

    public void ToggleVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    private void PlayAnimation()
    {
        float secondsPerSpin = 2f;

        this.EnsureCoroutineStopped(ref coroutineAnimateSpinning);
        coroutineAnimateSpinning = this.CreateUpdateRoutine(Routine);

        void Routine()
        {
            transform.eulerAngles += Time.deltaTime * new Vector3(0, 360f / secondsPerSpin, 0);
        }
    }

    private void Awake()
    {
        PlayAnimation();
    }

    private void OnEnable()
    {
        PlayAnimation();
    }

}
