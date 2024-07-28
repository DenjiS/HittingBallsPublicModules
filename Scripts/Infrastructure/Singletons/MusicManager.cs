using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Infrastructure
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : Singleton<MusicManager>
    {
        [SerializeField] private RandomArray<AssetReferenceT<AudioClip>> _audioClips = new();

        private AudioSource _audioSource;
        private WaitUntil _audioEndedCondition;

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            _audioEndedCondition = new(() => _audioSource.isPlaying == false && Application.isFocused);
        }

        /// <summary>
        /// Launches music main coroutine.
        /// </summary>
        public void Launch()
        {
            StartCoroutine(Playing());
        }
        
        /// <summary>
        /// Main coroutine.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Playing()
        {
            yield return PlayingClipAsset(_audioClips[0]);

            while (true)
                yield return PlayingClipAsset(_audioClips.GetNewRandom());
        }

        /// <summary>
        /// Loads, plays and unloads music audio file.
        /// </summary>
        /// <param name="clipReference">Music file reference.</param>
        /// <returns></returns>
        private IEnumerator PlayingClipAsset(AssetReferenceT<AudioClip> clipReference)
        {
            AsyncOperationHandle<AudioClip> handle = Addressables.LoadAssetAsync<AudioClip>(clipReference);

            yield return handle;

            _audioSource.clip = handle.Result;
            _audioSource.Play();

            yield return _audioEndedCondition;

            _audioSource.clip = null;
            Addressables.Release(handle);
        }

#if UNITY_EDITOR
        [ContextMenu(nameof(Stop))]
        private void Stop() =>
            _audioSource.Stop();
#endif
    }
}
