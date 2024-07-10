using System;
using UnityEngine;
using UnityEngine.VFX;

public class BurnEffectController : BaseEffectController
{
    private readonly float _damagePerIteration;

    private IDamageableHealable _targetHealth;

    public BurnEffectController(
        VisualEffect vfx,
        Action endCallback,
        float iterationTime,
        float duration,
        float damagePerIteration
        ) : base(vfx, endCallback, iterationTime, duration)
    {
        _damagePerIteration = damagePerIteration;
    }

    public override void InitializePlayerParts(object[] parts)
    {
        _targetHealth = parts[0] as IDamageableHealable;
    }

    protected override void IterateEffect()
    {
        _targetHealth.TakeDamage(_damagePerIteration);
    }
}
