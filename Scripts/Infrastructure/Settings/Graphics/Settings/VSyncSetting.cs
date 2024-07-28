using UnityEngine;
using UnityEngine.Localization;

namespace Infrastructure.Settings.Graphics
{
    public class VSyncSetting : BoolSetting
    {
        public VSyncSetting(LocalizedString localizedName) : base(localizedName)
        {
        }

        protected override bool Value
        {
            get => QualitySettings.vSyncCount > 0;
            set
            {
                if (value)
                    QualitySettings.vSyncCount = 1;
                else
                    QualitySettings.vSyncCount = 0;
            }
        }
    }
}
