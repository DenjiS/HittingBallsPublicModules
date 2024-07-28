using UnityEngine;
using UnityEngine.Localization;

namespace Infrastructure.Settings
{
    public class GameSettingsManager : Singleton<GameSettingsManager>
    {
        [SerializeField] private LocalizedString _languageString;

        public LanguageSetting LanguageSetting {  get; private set; }

        protected override void Awake()
        {
            base.Awake();

            LanguageSetting = new LanguageSetting(_languageString);
        }
    }
}
