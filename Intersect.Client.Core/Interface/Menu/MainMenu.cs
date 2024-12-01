using Intersect.Client.Core;
using Intersect.Client.Interface.Shared;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Network;
using Intersect.Utilities;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Menu;

public partial class MainMenu
{
    private readonly LoginWindow? _loginWindow;
    private readonly RegistrationWindow _registerWindow;
    private readonly ForgotPasswordWindow _forgotPasswordWindow;
    private readonly ResetPasswordWindow _resetPasswordWindow;
    private readonly CreateCharacterWindow _createCharacterWindow;
    private readonly SettingsWindow _settingsWindow;
    private readonly CreditsWindow _creditsWindow;
    public readonly SelectCharacterWindow SelectCharacterWindow;

    private readonly Project _serverStatusArea;
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

    public MainMenu()
    {
        _loginWindow = new LoginWindow();
        _loginWindow.Load(this);

        _serverStatusArea = Interface.LoadContent(Path.Combine("menu", "ServerStatus.xmmp"));
        Interface.AddElement(_serverStatusArea.Root);

        if (Interface.GetChildById<Label>("_serverStatusLabel", out var label))
        {
            _serverStatusLabel = label;
            _serverStatusLabel.Text = Strings.Server.StatusLabel.ToString(ActiveNetworkStatus.ToLocalizedString());
            _serverStatusLabel.Visible = !ClientContext.IsSinglePlayer;
        }

        NetworkStatusChanged += HandleNetworkStatusChanged;

        //_registerWindow = new RegistrationWindow(_menuCanvas, this);
        //_forgotPasswordWindow = new ForgotPasswordWindow(_menuCanvas, this);
        //_resetPasswordWindow = new ResetPasswordWindow(_menuCanvas, this);
        //SelectCharacterWindow = new SelectCharacterWindow(_menuCanvas, this);
        //_createCharacterWindow = new CreateCharacterWindow(_menuCanvas, this, SelectCharacterWindow);
        //_settingsWindow = new SettingsWindow(_menuCanvas, this, null);
        //_creditsWindow = new CreditsWindow(_menuCanvas, this);
    }

    ~MainMenu()
    {
        NetworkStatusChanged -= HandleNetworkStatusChanged;
    }

    public static void HandleReceivedConfiguration()
    {
        ReceivedConfiguration?.Invoke(default, EventArgs.Empty);
    }

    private void HandleNetworkStatusChanged()
    {
        _serverStatusLabel.Text = Strings.Server.StatusLabel.ToString(MainMenu.ActiveNetworkStatus.ToLocalizedString());
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

        if (_loginWindow?.IsHidden == false)
        {
            _loginWindow.Update();
        }

        return;

        if (!_createCharacterWindow.IsHidden)
        {
            _createCharacterWindow.Update();
        }

        if (!_registerWindow.IsHidden)
        {
            _registerWindow.Update();
        }

        if (!SelectCharacterWindow.IsHidden)
        {
            SelectCharacterWindow.Update();
        }

        _settingsWindow.Update();
    }

    public void Reset()
    {
        _loginWindow?.Toggle();
        _registerWindow.Hide();
        _settingsWindow.Hide();
        _creditsWindow.Hide();
        _forgotPasswordWindow.Hide();
        _resetPasswordWindow.Hide();
        _createCharacterWindow.Hide();
        SelectCharacterWindow.Hide();
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
        _loginWindow?.Toggle();
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
        _createCharacterWindow.Init();
        mShouldOpenCharacterCreation = false;
    }

    internal void SwitchToWindow<TMainMenuWindow>() where TMainMenuWindow : IMainMenuWindow
    {
        Reset();
        if (typeof(TMainMenuWindow) == typeof(LoginWindow))
        {
            _loginWindow?.Toggle();
        }
        else if (typeof(TMainMenuWindow) == typeof(RegistrationWindow))
        {
            _registerWindow.Show();
        }
        else if (typeof(TMainMenuWindow) == typeof(SettingsWindow))
        {
            _settingsWindow.Show();
        }
        else if (typeof(TMainMenuWindow) == typeof(CreditsWindow))
        {
            _creditsWindow.Show();
        }
    }

    public static void SetNetworkStatus(NetworkStatus networkStatus, bool resetStatusCheck = false)
    {
        ActiveNetworkStatus = networkStatus;
        NetworkStatusChanged?.Invoke();
        LastNetworkStatusChangeTime = resetStatusCheck ? -1 : Timing.Global.MillisecondsUtc;
    }
}
