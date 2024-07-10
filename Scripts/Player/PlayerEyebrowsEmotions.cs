using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Player
{
    [Serializable]
    public class PlayerEyebrowsEmotions : IDisposable
    {
        public enum Emotions
        {
            Normal,
            Angry,
            Sad
        }

        [SerializeField] private DecalProjector _normal;
        [SerializeField] private DecalProjector _angry;
        [SerializeField] private DecalProjector _sad;

        [SerializeField] private float _emotionDuration;

        private Emotions _currentEmotion;

        private CooldownTimer _emotionCooldown;

        private Dictionary<Emotions, DecalProjector> _emotions = new();

        public void Initialize(MonoBehaviour monoBeh)
        {
            _emotions.Add(Emotions.Normal, _normal);
            _emotions.Add(Emotions.Angry, _angry);
            _emotions.Add(Emotions.Sad, _sad);

            _currentEmotion = Emotions.Normal;

            _emotionCooldown = new CooldownTimer(monoBeh, _emotionDuration);
            _emotionCooldown.Finished += () => SetEmotion(Emotions.Normal);
        }

        public void ShowEmotion(Emotions emotion)
        {
            if (_currentEmotion == emotion)
            {
                return;
            }
            else if (_emotionCooldown.Activate())
            {
                SetEmotion(emotion);
            }
        }

        private void SetEmotion(Emotions emotion)
        {
            _emotions[_currentEmotion].enabled = false;
            _currentEmotion = emotion;
            _emotions[emotion].enabled = true;
        }

        public void Dispose()
        {
            _emotionCooldown.Dispose();
        }
    }
}
