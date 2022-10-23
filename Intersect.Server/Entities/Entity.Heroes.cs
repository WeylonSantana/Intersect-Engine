using System;
using System.Collections.Generic;
using Intersect.Enums;
using Intersect.GameObjects;

namespace Intersect.Server.Entities
{

    public partial class Entity
    {
        public List<SpellBase> GetStatusActiveList(StatusTypes status)
        {
            List<SpellBase> result = new List<SpellBase>();

            foreach (var s in CachedStatuses)
            {
                if (s.Type == status)
                {
                    var spell = SpellBase.Get(s.Spell.Id);
                    if (spell != null)
                    {
                        result.Add(spell);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Modifies a given speed by the given modifier.
        /// </summary>
        /// <param name="speed">Attack or Movement Speed</param>
        /// <param name="modifier">Haste or Swift spell status </param>
        /// <returns></returns>
        public float SpeedStatusModifier(float speed, StatusTypes modifier)
        {
            var spellStatus = GetStatusActiveList(modifier);
            if (spellStatus.Count == 0)
            {
                return speed;
            }

            for (int i = 0; i < spellStatus.Count; i++)
            {
                float timeMultiplier = Math.Abs(spellStatus[i].Combat.EffectPercentageValue / 100f);
                if (spellStatus[i].Combat.EffectPercentageValue > 0)
                {
                    speed -= speed * timeMultiplier;
                }
                else if (spellStatus[i].Combat.EffectPercentageValue < 0)
                {
                    speed += speed * timeMultiplier;
                }
            }

            return speed;
        }

        public int GetExtraSpellBuff(StatusTypes status)
        {
            int amount = 0;
            var spellStatus = GetStatusActiveList(status);

            for (var i = 0; i < spellStatus.Count; i++)
            {
                amount += spellStatus[i].Combat.EffectPercentageValue;
            }

            return amount;
        }
    }
}