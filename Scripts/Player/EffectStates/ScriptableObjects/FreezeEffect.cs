using System;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "NewFreezeEffect", menuName = "Scriptable Resources/Effects/FreezeEffect")]
public class FreezeEffect : BaseScriptableEffect
{
    [SerializeField] private float _lowestSpeedRatio;

    public override BaseEffectController InitializePlayer(VisualEffect vfx, Action endCallback)
    {
        return new FreezeEffectController(vfx, endCallback, IterationTime, Duration, _lowestSpeedRatio);
    }
}
