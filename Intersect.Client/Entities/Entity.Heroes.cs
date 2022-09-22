using System;
using Intersect.Enums;
using Intersect.GameObjects;

namespace Intersect.Client.Entities
{

    public partial class Entity
    {
        public SpellBase StatusActive(StatusTypes status)
        {
            for (var n = 0; n < Status.Count; n++)
            {
                if (Status[n].Type == status)
                {
                    var spell = SpellBase.Get(Status[n].SpellId);
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

        public int SwiftTime(int attackTime)
        {
            var spellStatus = StatusActive(StatusTypes.Swift);
            if (spellStatus == null)
            {
                return attackTime;
            }

            float timeMultiplier = Math.Abs(spellStatus.Combat.EffectPercentageValue / 100f);
            if (spellStatus.Combat.EffectPercentageValue > 0)
            {
                attackTime -= (int) Math.Floor(attackTime * timeMultiplier);
            }
            else if (spellStatus.Combat.EffectPercentageValue < 0)
            {
                attackTime += (int) Math.Floor(attackTime * timeMultiplier);
            }

            return attackTime;
        }
    }
}
