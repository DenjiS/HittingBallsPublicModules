using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player.UIGame
{
    public class WeaponUIView : MonoBehaviour, IWeaponView
    {
        [Header("Reload")]
        [SerializeField] private Image _reloadImage;

        [Header("Current ammo")]
        [SerializeField] private RectTransform _background;
        [SerializeField] private RectTransform foreground;
        [SerializeField] private RectTransform _bgItemTemplate;
        [SerializeField] private RectTransform _fgItemTemplate;
        [SerializeField] private int _minSize;

        [Header("Total ammo")]
        [SerializeField] private TMP_Text _totalAmmoText;
        [SerializeField] private float _totalAmmoDisappearDelay;

        private List<RectTransform> _backgroundItems = new();
        private List<RectTransform> _foregroundItems = new();

        private int _maxValue;
        private int _currentValue;

        public int CurrentAmmo
        {
            get => _currentValue;
            set
            {
                if (_currentValue == value)
                {
                    return;
                }

                _currentValue = value;
                OnValueChanged();
                UpdateForeground();
            }
        }

        public void Initialize(int maxAmmo, int currentAmmo)
        {
            if (maxAmmo < _minSize)
                _maxValue = _minSize;
            else
                _maxValue = maxAmmo;

            CurrentAmmo = currentAmmo;

            CreateList(_backgroundItems, _background, _bgItemTemplate, maxAmmo);
            CreateList(_foregroundItems, foreground, _fgItemTemplate, maxAmmo);
            UpdateForeground();
        }

        public void Clear()
        {
            ClearList(_backgroundItems);
            ClearList(_foregroundItems);
        }

        public void RenderReload(float max, float current)
        {
            _reloadImage.fillAmount = Mathf.Clamp01(current / max);
        }

        public void ShowTotalAmmo(int totalAmmo)
        {
            StartCoroutine(TotalAmmoShowing(totalAmmo));
        }

        private void CreateList(List<RectTransform> items, RectTransform parent, RectTransform itemTemplate, int maxValue)
        {
            itemTemplate.gameObject.SetActive(false);
            float angle = 360f / Convert.ToSingle(_maxValue);
            for (int i = 0; i < _maxValue; i++)
            {
                RectTransform item = CreateItem(parent, itemTemplate, i);
                items.Add(item);
                item.localEulerAngles = new Vector3(0, 0, -angle * i);

                if (i >= maxValue)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }

        private void ClearList(List<RectTransform> items)
        {
            foreach (RectTransform item in items)
            {
                Destroy(item.gameObject);
            }

            items.Clear();
        }

        private RectTransform CreateItem(RectTransform parent, RectTransform itemTemplate, int index)
        {
            GameObject item = Instantiate(itemTemplate.gameObject, parent);
            item.SetActive(true);
            item.name = "item" + (index + 1);
            RectTransform itemTransform = item.GetComponent<RectTransform>();
            itemTransform.localScale = Vector3.one;
            itemTransform.localEulerAngles = Vector3.zero;
            itemTransform.anchoredPosition3D = Vector3.zero;
            return itemTransform;
        }

        private void OnValueChanged()
        {
            if (_currentValue < 0)
            {
                _currentValue = 0;
            }
            else if (_currentValue > _maxValue)
            {
                _currentValue = _maxValue;
            }
        }

        private void UpdateForeground()
        {
            for (int i = 0; i < _foregroundItems.Count; i++)
            {
                RectTransform rectTrans = _foregroundItems[i];

                if (i < _currentValue)
                {
                    rectTrans.gameObject.SetActive(true);
                }
                else
                {
                    rectTrans.gameObject.SetActive(false);
                }
            }
        }

        private IEnumerator TotalAmmoShowing(int ammo)
        {
            _totalAmmoText.enabled = true;
            _totalAmmoText.text = ammo.ToString();

            float elapsed = _totalAmmoDisappearDelay;

            while (elapsed > 0)
            {
                elapsed -= Time.deltaTime;
                _totalAmmoText.alpha = elapsed / _totalAmmoDisappearDelay;
                yield return null;
            }

            _totalAmmoText.enabled = false;
        }
    }
}