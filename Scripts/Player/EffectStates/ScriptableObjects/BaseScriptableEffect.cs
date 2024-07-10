using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public abstract class BaseScriptableEffect : ScriptableObject
{
    [SerializeField] private VisualEffect _vfxTemplate;
    [SerializeField] private float _iterationTime;
    [SerializeField] private float _duration;

    public VisualEffect VfxTemplate => _vfxTemplate;
    protected float IterationTime => _iterationTime;
    protected float Duration => _duration;

    public abstract BaseEffectController InitializePlayer(VisualEffect vfx, Action endCallback);
}
