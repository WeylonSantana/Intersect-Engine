using Intersect.Localization;
using Newtonsoft.Json;

namespace Intersect.Editor.Localization
{
    public partial class Strings
    {
        public partial struct ClassEditor
        {
            public static LocalizedString AccuracyBoost = @"Accuracy (+{00}):";

            public static LocalizedString BaseAccuracy = @"Accuracy";

            public static LocalizedString BaseEvasion = @"Evasion";

            public static LocalizedString EvasionBoost = @"Evasion (+{00}):";
        }

        public partial struct NpcEditor
        {
            public static LocalizedString Accuracy = @"Accuracy:";

            public static LocalizedString Evasion = @"Evasion:";
        }

        public partial struct SpellEditor
        {
            public static LocalizedString Accuracy = @"Accuracy:";

            public static LocalizedString Evasion = @"Evasion:";
        }

        public partial struct ItemEditor
        {
            public static LocalizedString AccuracyBonus = @"Accuracy:";

            public static LocalizedString EvasionBonus = @"Evasion:";
        }

        public partial struct SpellEditor
        {
            public static LocalizedString EffectPercentageValue = @"Percentage Effect (%):";

            public static LocalizedString StatusPersist = @"Persist Effect on Death";
        }

        public partial struct ProfessionEditor
        {
            public static LocalizedString Cancel = @"Cancel";

            public static LocalizedString Copy = @"Copy Profession";

            public static LocalizedString Delete = @"Delete Profession";

            public static LocalizedString Deletetitle = @"Delete Profession";

            public static LocalizedString Deleteprompt =
                @"Are you sure you want to delete this profession? This action cannot be reverted!";

            public static LocalizedString Description = @"Description:";

            public static LocalizedString Expbase = @"Exp Base:";

            public static LocalizedString Expgrid = @"Experience Overrides";

            public static LocalizedString Expincrease = @"Exp Increase (%):";

            public static LocalizedString Folderlabel = @"Folder:";

            public static LocalizedString Foldertitle = @"Add Folder";

            public static LocalizedString Folderprompt = @"Enter a name for the folder you'd like to add:";

            public static LocalizedString General = @"General";

            public static LocalizedString Gridlevel = "Level";

            public static LocalizedString Gridtnl = "Exp TNL";

            public static LocalizedString Gridtotalexp = "Total Exp";

            public static LocalizedString Icon = "Icon:";

            public static LocalizedString Maxlevel = @"Max Level:";

            public static LocalizedString Name = @"Name:";

            public static LocalizedString New = @"New Profession";

            public static LocalizedString Paste = @"Paste Profession";

            public static LocalizedString Professions = @"Professions";

            public static LocalizedString Updategrid = @"Update Grid";

            public static LocalizedString Title = @"Profession Editor";

            public static LocalizedString Save = @"Save";

            public static LocalizedString Searchplaceholder = @"Search...";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString Sortalphabetically = @"Order Alphabetically";

            public static LocalizedString Undo = @"Undo Changes";

            public static LocalizedString Undoprompt =
                @"Are you sure you want to undo changes made to this animation? This action cannot be reverted!";

            public static LocalizedString Undotitle = @"Undo Changes";
        }

        public struct EventGiveProfessionExp
        {

            public static LocalizedString cancel = @"Cancel";

            public static LocalizedString label = @"Give Experience:";

            public static LocalizedString okay = @"Ok";

            public static LocalizedString title = @"Give Profession Exp";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString AmountType = @"Amount Type";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString Variable = @"Variable";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString Manual = @"Manual";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString PlayerVariable = @"Player Variable";

            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public static LocalizedString ServerVariable = @"Global Variable";

        }

        public partial struct EventCommandList
        {
            public static LocalizedString setprofessionlevel = @"Set Profession {00} Level To: {01}";

            public static LocalizedString giveprofessionexp = @"Give {00} Exp to Profession {01}";
        }

        public struct EventSetProfessionLevel
        {

            public static LocalizedString cancel = @"Cancel";

            public static LocalizedString level = @"Set Level:";

            public static LocalizedString profession = @"Profession:";

            public static LocalizedString okay = @"Ok";

            public static LocalizedString title = @"Change Level";

        }
    }
}
