using UnityEngine;

namespace Utils
{
    /// <summary>
    /// Contains sound clip and volume.
    /// </summary>
    [System.Serializable]
    public struct SoundData
    {
        [SerializeField] AudioClip _clip;
        [SerializeField, Range(0, 1)] float _volume;

        /// <summary>
        /// Plays this sound at corresponding volume by passed <see cref="AudioSource"/>.
        /// </summary>
        /// <param name="source"></param>
        public void PlayBy(AudioSource source) =>
            source.PlayOneShot(_clip, _volume);
    }
}