using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Infrastructure.Settings.Graphics
{
    public class QualitySetting : Setting<int>
    {
        public QualitySetting(LocalizedString localizedName) : base(localizedName)
        {
        }

        protected override int Value { get => QualitySettings.GetQualityLevel(); set => QualitySettings.SetQualityLevel(value); }

        protected override List<int> CreateOptionsList()
        {
            List<int> options = new();

            for (int i = 0; i < QualitySettings.count; i++)
                options.Add(i);

            return options;
        }

        protected override Dictionary<int, string> CreateOptionsNames(IList<int> options)
        {
            Dictionary<int, string> names = new();

            foreach (int quality in options)
                names[quality] = QualitySettings.names[quality].ToUpper();

            return names;
        }
    }
}