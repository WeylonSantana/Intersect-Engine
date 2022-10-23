using Intersect.Client.Framework.Gwen.Control;
using Intersect.Client.Framework.Gwen.Control.EventArguments;

namespace Intersect.Client.Interface.Game
{
    public partial class Menu
    {
        private readonly ImagePanel mProfessionBackground;

        private readonly Button mProfessionButton;

        private readonly ProfessionWindow mProfessionWindow;

        public void ToggleProfessionWindow()
        {
            if (mProfessionWindow.IsVisible())
            {
                mProfessionWindow.Hide();
            }
            else
            {
                HideWindows();
                mProfessionWindow.Show();
            }
        }

        private void ProfessionButton_Clicked(Base sender, ClickedEventArgs arguments)
        {
            ToggleProfessionWindow();
        }
    }
}
