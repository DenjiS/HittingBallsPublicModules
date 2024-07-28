using System.Collections.Generic;
using UnityEngine.Localization;

namespace Infrastructure.Settings
{
    public abstract class BoolSetting : Setting<bool>
    {
        protected BoolSetting(LocalizedString localizedName) : base(localizedName)
        {
        }

        protected override List<bool> CreateOptionsList()
        {
            return new()
            {
                true,
                false,
            };
        }

        protected override Dictionary<bool, string> CreateOptionsNames(IList<bool> _)
        {
            return new()
            {
                [true] = "ON",
                [false] = "OFF",
            };
        }
    }
}
