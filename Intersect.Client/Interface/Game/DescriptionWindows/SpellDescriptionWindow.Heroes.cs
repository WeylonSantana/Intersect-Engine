using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intersect.Client.Interface.Game.DescriptionWindows.Components;
using Intersect.Client.Localization;
using Intersect.Enums;

namespace Intersect.Client.Interface.Game.DescriptionWindows
{
    public partial class SpellDescriptionWindow
    {
        void HasteValueSetup(RowContainerComponent rows)
        {
            if (mSpell.Combat.Effect == StatusTypes.Haste)
            {
                if (mSpell.Combat.EffectPercentageValue > 0)
                {
                    rows.AddKeyValueRow(
                        Strings.SpellDescription.HastePositiveEffect,
                        Strings.SpellDescription.Percentage.ToString(mSpell.Combat.EffectPercentageValue)
                    );
                }
                else if (mSpell.Combat.EffectPercentageValue < 0)
                {
                    rows.AddKeyValueRow(
                        Strings.SpellDescription.HasteNegativeEffect,
                        Strings.SpellDescription.Percentage.ToString(mSpell.Combat.EffectPercentageValue)
                    );
                }
            }
        }
    }
}
