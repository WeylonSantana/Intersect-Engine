using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;
using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Enums;

namespace Intersect.Client.Interface.Game.Character
{
    public partial class CharacterWindow
    {
        Button mAddAccuracyBtn;

        Button mAddEvasionBtn;

        Label mAccuracyLabel;

        Label mEvasionLabel;

        void UpdateExtraStatus()
        {
            mAccuracyLabel.SetText(
                Strings.Character.stat5.ToString(Strings.Combat.stat5, Globals.Me.Stat[(int)Stats.Accuracy])
            );

            mEvasionLabel.SetText(
                Strings.Character.stat6.ToString(Strings.Combat.stat6, Globals.Me.Stat[(int)Stats.Evasion])
            );

            mAddAccuracyBtn.IsHidden =
                Globals.Me.StatPoints == 0 || Globals.Me.Stat[(int)Stats.Accuracy] == Options.MaxStatValue;

            mAddEvasionBtn.IsHidden =
                Globals.Me.StatPoints == 0 || Globals.Me.Stat[(int)Stats.Evasion] == Options.MaxStatValue;
        }
        void _addAccuracyBtn_Clicked(Base sender, ClickedEventArgs arguments)
        {
            PacketSender.SendUpgradeStat((int)Stats.Accuracy);
        }

        void _addEvasionBtn_Clicked(Base sender, ClickedEventArgs arguments)
        {
            PacketSender.SendUpgradeStat((int)Stats.Evasion);
        }
    }
}
