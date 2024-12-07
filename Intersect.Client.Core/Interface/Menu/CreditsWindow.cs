using Intersect.Client.Interface.Extensions;
using Intersect.Client.Localization;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Menu;

public partial class CreditsWindow : IMainMenuWindow
{
    private MenuInterface _mainMenu = null!;
    private Widget? _creditsPanel;

    private const string TITLE = $"{nameof(TITLE)}";
    private const string BACK_BUTTON = $"{nameof(BACK_BUTTON)}";

    public void Load(MenuInterface menu)
    {
        _mainMenu = menu;

        _creditsPanel = Interface.LoadContent(Path.Combine("menu", "CreditsWindow.xmmp"));
        Interface.GetChildById<Label>(TITLE)?.SetText(Strings.Credits.Title);

        if (Interface.GetChildById<Button>(BACK_BUTTON, out var buttonCreditsBack))
        {
            buttonCreditsBack.Click += BackBtn_Clicked;
            buttonCreditsBack.SetText(Strings.Credits.Back);
        }

        _creditsPanel.Visible = false;
    }

    private void BackBtn_Clicked(object? sender, EventArgs e)
    {
        _mainMenu.SwitchToWindow<LoginWindow>();
    }

    public void Show()
    {
        _creditsPanel!.Visible = true;
    }

    public void Hide()
    {
        _creditsPanel!.Visible = false;
    }
}