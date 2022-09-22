using System;
using Intersect.Enums;
using Intersect.GameObjects;

namespace Intersect.Server.Entities
{

    public partial class Entity
    {
        public SpellBase StatusActive(StatusTypes status)
        {
            foreach (var s in CachedStatuses)
            {
                if (s.Type == status)
                {
                    var spellNum = Spells[SpellCastSlot].SpellId;
                    var spell = SpellBase.Get(spellNum);
                    if (spell != null)
                    {
                        return spell;
                    }
                }
            }

            return null;
        }

        public float HasteTime(float time)
        {
            var spellStatus = StatusActive(StatusTypes.Haste);
            if (spellStatus == null)
            {
                return time;
            }

            float timeMultiplier = Math.Abs(spellStatus.Combat.EffectPercentageValue / 100f);
            if (spellStatus.Combat.EffectPercentageValue > 0)
            {
                time -= time * timeMultiplier;
            }
            else if (spellStatus.Combat.EffectPercentageValue < 0)
            {
                time += time * timeMultiplier;
            }

            return time;
        }
    }
}
