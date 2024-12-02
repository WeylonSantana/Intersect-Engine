using Intersect.Client.Framework.Input;
using Intersect.Client.General;
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

    public bool IsHidden
    {
        get => _loginWindow?.Visible == false;
        set => Toggle(!value);
    }

    public void Load(MenuInterface mainMenu)
    {
        _mainMenu = mainMenu;
        _loginWindow = Interface.LoadContent(Path.Combine("menu", "LoginWindow.xmmp"));
        if (Interface.GetChildById<Label>("_labelLoginTitle", out var labelLoginTitle))
        {
            labelLoginTitle.Text = Strings.LoginWindow.Title;
        }

        if (Interface.GetChildById<Label>("_labelLoginUsername", out var labelLoginUsername))
        {
            labelLoginUsername.Text = Strings.LoginWindow.Username;
        }

        if (Interface.GetChildById<TextBox>("_textboxLoginUsername", out var textboxUsername))
        {
            _textboxLoginUsername = textboxUsername;
            _textboxLoginUsername.TouchDown += _textboxUsername_Clicked;
            Interface.SetInputFocus(_textboxLoginUsername);
        }

        if (Interface.GetChildById<Label>("_labelLoginPassword", out var labelLoginPassword))
        {
            labelLoginPassword.Text = Strings.LoginWindow.Password;
        }

        if (Interface.GetChildById<TextBox>("_textboxLoginPassword", out var textboxPassword))
        {
            _textboxLoginPassword = textboxPassword;
            _textboxLoginPassword.PasswordField = true;
            _textboxLoginPassword.TouchDown += _textboxPassword_Clicked;
            _textboxLoginPassword.TextChanged += _textboxPassword_TextChanged;
        }

        if (Interface.GetChildById<Label>("_labelSave", out var labelSave))
        {
            labelSave.Text = Strings.LoginWindow.SavePassword;
        }

        if (Interface.GetChildById<CheckButton>("_checkboxSave", out var checkboxSave))
        {
            _checkboxSave = checkboxSave;
        }

        if (Interface.GetChildById<Label>("_labelLogin", out var labelLogin))
        {
            labelLogin.Text = Strings.MainMenu.Login;
        }

        if (Interface.GetChildById<Button>("_buttonLogin", out var buttonLogin))
        {
            _buttonLogin = buttonLogin;
            _buttonLogin.Enabled = false;
            _buttonLogin.Click += (sender, args) => TryLogin();
        }

        if (Interface.GetChildById<Label>("_labelForgotPassword", out var labelForgotPassword))
        {
            labelForgotPassword.Text = Strings.LoginWindow.ForgotPassword;
        }

        if (Interface.GetChildById<Button>("_buttonForgotPassword", out var buttonForgotPassword))
        {
            _buttonForgotPassword = buttonForgotPassword;
            _buttonForgotPassword.Click += _buttonForgotPassword_Clicked;
        }

        if (Interface.GetChildById<Label>("_labelRegister", out var labelRegister))
        {
            labelRegister.Text = Strings.MainMenu.Register;
        }

        if (Interface.GetChildById<Button>("_buttonRegister", out var buttonRegister))
        {
            _buttonRegister = buttonRegister;
            _buttonRegister.Enabled = false;
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

        if (Interface.GetChildById<Label>("_labelSettings", out var labelSettings))
        {
            labelSettings.Text = Strings.MainMenu.Settings;
        }

        if (Interface.GetChildById<Button>("_buttonSettings", out var buttonSettings))
        {
            buttonSettings.Click += (sender, args) => _mainMenu.SwitchToWindow<SettingsWindow>();
        }

        if (Interface.GetChildById<Label>("_labelCredits", out var labelCredits))
        {
            labelCredits.Text = Strings.MainMenu.Credits;
        }

        if (Interface.GetChildById<Button>("_buttonCredits", out var buttonCredits))
        {
            buttonCredits.Click += (sender, args) => _mainMenu.SwitchToWindow<CreditsWindow>();
        }

        if (Interface.GetChildById<Label>("_labelExit", out var labelExit))
        {
            labelExit.Text = Strings.MainMenu.Exit;
        }

        if (Interface.GetChildById<Button>("_buttonExit", out var buttonExit))
        {
            buttonExit.Click += (sender, args) =>
            {
                Log.Info("User clicked exit button.");
                Globals.IsRunning = false;
            };
        }

        LoadCredentials();
    }

    public void Toggle(bool value)
    {
        if (_loginWindow == default)
        {
            return;
        }

        _loginWindow.Visible = value;

        if (_loginWindow.Visible)
        {
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
    }

    public void Update()
    {
        if (_buttonLogin != default)
        {
            _buttonLogin.Enabled = MenuInterface.ActiveNetworkStatus == NetworkStatus.Online && !Globals.WaitingOnServer;
        }

        if (_buttonRegister != default)
        {
            _buttonRegister.Enabled = MenuInterface.ActiveNetworkStatus == NetworkStatus.Online && !Globals.WaitingOnServer;
        }
    }

    private void SaveCredentials()
    {
        string username = string.Empty, password = string.Empty;

        if (_textboxLoginUsername == default || _textboxLoginPassword == default)
        {
            return;
        }

        if (_checkboxSave?.IsChecked == true)
        {
            username = _textboxLoginUsername.Text.Trim();
            password = _useSavedPass ? _savedPass : PasswordUtils.ComputePasswordHash(_textboxLoginPassword.Text.Trim());
        }

        Globals.Database.SavePreference("Username", username);
        Globals.Database.SavePreference("Password", password);
    }

    private void LoadCredentials()
    {
        var name = Globals.Database.LoadPreference("Username");
        if (string.IsNullOrEmpty(name) || _textboxLoginUsername == default || _textboxLoginPassword == default)
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

        if (!FieldChecking.IsValidUsername(_textboxLoginUsername.Text, Strings.Regex.Username))
        {
            Interface.ShowError(Strings.Errors.UsernameInvalid);
            return;
        }

        if (!FieldChecking.IsValidPassword(_textboxLoginPassword.Text, Strings.Regex.Password))
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
            _storedPassword = PasswordUtils.ComputePasswordHash(_textboxLoginPassword.Text.Trim());
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

    private void _loginConnectionFailed(INetworkLayerInterface nli, ConnectionEventArgs args, bool denied) => _removeLoginEvents();

    private void _loginDisconnected(INetworkLayerInterface nli, ConnectionEventArgs args) => _removeLoginEvents();

    private void _loginConnected(object? sender, EventArgs eventArgs)
    {
        PacketSender.SendLogin(_textboxLoginUsername!.Text, _storedPassword);
        SaveCredentials();
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

    private void _registerConnectionFailed(INetworkLayerInterface nli, ConnectionEventArgs args, bool denied) => _removeRegisterEvents();

    private void _registerDisconnected(INetworkLayerInterface nli, ConnectionEventArgs args) => _removeRegisterEvents();

    private void _registerConnected(object? sender, EventArgs eventArgs)
    {
        _removeRegisterEvents();
        _mainMenu.SwitchToWindow<RegisterWindow>();
    }
    #endregion
}