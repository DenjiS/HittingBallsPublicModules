using UnityEngine;

namespace Infrastructure
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : Singleton<MusicManager>
    {
        [SerializeField] private RandomArray<AudioClip> _audioClips = new();

        private AudioSource _audioSource;

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            PlayClip(_audioClips[0]);
        }

        private void Update()
        {
            if (_audioSource.isPlaying || Application.isFocused == false)
                return;

            PlayClip(_audioClips.GetNewRandom());
        }

        private void PlayClip(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }
    }
}
