using Intersect.Client.Interface.Extensions;
using Intersect.Client.Localization;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Menu;

public partial class CreditsWindow : IMainMenuWindow
{
    private MenuInterface _mainMenu = null!;
    private Widget? _creditsPanel;

    public void Load(MenuInterface menu)
    {
        _mainMenu = menu;

        _creditsPanel = Interface.LoadContent(Path.Combine("menu", "CreditsWindow.xmmp"));
        _creditsPanel.FindChildById<Label>(TITLE_LABEL)?.SetText(Strings.Credits.Title);

        if (_creditsPanel.FindChildById<Button>(BACK_BUTTON, out var buttonCreditsBack))
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

    #region Constants

    private const string TITLE_LABEL = nameof(TITLE_LABEL);
    private const string BACK_BUTTON = nameof(BACK_BUTTON);

    #endregion
}