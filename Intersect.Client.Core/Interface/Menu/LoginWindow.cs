using Intersect.Client.Framework.Input;
using Intersect.Client.General;
using Intersect.Client.Interface.Extensions;
using Intersect.Client.Interface.Shared;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Logging;
using Intersect.Network;
using Intersect.Network.Events;
using Intersect.Security;
using Intersect.Utilities;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Menu;

public partial class LoginWindow : IMainMenuWindow
{
    private MenuInterface _mainMenu = null!;
    private Widget? _loginWindow;
    private TextBox? _textboxLoginUsername;
    private TextBox? _textboxLoginPassword;
    private CheckButton? _checkboxSave;
    private Button? _buttonLogin;
    private Button? _buttonForgotPassword;
    private Button? _buttonRegister;

    private bool _useSavedPass;
    private string _savedPass = string.Empty;
    private string _storedPassword = string.Empty;

    public bool Visible => _loginWindow?.Visible ?? false;

    public void Load(MenuInterface mainMenu)
    {
        _mainMenu = mainMenu;
        _loginWindow = Interface.LoadContent(Path.Combine("menu", "LoginWindow.xmmp"));
        _loginWindow.FindChildById<Label>(TITLE_LABEL)?.SetText(Strings.LoginWindow.Title);
        _loginWindow.FindChildById<Label>(USERNAME_LABEL)?.SetText(Strings.LoginWindow.Username);
        _loginWindow.FindChildById<Label>(PASSWORD_LABEL)?.SetText(Strings.LoginWindow.Password);

        if (_loginWindow.FindChildById<TextBox>(USERNAME_TEXTBOX, out var textboxUsername))
        {
            _textboxLoginUsername = textboxUsername;
            _textboxLoginUsername.TouchDown += _textboxUsername_Clicked;
            Interface.SetInputFocus(_textboxLoginUsername);
        }

        if (_loginWindow.FindChildById<TextBox>(PASSWORD_TEXTBOX, out var textboxPassword))
        {
            _textboxLoginPassword = textboxPassword;
            _textboxLoginPassword.PasswordField = true;
            _textboxLoginPassword.TouchDown += _textboxPassword_Clicked;
            _textboxLoginPassword.TextChanged += _textboxPassword_TextChanged;
        }

        if (_loginWindow.FindChildById<CheckButton>(SAVE_CREDENTIALS_CHECK, out var checkboxSave))
        {
            _checkboxSave = checkboxSave;
            _checkboxSave.SetText(Strings.LoginWindow.SavePassword);
        }

        if (_loginWindow.FindChildById<Button>(LOGIN_BUTTON, out var buttonLogin))
        {
            _buttonLogin = buttonLogin;
            _buttonLogin.Enabled = false;
            _buttonLogin.Click += (sender, args) => TryLogin();
            _buttonLogin.SetText(Strings.LoginWindow.Login);
        }

        if (_loginWindow.FindChildById<Button>(FORGOT_PASSWORD_BUTTON, out var buttonForgotPassword))
        {
            _buttonForgotPassword = buttonForgotPassword;
            _buttonForgotPassword.Click += _buttonForgotPassword_Clicked;
            _buttonForgotPassword.SetText(Strings.LoginWindow.ForgotPassword);
        }

        if (_loginWindow.FindChildById<Button>(REGISTER_BUTTON, out var buttonRegister))
        {
            _buttonRegister = buttonRegister;
            _buttonRegister.Enabled = false;
            _buttonRegister.SetText(Strings.LoginWindow.Register);
            _buttonRegister.Click += (sender, args) =>
            {
                if (Networking.Network.InterruptDisconnectsIfConnected())
                {
                    _mainMenu.SwitchToWindow<RegisterWindow>();
                }
                else
                {
                    _addRegisterEvents();
                    Networking.Network.TryConnect();
                }
            };
        }

        if (_loginWindow.FindChildById<Button>(SETTINGS_BUTTON, out var buttonSettings))
        {
            buttonSettings.Click += (sender, args) => _mainMenu.SwitchToWindow<SettingsWindow>();
            buttonSettings.SetText(Strings.LoginWindow.Settings);
        }

        if (_loginWindow.FindChildById<Button>(CREDITS_BUTTON, out var buttonCredits))
        {
            buttonCredits.Click += (sender, args) => _mainMenu.SwitchToWindow<CreditsWindow>();
            buttonCredits.SetText(Strings.LoginWindow.Credits);
        }

        if (_loginWindow.FindChildById<Button>(EXIT_BUTTON, out var buttonExit))
        {
            buttonExit.SetText(Strings.LoginWindow.Exit);
            buttonExit.Click += (sender, args) =>
            {
                Log.Info("User clicked exit button.");
                Globals.IsRunning = false;
            };
        }

        LoadCredentials();
    }

