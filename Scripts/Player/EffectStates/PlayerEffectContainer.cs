using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

namespace Player.EffectStates
{
    [RequireComponent(typeof(IPartsForEffects))]
    public class PlayerEffectContainer : MonoBehaviour, IEffectReceiver
    {
        [SerializeField] private BaseScriptableEffect[] _effects;

        private Dictionary<BaseScriptableEffect, VisualEffect> _effectsVfx = new();
        private Dictionary<BaseScriptableEffect, BaseEffectController> _currentEffects = new();
        private IPartsForEffects _controller;

        private void Awake()
        {
            _controller = GetComponent<IPartsForEffects>();

            foreach (BaseScriptableEffect effect in _effects)
            {
                VisualEffect vfx = Instantiate(effect.VfxTemplate, transform);

                _effectsVfx[effect] = vfx;
            }
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            BaseEffectController[] effectControllers = _currentEffects.Values.ToArray();

            for (int i = 0; i < effectControllers.Length; i++)
                effectControllers[i].Tick(deltaTime);
        }

        public void ApplyEffect(BaseScriptableEffect effect)
        {
            if (_currentEffects.ContainsKey(effect) == false)
            {
                object[] parts = _controller.GetPlayerParts(effect);

                BaseEffectController effectController = effect.InitializePlayer(_effectsVfx[effect], () => _currentEffects.Remove(effect));
                effectController.InitializePlayerParts(parts);

                _currentEffects.Add(effect, effectController);
            }
            else
            {
                _currentEffects[effect].Contact();
            }
        }
    }
}
