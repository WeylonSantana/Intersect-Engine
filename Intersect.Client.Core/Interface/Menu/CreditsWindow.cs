using Intersect.Client.Localization;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Menu;

public partial class CreditsWindow : IMainMenuWindow
{
    private MainMenu _mainMenu = null!;
    private Panel? _creditsPanel;
    private Label? _labelTitle;
    private Label? _labelBack;
    private Button? _buttonBack;

    public bool IsHidden => _creditsPanel!.Visible;

    public void Load(MainMenu menu)
    {
        _mainMenu = menu;

        _creditsPanel = (Panel)Interface.LoadContent(Path.Combine("menu", "CreditsWindow.xmmp")).Root;
        if (Interface.GetChildById<Label>("_labelTitle", out var labelTitle))
        {
            _labelTitle = labelTitle;
            _labelTitle.Text = Strings.Credits.Title;
        }

        if (Interface.GetChildById<Label>("_labelBack", out var labelBack))
        {
            _labelBack = labelTitle;
            _labelBack.Text = Strings.Credits.Back;
        }

        if (Interface.GetChildById<Button>("_buttonBack", out var buttonBack))
        {
            _buttonBack = buttonBack;
            _buttonBack.Click += BackBtn_Clicked;
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