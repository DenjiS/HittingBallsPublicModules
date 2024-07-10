using System;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class FreezeEffectController : BaseEffectController
{
    private readonly float _lowestSpeedRatio;

    private IChangableMover _targetMover;

    public FreezeEffectController(
        VisualEffect vfx, 
        Action endCallback, float 
        iterationTime, 
        float duration,
        float lowestSpeedRatio
        ) : base(vfx, endCallback, iterationTime, duration)
    {
        _lowestSpeedRatio = lowestSpeedRatio;
    }

    public override void InitializePlayerParts(object[] parts)
    {
        _targetMover = parts[0] as IChangableMover;
        _targetMover.Speed *= _lowestSpeedRatio;
    }

    protected override void End()
    {
        _targetMover.Speed = _targetMover.DefaultSpeed;
        base.End();
    }

    protected override void IterateEffect() { }
}
