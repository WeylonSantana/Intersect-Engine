using Intersect.Client.Interface.Game.DescriptionWindows.Components;
using Intersect.Client.Localization;
using Intersect.Enums;

namespace Intersect.Client.Interface.Game.DescriptionWindows
{
    public partial class SpellDescriptionWindow
    {
        void ExtraValueSetup(RowContainerComponent rows)
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

            if (mSpell.Combat.Effect == StatusTypes.Swift)
            {
                if (mSpell.Combat.EffectPercentageValue != 0)
                {
                    rows.AddKeyValueRow(
                        Strings.SpellDescription.SwiftChange,
                        Strings.SpellDescription.Percentage.ToString(mSpell.Combat.EffectPercentageValue)
                    );
                }
            }

            if (mSpell.Combat.Effect == StatusTypes.CooldownChange)
            {
                if (mSpell.Combat.EffectPercentageValue != 0)
                {
                    rows.AddKeyValueRow(
                        Strings.SpellDescription.CooldownChange,
                        Strings.SpellDescription.Percentage.ToString(mSpell.Combat.EffectPercentageValue)
                    );
                }
            }

            if (mSpell.Combat.Effect == StatusTypes.ExpChange)
            {
                if (mSpell.Combat.EffectPercentageValue != 0)
                {
                    rows.AddKeyValueRow(
                        Strings.SpellDescription.ExpChange,
                        Strings.SpellDescription.Percentage.ToString(mSpell.Combat.EffectPercentageValue)
                    );
                }
            }

            if (mSpell.Combat.Effect == StatusTypes.LuckChange)
            {
                if (mSpell.Combat.EffectPercentageValue != 0)
                {
                    rows.AddKeyValueRow(
                        Strings.SpellDescription.LuckChange,
                        Strings.SpellDescription.Percentage.ToString(mSpell.Combat.EffectPercentageValue)
                    );
                }
            }

            if (mSpell.Combat.Effect == StatusTypes.TenacityChange)
            {
                if (mSpell.Combat.EffectPercentageValue != 0)
                {
                    rows.AddKeyValueRow(
                        Strings.SpellDescription.TenacityChange,
                        Strings.SpellDescription.Percentage.ToString(mSpell.Combat.EffectPercentageValue)
                    );
                }
            }
        }
    }
}
