using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

namespace Infrastructure.Settings.Graphics
{
    internal class ResolutionSetting : Setting<Vector2Int>
    {
        public ResolutionSetting(LocalizedString localizedString) : base(localizedString)
        {
        }

        protected override Vector2Int Value
        {
            get
            {
                Resolution resolution = Screen.currentResolution;
                return new Vector2Int(resolution.width, resolution.height);
            }

            set => Screen.SetResolution(value.x, value.y, Screen.fullScreenMode);
        }

        protected override List<Vector2Int> CreateOptionsList()
        {
            List<Vector2Int> options = new();

            foreach (Resolution resolution in Screen.resolutions)
            {
                Vector2Int resolutionVector = new Vector2Int(resolution.width,resolution.height);
                options.Add(resolutionVector);
            }

            return options.Distinct()
                .OrderBy(resolutionVector => resolutionVector.x).ThenBy(resolutionVector => resolutionVector.y)
                .ToList();
        }

        protected override Dictionary<Vector2Int, string> CreateOptionsNames(IList<Vector2Int> options)
        {
            Dictionary<Vector2Int, string> names = new();

            foreach (Vector2Int resolutionVector in options)
            {
                names[resolutionVector] = $"{resolutionVector.x}x{resolutionVector.y}";
            }

            return names;
        }
    }
}
