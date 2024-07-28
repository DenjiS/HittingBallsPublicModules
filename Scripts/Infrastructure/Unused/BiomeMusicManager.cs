using System.Linq;
using UnityEngine;

[System.Serializable]
public class BiomeMusicManager
{
    [SerializeField] private BiomeMusic[] _soundtracks;
    [SerializeField] private AudioSource _source;

    public enum Biomes
    {
        Default,
        SciFi,
        Mars
    }

    public void Play(Biomes biome)
    {
        AudioClip clip = _soundtracks
            .Where(soundtrack => soundtrack.Biome == biome)
            .FirstOrDefault().Music;

        if (clip != _source.clip)
        {
            _source.clip = clip;
            _source.Play();
        }
    }

    [System.Serializable]
    public struct BiomeMusic
    {
        public Biomes Biome;
        public AudioClip Music;
    }
}
