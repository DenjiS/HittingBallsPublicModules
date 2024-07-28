using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Infrastructure.Settings.Graphics
{
    public class GraphicsSettingsManager : Singleton<GraphicsSettingsManager>
    {
        [Header("Settings localization")]
        [SerializeField] private LocalizedString _qualityName;
        [SerializeField] private LocalizedString _resolutionName;
        [SerializeField] private LocalizedString _fullscreenName;
        [SerializeField] private LocalizedString _vSyncName;

        [Header("Utils localization")]
        [SerializeField] private LocalizedString _onString;
        [SerializeField] private LocalizedString _offString;

        private ISetting[] _settings;

        public IReadOnlyCollection<ISetting> Settings => _settings;

        protected override void Awake()
        {
            base.Awake();

            _settings = new ISetting[]
            {
                new QualitySetting(_qualityName),
                new ResolutionSetting(_resolutionName),
                new FullscreenSetting(_fullscreenName),
                new VSyncSetting(_vSyncName),
            };
        }
    }
}
