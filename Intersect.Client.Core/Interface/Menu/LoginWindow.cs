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

public sealed partial class LoginWindow : Window
{
    private TextBox? _textboxUsername;
    private TextBox? _textboxPassword;
    private CheckButton? _checkboxSave;
    private Button? _buttonLogin;
    private Button? _buttonForgotPassword;
    private Button? _buttonRegister;

    private string? _username;
    private string? _passwordHash;

    public LoginWindow(Desktop? desktop = default) : base(Path.Combine("menu", "LoginWindow.xmmp"), desktop)
    {
        Connect(reloading: false);
    }

    private void Connect(bool reloading)
    {
        Root.FindChildById<Label>(TITLE_LABEL)?.SetText(Strings.LoginWindow.Title);
        Root.FindChildById<Label>(USERNAME_LABEL)?.SetText(Strings.LoginWindow.Username);
        Root.FindChildById<Label>(PASSWORD_LABEL)?.SetText(Strings.LoginWindow.Password);

        if (Root.FindChildById<TextBox>(USERNAME_TEXTBOX, out var textboxUsername))
        {
            _textboxUsername = textboxUsername;
            _textboxUsername.TouchDown += _textboxUsername_Clicked;
            _textboxUsername.TextChanged += _textboxUsername_TextChanged;
            if (!reloading)
            {
                Interface.SetInputFocus(_textboxUsername);
            }
        }

        if (Root.FindChildById<TextBox>(PASSWORD_TEXTBOX, out var textboxPassword))
        {
            _textboxPassword = textboxPassword;
            _textboxPassword.PasswordField = true;
            _textboxPassword.TouchDown += _textboxPassword_Clicked;
            _textboxPassword.TextChanged += _textboxPassword_TextChanged;
        }

        if (Root.FindChildById<CheckButton>(SAVE_CREDENTIALS_CHECK, out var checkboxSave))
        {
            _checkboxSave = checkboxSave;
            _checkboxSave.SetText(Strings.LoginWindow.SavePassword);
        }

        if (Root.FindChildById<Button>(LOGIN_BUTTON, out var buttonLogin))
        {
            _buttonLogin = buttonLogin;
            _buttonLogin.Enabled = false;
            _buttonLogin.Click += (sender, args) => TryLogin();
            _buttonLogin.SetText(Strings.LoginWindow.Login);
        }

        if (Root.FindChildById<Button>(FORGOT_PASSWORD_BUTTON, out var buttonForgotPassword))
        {
            _buttonForgotPassword = buttonForgotPassword;
            _buttonForgotPassword.Click += _buttonForgotPassword_Clicked;
            _buttonForgotPassword.SetText(Strings.LoginWindow.ForgotPassword);
        }

        if (Root.FindChildById<Button>(REGISTER_BUTTON, out var buttonRegister))
        {
            _buttonRegister = buttonRegister;
            _buttonRegister.Enabled = false;
            _buttonRegister.SetText(Strings.LoginWindow.Register);
            _buttonRegister.Click += (sender, args) =>
            {
                if (Networking.Network.InterruptDisconnectsIfConnected())
                {
                    Interface.MenuUi?.SwitchToWindow<RegisterWindow>();
                }
                else
                {
                    _addRegisterEvents();
                    Networking.Network.TryConnect();
                }
            };
        }

        if (Root.FindChildById<Button>(SETTINGS_BUTTON, out var buttonSettings))
        {
            buttonSettings.Click += (sender, args) => Interface.MenuUi?.SwitchToWindow<SettingsWindow>();
            buttonSettings.SetText(Strings.LoginWindow.Settings);
        }

        if (Root.FindChildById<Button>(CREDITS_BUTTON, out var buttonCredits))
        {
            buttonCredits.Click += (sender, args) => Interface.MenuUi?.SwitchToWindow<CreditsWindow>();
            buttonCredits.SetText(Strings.LoginWindow.Credits);
        }

        if (Root.FindChildById<Button>(EXIT_BUTTON, out var buttonExit))
        {
            buttonExit.SetText(Strings.LoginWindow.Exit);
            buttonExit.Click += (sender, args) =>
            {
                Log.Info("User clicked exit button.");
                Globals.IsRunning = false;
            };
        }

        LoadCredentials();

        if (reloading)
        {
            var focusedKeyboardWidget = Interface.FocusedKeyboardWidget;
            if (!string.IsNullOrWhiteSpace(focusedKeyboardWidget?.Id))
            {
                var existingWidget = Root.FindChildById(focusedKeyboardWidget.Id);
                if (existingWidget != default)
                {
                    Interface.FocusedKeyboardWidget = existingWidget;
                }
            }
        }
    }

    protected override void OnReload()
    {
        base.OnReload();

        Connect(reloading: true);
    }

