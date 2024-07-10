using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CooldownTimer : IDisposable
{
    private readonly MonoBehaviour _monoBeh;
    private WaitForSeconds _delay;

    public event UnityAction Started;
    public event UnityAction Finished;

    public bool IsReady { get; private set; } = true;

    public CooldownTimer(MonoBehaviour callingMono, float cooldownTime)
    {
        _monoBeh = callingMono;
        _delay = new WaitForSeconds(cooldownTime);
    }

    public bool Activate()
    {
        if (IsReady == true && _monoBeh.isActiveAndEnabled)
        {
            IsReady = false;
            _monoBeh.StartCoroutine(CountdownTimer());

            return true;
        }

        return false;
    }

    public void ChangeDelay(float delay)
    {
        _delay = new WaitForSeconds(delay);
    }

    public void Dispose()
    {
        Started = null;
        Finished = null;
    }

    private IEnumerator CountdownTimer()
    {
        Started?.Invoke();

        yield return _delay;
        IsReady = true;

        Finished?.Invoke();
    }
}
