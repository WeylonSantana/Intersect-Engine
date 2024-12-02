using Intersect.Client.Localization;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Menu;

public partial class CreditsWindow : IMainMenuWindow
{
    private MainMenu _mainMenu = null!;
    private Widget? _creditsPanel;

    public void Load(MainMenu menu)
    {
        _mainMenu = menu;

        _creditsPanel = Interface.LoadContent(Path.Combine("menu", "CreditsWindow.xmmp"));
        if (Interface.GetChildById<Label>("_labelCreditsTitle", out var labelCreditsTitle))
        {
            labelCreditsTitle.Text = Strings.Credits.Title;
        }

        if (Interface.GetChildById<Label>("_labelCreditsBack", out var labelCreditsBack))
        {
            labelCreditsBack.Text = Strings.Credits.Back;
        }

        if (Interface.GetChildById<Button>("_buttonCreditsBack", out var buttonCreditsBack))
        {
            buttonCreditsBack.Click += BackBtn_Clicked;
        }

        _creditsPanel.Visible = false;
    }

    private void BackBtn_Clicked(object? sender, EventArgs e)
    {
        _mainMenu.SwitchToWindow<LoginWindow>();
    }

    public void Toggle(bool value)
    {
        _creditsPanel!.Visible = value;
    }
}