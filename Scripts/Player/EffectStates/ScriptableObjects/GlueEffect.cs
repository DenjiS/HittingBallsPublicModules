using System;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "NewGlueEffect", menuName = "Scriptable Resources/Effects/GlueEffect")]
public class GlueEffect : BaseScriptableEffect
{
    [SerializeField] private float _lowestSpeedRatio;
    [SerializeField] private float _shiftPerIteration;

    public override BaseEffectController InitializePlayer(VisualEffect vfx, Action endCallback)
    {
        return new GlueEffectController(vfx, endCallback, IterationTime, Duration, _lowestSpeedRatio, _shiftPerIteration);
    }
}