    public override void Show()
    {
        base.Show();

        if (_buttonForgotPassword != default)
        {
            _buttonForgotPassword.Visible = !Options.Instance.SmtpValid;
        }

        if (_textboxUsername != default && string.IsNullOrWhiteSpace(_textboxUsername.Text))
        {
            Interface.FocusedKeyboardWidget = _textboxUsername;
        }
        else if (_textboxPassword != default && string.IsNullOrWhiteSpace(_textboxPassword.Text))
        {
            Interface.FocusedKeyboardWidget = _textboxPassword;
        }
        else if (_buttonLogin != default)
        {
            Interface.FocusedKeyboardWidget = _buttonLogin;
        }
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
        string? username = _username;
        string? passwordHash = _passwordHash;

        if (_textboxUsername == default || _textboxPassword == default || _checkboxSave == default)
        {
            return;
        }

        if (!_checkboxSave.IsChecked)
        {
            username = default;
            passwordHash = default;
        }

        Globals.Database.SavePreference("Username", username ?? string.Empty);
        Globals.Database.SavePreference("Password", passwordHash ?? string.Empty);
    }

    private void LoadCredentials()
    {
        _username = Globals.Database.LoadPreference("Username");
        if (string.IsNullOrWhiteSpace(_username))
        {
            return;
        }

        if (_textboxUsername == default || _textboxPassword == default)
        {
            return;
        }

        _textboxUsername.Text = _username;

        _passwordHash = Globals.Database.LoadPreference("Password");
        if (string.IsNullOrEmpty(_passwordHash))
        {
            return;
        }

        _textboxPassword.Text = "************************";

        if (_checkboxSave != default)
        {
            _checkboxSave.IsChecked = true;
        }
    }

    #region Input Handling

    private void _textboxUsername_Clicked(object? sender, EventArgs e)
    {
        if (_textboxUsername == default)
        {
            return;
        }

        Globals.InputManager.OpenKeyboard(
            KeyboardType.Normal,
            text => _textboxUsername.Text = text ?? string.Empty,
            Strings.LoginWindow.Username,
            _textboxUsername.Text,
            inputBounds: new Framework.GenericClasses.Rectangle(
                _textboxUsername.Bounds.X,
                _textboxUsername.Bounds.Y,
                _textboxUsername.Bounds.Width,
                _textboxUsername.Bounds.Height
            )
        );
    }

    private void _textboxPassword_Clicked(object? sender, EventArgs e)
    {
        if (_textboxPassword == default)
        {
            return;
        }

        Globals.InputManager.OpenKeyboard(
            KeyboardType.Password,
            text => _textboxPassword.Text = text ?? string.Empty,
            Strings.LoginWindow.Password,
            _textboxPassword.Text
        );
    }

    private void _textboxUsername_TextChanged(object? sender, EventArgs e)
    {
        _username = _textboxUsername?.Text;

        if (_passwordHash == default)
        {
            return;
        }

        if (_textboxPassword != default)
        {
            _textboxPassword.Text = string.Empty;
        }

        _passwordHash = default;
    }

    private void _textboxPassword_TextChanged(object? sender, EventArgs e)
    {
        _passwordHash = default;
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

        if (_textboxPassword == default)
        {
            return;
        }

        if (!FieldChecking.IsValidUsername(_username, Strings.Regex.Username))
        {
            Interface.ShowError(Strings.Errors.UsernameInvalid);
            return;
        }

        if (_passwordHash == default)
        {
            var password = _textboxPassword.Text;

            if (!FieldChecking.IsValidPassword(password, Strings.Regex.Password))
            {
                Interface.ShowError(Strings.Errors.PasswordInvalid);
                return;
            }

            _passwordHash = PasswordUtils.ComputePasswordHash(password);
        }

        Globals.WaitingOnServer = true;
        if (Networking.Network.InterruptDisconnectsIfConnected())
        {
            Interface.MenuUi?.SwitchToWindow<LoginWindow>();
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

    private void _loginConnectionFailed(INetworkLayerInterface nli, ConnectionEventArgs args, bool denied)
    {
        _removeLoginEvents();
    }

    private void _loginDisconnected(INetworkLayerInterface nli, ConnectionEventArgs args) => _removeLoginEvents();

    private void _loginConnected(object? sender, EventArgs eventArgs)
    {
        if (_username == default)
        {
            Log.Error("Attempted to log in with null username");
            return;
        }

        if (_passwordHash == default)
        {
            Log.Error("Attempted to log in with null password hash");
            return;
        }

        PacketSender.SendLogin(_username, _passwordHash);
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

    private void _registerConnectionFailed(INetworkLayerInterface nli, ConnectionEventArgs args, bool denied)
        => _removeRegisterEvents();

    private void _registerDisconnected(INetworkLayerInterface nli, ConnectionEventArgs args) =>
        _removeRegisterEvents();

    private void _registerConnected(object? sender, EventArgs eventArgs)
    {
        _removeRegisterEvents();
        Interface.MenuUi?.SwitchToWindow<RegisterWindow>();
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