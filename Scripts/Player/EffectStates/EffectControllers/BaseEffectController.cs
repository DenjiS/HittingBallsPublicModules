using System;
using UnityEngine.VFX;

public abstract class BaseEffectController
{
    private readonly VisualEffect _vfx;

    private readonly float _iterationTime;
    private readonly float _duration;

    private readonly Action _endCallback;

    private float _elapsedDuration;
    private float _elapsedIteration;

    public BaseEffectController(VisualEffect vfx, Action endCallback, float iterationTime, float duration)
    {
        _vfx = vfx;
        _vfx.Play();

        _endCallback = endCallback;

        _iterationTime = iterationTime;
        _duration = duration;

        _elapsedDuration = 0;
        _elapsedIteration = 0;
    }

    public void Contact()
    {
        _elapsedDuration = 0;
    }

    public void Tick(float deltaTime)
    {
        _elapsedIteration += deltaTime;

        if (_elapsedIteration >= _iterationTime)
        {
            _elapsedIteration = 0;
            IterateEffect();
        }

        _elapsedDuration += deltaTime;

        if (_elapsedDuration >= _duration)
        {
            End();
        }
    }

    public abstract void InitializePlayerParts(object[] parts);

    protected virtual void End()
    {
        _vfx.Stop();
        _endCallback?.Invoke();
    }

    protected abstract void IterateEffect();
}