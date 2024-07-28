using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Infrastructure.Settings
{
    public abstract class Setting<T> : ISetting
    {
        private readonly IReadOnlyDictionary<T, string> _optionsNames;
        private readonly IList<T> _optionsList;

        private readonly LocalizedString _localizedName;

        private int _currentIndex;

        public Setting(LocalizedString localizedName)
        {
            _localizedName = localizedName;

            _optionsList = CreateOptionsList();
            _optionsNames = CreateOptionsNames(_optionsList);

            Load();
        }

        protected abstract T Value { get; set; }

        public LocalizedString LocalizedSettingName => _localizedName;

        public string CurrentValueName => _optionsNames[_optionsList[_currentIndex]];

        public void SetNextIndex()
        {
            if (++_currentIndex == _optionsList.Count)
                _currentIndex = 0;
        }

        public void SetPreviousIndex()
        {
            if (--_currentIndex < 0)
                _currentIndex = _optionsList.Count - 1;
        }

        public void SetCurrentIndex()
        {
            _currentIndex = _optionsList.IndexOf(Value);
        }

        public void Apply()
        {
            Value = _optionsList[_currentIndex];
            Save();
        }

        protected abstract List<T> CreateOptionsList();

        protected abstract Dictionary<T, string> CreateOptionsNames(IList<T> options);

        private void Load()
        {
            if (PlayerPrefsExtended.HasKey(_localizedName.TableEntryReference.ToString()))
            {
                _currentIndex = PlayerPrefsExtended.GetInt(_localizedName.TableEntryReference.ToString());

                if (_currentIndex < _optionsList.Count)
                {
                    Value = _optionsList[_currentIndex];
                    return;
                }
            }

            SetCurrentIndex();
            Save();

        }

        private void Save()
        {
            PlayerPrefsExtended.SetInt(_localizedName.TableEntryReference.ToString(), _currentIndex);
        }
    }
}
