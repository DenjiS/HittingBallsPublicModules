using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UIGame
{
    public class PlayerUIView : MonoBehaviour, IHealthView
    {
        [SerializeField] private WeaponUIView _ammoView;
        [SerializeField] private Image _healthImage;

        [Header("Tag")]
        [SerializeField] private TMP_Text _tag;
        [SerializeField][Min(0.01f)] private float _tagDisappearDelay;
        [SerializeField][Range(0, 1)] private float _brightness;

        [Header("UI position")]
        [SerializeField] private float _UIRelativeHeight;
        [SerializeField] private float _xRotation;
        private Transform _transform;

        private Transform _playerPoint;

        public WeaponUIView AmmoView => _ammoView;

        private void Awake()
        {
            _transform = transform;
            _transform.rotation = Quaternion.Euler(_xRotation, 0f, 0f);

            _ammoView.gameObject.SetActive(false);
        }

        private void Update()
        {
            Vector3 playerPosition = _playerPoint.position;

            _transform.position = new Vector3(
                playerPosition.x,
                _playerPoint.position.y + _UIRelativeHeight,
                playerPosition.z);
        }

        public void Initialize(Transform playerPoint, Color color, int playerNumber)
        {
            _playerPoint = playerPoint;
            _healthImage.color = color;

            _tag.color = Color.Lerp(color, Color.white, _brightness);
            _tag.text = $"P{playerNumber}";
            StartCoroutine(TagDisappearing());
        }

        public void RenderHealth(float healthRatio)
        {
            _healthImage.fillAmount = healthRatio;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        private IEnumerator TagDisappearing()
        {
            float elapsed = _tagDisappearDelay;

            while (elapsed > 0)
            {
                elapsed -= Time.deltaTime;
                _tag.alpha = elapsed / _tagDisappearDelay;
                yield return null;
            }

            _tag.enabled = false;
        }
    }
}
