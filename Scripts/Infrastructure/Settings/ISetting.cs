using UnityEngine.Localization;

namespace Infrastructure.Settings
{
    public interface ISetting
    {
        public LocalizedString LocalizedSettingName { get; }

        public string CurrentValueName { get; }

        public void SetNextIndex();

        public void SetPreviousIndex();

        public void SetCurrentIndex();

        public void Apply();
    }
}