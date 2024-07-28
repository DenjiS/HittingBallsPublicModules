using UnityEngine;
using UnityEngine.Localization;

namespace Infrastructure.Settings.Graphics
{
    internal class FullscreenSetting : BoolSetting
    {
        public FullscreenSetting(LocalizedString localizedName) : base(localizedName)
        {
        }

        protected override bool Value { get => Screen.fullScreen; set => Screen.fullScreen = value; }
    }
}