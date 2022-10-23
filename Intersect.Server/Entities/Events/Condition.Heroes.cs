using Intersect.Enums;
using Intersect.GameObjects;
using Intersect.GameObjects.Events;

namespace Intersect.Server.Entities.Events
{
    public static partial class Conditions
    {
        public static bool MeetsCondition(
            ProfessionLevelIs condition,
            Player player,
            Event eventInstance,
            QuestBase questBase)
        {
            var curValue = player.GetPlayerProfessionValue(condition.ProfessionId).Level;
            var compValue = condition.Value;

            if (curValue == null)
            {
                return false;
            }

            switch (condition.Comparator) //Comparator
            {
                case VariableComparators.Equal:
                    if (curValue == compValue)
                    {
                        return true;
                    }

                    break;
                case VariableComparators.GreaterOrEqual:
                    if (curValue >= compValue)
                    {
                        return true;
                    }

                    break;
                case VariableComparators.LesserOrEqual:
                    if (curValue <= compValue)
                    {
                        return true;
                    }

                    break;
                case VariableComparators.Greater:
                    if (curValue > compValue)
                    {
                        return true;
                    }

                    break;
                case VariableComparators.Less:
                    if (curValue < compValue)
                    {
                        return true;
                    }

                    break;
                case VariableComparators.NotEqual:
                    if (curValue != compValue)
                    {
                        return true;
                    }

                    break;
            }

            return false;
        }
    }
}
