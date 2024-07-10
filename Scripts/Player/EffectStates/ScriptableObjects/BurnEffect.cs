using System;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "NewBurnEffect", menuName = "Scriptable Resources/Effects/BurnEffect")]
public class BurnEffect : BaseScriptableEffect
{
    [SerializeField] private float _damagePerTick;

    public override BaseEffectController InitializePlayer(VisualEffect vfx, Action endCallback)
    {
        return new BurnEffectController(vfx, endCallback, IterationTime, Duration, _damagePerTick);
    }
}
