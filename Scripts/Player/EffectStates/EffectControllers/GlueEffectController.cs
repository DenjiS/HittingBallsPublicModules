using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class GlueEffectController : BaseEffectController
{
    private readonly float _lowestSpeedRatio;
    private readonly float _shiftPerIteration;

    private float _lowestSpeed;

    private IChangableMover _mover;

    public GlueEffectController(
        VisualEffect vfx,
        Action endCallback,
        float iterationTime,
        float duration,
        float lowestSpeedRatio,
        float shiftPerIteration
        ) : base(vfx, endCallback, iterationTime, duration)
    {
        _lowestSpeedRatio = lowestSpeedRatio;
        _shiftPerIteration = shiftPerIteration;
    }

    public override void InitializePlayerParts(object[] parts)
    {
        _mover = parts[0] as IChangableMover;
        _lowestSpeed = _mover.Speed * _lowestSpeedRatio;
    }

    protected override void End()
    {
        _mover.Speed = _mover.DefaultSpeed;
        base.End();
    }

    protected override void IterateEffect()
    {
        _mover.Speed = Mathf.Lerp(_mover.Speed, _lowestSpeed, _shiftPerIteration);
    }
}
