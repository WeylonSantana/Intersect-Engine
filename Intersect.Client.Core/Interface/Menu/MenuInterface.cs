using Intersect.Client.Core;
using Intersect.Client.Interface.Shared;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Network;
using Intersect.Utilities;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Menu;

public partial class MenuInterface
{
    private readonly LoginWindow _loginWindow;
    private readonly RegisterWindow _registerWindow;
    private readonly ForgotPasswordWindow _forgotPasswordWindow;
    private readonly ResetPasswordWindow _resetPasswordWindow;
    private readonly CreateCharacterWindow _createCharacterWindow;
    private readonly SettingsWindow _settingsWindow;
    private static DebugWindow _debugUi;
    private readonly CreditsWindow _creditsWindow;
    public readonly SelectCharacterWindow SelectCharacterWindow;

    private readonly Label _serverStatusLabel;

    //Character creation feild check
    private bool mShouldOpenCharacterCreation;
    private bool mShouldOpenCharacterSelection;

    // Network status
    public static NetworkStatus ActiveNetworkStatus;

    public delegate void NetworkStatusHandler();

    public static NetworkStatusHandler? NetworkStatusChanged;
    internal static event EventHandler? ReceivedConfiguration;

    public static long LastNetworkStatusChangeTime { get; private set; }

    public MenuInterface()
    {
        var _serverStatusArea = Interface.LoadContent(Path.Combine("menu", "ServerStatus.xmmp"));
        if (Interface.GetChildById<Label>("_serverStatusLabel", out var label))
        {
            _serverStatusLabel = label;
            _serverStatusLabel.Text = Strings.Server.StatusLabel.ToString(ActiveNetworkStatus.ToLocalizedString());
            _serverStatusLabel.Visible = !ClientContext.IsSinglePlayer;
        }

        NetworkStatusChanged += HandleNetworkStatusChanged;

        _loginWindow = new LoginWindow(Interface.Desktop);
        _registerWindow = new RegisterWindow();
        SelectCharacterWindow = new SelectCharacterWindow();
        _createCharacterWindow = new CreateCharacterWindow();
        _settingsWindow = new SettingsWindow();
        _debugUi = new DebugWindow();
        _creditsWindow = new CreditsWindow();

        //_forgotPasswordWindow = new ForgotPasswordWindow(_menuCanvas, this);
        //_resetPasswordWindow = new ResetPasswordWindow(_menuCanvas, this);

        SwitchToWindow<LoginWindow>();
    }

    ~MenuInterface()
    {
        NetworkStatusChanged -= HandleNetworkStatusChanged;
    }

    public static void HandleReceivedConfiguration()
    {
        ReceivedConfiguration?.Invoke(default, EventArgs.Empty);
    }

    private void HandleNetworkStatusChanged()
    {
        _serverStatusLabel.Text = Strings.Server.StatusLabel.ToString(ActiveNetworkStatus.ToLocalizedString());
    }

    //Methods
    public void Update()
    {
        if (mShouldOpenCharacterSelection)
        {
            CreateCharacterSelection();
        }

        if (mShouldOpenCharacterCreation)
        {
            CreateCharacterCreation();
        }

        if (_loginWindow.Visible)
        {
            _loginWindow.Update();
        }

        if (_registerWindow.Visible)
        {
            _registerWindow.Update();
        }

        if (_settingsWindow.Visible) // shouldn't this be at "SharedInterface" or something?
        {
            _settingsWindow.Update();
        }
        
        if (_debugUi.Visible) // shouldn't this be at "SharedInterface" or something?
        {
            _debugUi.Update();
        }

        if (_createCharacterWindow.Visible)
        {
            _createCharacterWindow.Update();
        }

        if (SelectCharacterWindow.Visible)
        {
            SelectCharacterWindow.Update();
        }
    }

    public void Reset()
    {
        _loginWindow.Hide();
        _registerWindow.Hide();
        _settingsWindow.Hide();
        _creditsWindow.Hide();
        _forgotPasswordWindow?.Hide();
        _resetPasswordWindow?.Hide();
        _createCharacterWindow?.Hide();
        SelectCharacterWindow?.Hide();
    }

    public void NotifyOpenCharacterSelection(List<Character> characters)
    {
        mShouldOpenCharacterSelection = true;
        SelectCharacterWindow.Characters = [.. characters];
    }

    public void NotifyOpenForgotPassword()
    {
        Reset();
        _forgotPasswordWindow.Show();
    }

    public void NotifyOpenLogin()
    {
        Reset();
        _loginWindow.Show();
    }

    public void OpenResetPassword(string nameEmail)
    {
        Reset();
        _resetPasswordWindow.Target = nameEmail;
        _resetPasswordWindow.Show();
    }

    public void CreateCharacterSelection()
    {
        Reset();
        SelectCharacterWindow.Show();
        mShouldOpenCharacterSelection = false;
    }

    public void NotifyOpenCharacterCreation() => mShouldOpenCharacterCreation = true;

    public void CreateCharacterCreation()
    {
        Reset();
        _createCharacterWindow.Show();
        mShouldOpenCharacterCreation = false;
    }

    internal void SwitchToWindow<TMainMenuWindow>() where TMainMenuWindow : IWindow
    {
        Reset();
        if (typeof(TMainMenuWindow) == typeof(LoginWindow))
        {
            _loginWindow.Show();
        }
        else if (typeof(TMainMenuWindow) == typeof(RegisterWindow))
        {
            _registerWindow.Show();
        }
        else if (typeof(TMainMenuWindow) == typeof(SettingsWindow))
        {
            _settingsWindow.Show(true);
        }
        else if (typeof(TMainMenuWindow) == typeof(CreditsWindow))
        {
            _creditsWindow.Show();
        }
        else if (typeof(TMainMenuWindow) == typeof(SelectCharacterWindow))
        {
            SelectCharacterWindow.Show();
        }
        else if (typeof(TMainMenuWindow) == typeof(CreateCharacterWindow))
        {
            _createCharacterWindow.Show();
        }
    }
    
    public static void ToggleDebugWindow()
    {
        _debugUi.Toggle();
    }

    public static void SetNetworkStatus(NetworkStatus networkStatus, bool resetStatusCheck = false)
    {
        ActiveNetworkStatus = networkStatus;
        NetworkStatusChanged?.Invoke();
        LastNetworkStatusChangeTime = resetStatusCheck ? -1 : Timing.Global.MillisecondsUtc;
    }
}
