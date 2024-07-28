using UnityEngine;

namespace Infrastructure
{
    public class CustomizationElement<T> : ScriptableObject
    {
        [SerializeField] private T _element;

        [Header("UI View")]
        [SerializeField] private Sprite _texture;

        public T Element => _element;

        public Sprite Sprite => _texture;
    }
}