    public void Show()
    {
        if (_loginWindow == default)
        {
            return;
        }

        _loginWindow.Visible = true;

        if (_buttonForgotPassword?.Visible == false)
        {
            _buttonForgotPassword.Visible = !Options.Instance.SmtpValid;
        }

        if (string.IsNullOrWhiteSpace(_textboxLoginUsername?.Text))
        {
            Interface.SetInputFocus(_textboxLoginUsername);
        }
        else
        {
            Interface.SetInputFocus(_textboxLoginPassword);
        }
    }

    public void Hide()
    {
        if (_loginWindow == default)
        {
            return;
        }

        _loginWindow.Visible = false;
    }

    public void Update()
    {
        if (_buttonLogin != default)
        {
            _buttonLogin.Enabled =
                MenuInterface.ActiveNetworkStatus == NetworkStatus.Online
                && !Globals.WaitingOnServer;
        }

        if (_buttonRegister != default)
        {
            _buttonRegister.Enabled =
                MenuInterface.ActiveNetworkStatus == NetworkStatus.Online
                && !Globals.WaitingOnServer;
        }
    }

    private void SaveCredentials()
    {
        string username = string.Empty,
            password = string.Empty;

        if (_textboxLoginUsername == default || _textboxLoginPassword == default)
        {
            return;
        }

        if (_checkboxSave?.IsChecked == true)
        {
            username = _textboxLoginUsername.Text.Trim();
            password = _useSavedPass
                ? _savedPass
                : PasswordUtils.ComputePasswordHash(_textboxLoginPassword.Text.Trim());
        }

        Globals.Database.SavePreference("Username", username);
        Globals.Database.SavePreference("Password", password);
    }

    private void LoadCredentials()
    {
        var name = Globals.Database.LoadPreference("Username");
        if (
            string.IsNullOrEmpty(name)
            || _textboxLoginUsername == default
            || _textboxLoginPassword == default
        )
        {
            return;
        }

        _textboxLoginUsername.Text = name;
        var pass = Globals.Database.LoadPreference("Password");
        if (string.IsNullOrEmpty(pass))
        {
            return;
        }

        _textboxLoginPassword.Text = "****************";
        _savedPass = pass;
        _useSavedPass = true;
        if (_checkboxSave != default)
        {
            _checkboxSave.IsChecked = true;
        }
    }

    #region Input Handling

    private void _textboxUsername_Clicked(object? sender, EventArgs e)
    {
        if (_textboxLoginUsername == default)
        {
            return;
        }

        Globals.InputManager.OpenKeyboard(
            KeyboardType.Normal,
            text => _textboxLoginUsername.Text = text ?? string.Empty,
            Strings.LoginWindow.Username,
            _textboxLoginUsername.Text,
            inputBounds: new Framework.GenericClasses.Rectangle(
                _textboxLoginUsername.Bounds.X,
                _textboxLoginUsername.Bounds.Y,
                _textboxLoginUsername.Bounds.Width,
                _textboxLoginUsername.Bounds.Height
            )
        );
    }

    private void _textboxPassword_Clicked(object? sender, EventArgs e)
    {
        if (_textboxLoginPassword == default)
        {
            return;
        }

        Globals.InputManager.OpenKeyboard(
            KeyboardType.Password,
            text => _textboxLoginPassword.Text = text ?? string.Empty,
            Strings.LoginWindow.Password,
            _textboxLoginPassword.Text
        );
    }

    private void _textboxPassword_TextChanged(object? sender, EventArgs e)
    {
        _useSavedPass = false;
    }

    private static void _buttonForgotPassword_Clicked(object? sender, EventArgs e)
    {
        Interface.MenuUi?.NotifyOpenForgotPassword();
    }

    #endregion

    # region Login Handler

