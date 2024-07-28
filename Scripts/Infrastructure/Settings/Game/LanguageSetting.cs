using System.Collections;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Infrastructure.Settings
{
    public class LanguageSetting : Setting<Locale>
    {
        public LanguageSetting(LocalizedString localizedString) : base(localizedString)
        {
        }

        protected override Locale Value
        {
            get => LocalizationSettings.SelectedLocale;
            set => GameSettingsManager.Instance.StartCoroutine(SetInitializedLocale(value));
        }

        protected override List<Locale> CreateOptionsList()
        {
            return LocalizationSettings.AvailableLocales.Locales;
        }

        protected override Dictionary<Locale, string> CreateOptionsNames(IList<Locale> options)
        {
            Dictionary<Locale, string> optionsNames = new();

            foreach (Locale locale in options)
                optionsNames[locale] = locale.LocaleName;

            return optionsNames;
        }

        private IEnumerator SetInitializedLocale(Locale locale)
        {
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = locale;
        }
    }
}