    private void TryLogin()
    {
        if (Globals.WaitingOnServer)
        {
            return;
        }

        if (_textboxLoginUsername == default || _textboxLoginPassword == default)
        {
            return;
        }

        if (!Networking.Network.IsConnected)
        {
            Interface.ShowError(Strings.Errors.NotConnected);
            return;
        }

        if (!FieldChecking.IsValidUsername(_textboxLoginUsername?.Text, Strings.Regex.Username))
        {
            Interface.ShowError(Strings.Errors.UsernameInvalid);
            return;
        }

        if (!FieldChecking.IsValidPassword(_textboxLoginPassword?.Text, Strings.Regex.Password))
        {
            if (!_useSavedPass)
            {
                Interface.ShowError(Strings.Errors.PasswordInvalid);
                return;
            }
        }

        _storedPassword = _savedPass;
        if (!_useSavedPass)
        {
            _storedPassword = PasswordUtils.ComputePasswordHash(_textboxLoginPassword?.Text.Trim());
        }

        Globals.WaitingOnServer = true;
        if (Networking.Network.InterruptDisconnectsIfConnected())
        {
            _mainMenu.SwitchToWindow<LoginWindow>();
        }
        else
        {
            _addLoginEvents();
            Networking.Network.TryConnect();
        }
    }

    private void _addLoginEvents()
    {
        MenuInterface.ReceivedConfiguration += _loginConnected;
        Networking.Network.Socket.ConnectionFailed += _loginConnectionFailed;
        Networking.Network.Socket.Disconnected += _loginDisconnected;
    }

    private void _removeLoginEvents()
    {
        MenuInterface.ReceivedConfiguration -= _loginConnected;
        Networking.Network.Socket.ConnectionFailed -= _loginConnectionFailed;
        Networking.Network.Socket.Disconnected -= _loginDisconnected;
    }

    private void _loginConnectionFailed(
        INetworkLayerInterface nli,
        ConnectionEventArgs args,
        bool denied
    ) => _removeLoginEvents();

    private void _loginDisconnected(INetworkLayerInterface nli, ConnectionEventArgs args) =>
        _removeLoginEvents();

    private void _loginConnected(object? sender, EventArgs eventArgs)
    {
        PacketSender.SendLogin(_textboxLoginUsername!.Text, _storedPassword);
        SaveCredentials();
        //MYRA-TODO
        //ChatboxMsg.ClearMessages();
        _removeLoginEvents();
    }

    #endregion

    #region Register Handler

    private void _addRegisterEvents()
    {
        MenuInterface.ReceivedConfiguration += _registerConnected;
        Networking.Network.Socket.ConnectionFailed += _registerConnectionFailed;
        Networking.Network.Socket.Disconnected += _registerDisconnected;
    }

    private void _removeRegisterEvents()
    {
        MenuInterface.ReceivedConfiguration -= _registerConnected;
        Networking.Network.Socket.ConnectionFailed -= _registerConnectionFailed;
        Networking.Network.Socket.Disconnected -= _registerDisconnected;
    }

    private void _registerConnectionFailed(
        INetworkLayerInterface nli,
        ConnectionEventArgs args,
        bool denied
    ) => _removeRegisterEvents();

    private void _registerDisconnected(INetworkLayerInterface nli, ConnectionEventArgs args) =>
        _removeRegisterEvents();

    private void _registerConnected(object? sender, EventArgs eventArgs)
    {
        _removeRegisterEvents();
        _mainMenu.SwitchToWindow<RegisterWindow>();
    }

    #endregion

    #region Constants

    private const string TITLE_LABEL = nameof(TITLE_LABEL);
    private const string USERNAME_LABEL = nameof(USERNAME_LABEL);
    private const string USERNAME_TEXTBOX = nameof(USERNAME_TEXTBOX);
    private const string PASSWORD_LABEL = nameof(PASSWORD_LABEL);
    private const string PASSWORD_TEXTBOX = nameof(PASSWORD_TEXTBOX);
    private const string SAVE_CREDENTIALS_CHECK = nameof(SAVE_CREDENTIALS_CHECK);
    private const string REGISTER_BUTTON = nameof(REGISTER_BUTTON);
    private const string LOGIN_BUTTON = nameof(LOGIN_BUTTON);
    private const string FORGOT_PASSWORD_BUTTON = nameof(FORGOT_PASSWORD_BUTTON);
    private const string SETTINGS_BUTTON = nameof(SETTINGS_BUTTON);
    private const string CREDITS_BUTTON = nameof(CREDITS_BUTTON);
    private const string EXIT_BUTTON = nameof(EXIT_BUTTON);

    #endregion
}