using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.Client.MonoGame.Network;
using Intersect.Client.Networking;
using Intersect.Network;
using Intersect.Network.Events;
using Intersect.Security;
using Intersect.Utilities;
using Microsoft.Xna.Framework.Input;

namespace Intersect.Client.Interface.Screens;

public partial class LoginWindow
{
    private string _hashedPassword = string.Empty;
    private bool _useHashedPass = false;

    partial void CustomInitialize()
    {
        WindowTitle.Text = Strings.LoginWindow.Title;
        UsernameLabel.Text = Strings.LoginWindow.Username;
        PasswordLabel.Text = Strings.LoginWindow.Password;
        SaveCredentialsCheckbox.Text = Strings.LoginWindow.SavePassword;
        LoginButton.Text = Strings.LoginWindow.Login;
        RegisterButton.Text = Strings.LoginWindow.Register;
        ForgotPasswordButton.Text = Strings.LoginWindow.ForgotPassword;

        if ((!Options.IsLoaded || !Options.Instance.SmtpValid) && ForgotPasswordButton.IsVisible)
        {
            ForgotPasswordButton.IsVisible = false;
        }

        UsernameInput.KeyDown += (sender, args) =>
        {
            if (args.Key == Keys.Enter) ConnectAndTryLogin();
        };

        PasswordInput.KeyDown += (sender, args) =>
        {
            if (args.Key == Keys.Enter) ConnectAndTryLogin();
        };

        PasswordInput.PasswordChanged += (sender, args) =>
        {
            _useHashedPass = false;
        };

        LoginButton.Click += (sender, args) =>
        {
            ConnectAndTryLogin();
        };

        RegisterButton.Click += (sender, args) =>
        {
            if (Networking.Network.InterruptDisconnectsIfConnected())
            {
                ShowRegisterWindow();
                return;
            }

            Networking.Network.Socket.ReceivedConfiguration += ShowRegisterWindow;
            Networking.Network.Socket.ConnectionFailed += RemoveRegisterEvents;
            Networking.Network.Socket.Disconnected += RemoveRegisterEvents;
            Networking.Network.TryConnect();
        };

        LoadCredentials();
    }

    private void LoadCredentials()
    {
        var name = Globals.Database?.LoadPreference("Username");
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        UsernameInput.Text = name;

        var pass = Globals.Database?.LoadPreference("Password");
        if (string.IsNullOrEmpty(pass))
        {
            return;
        }

        PasswordInput.Password = "****************";
        SaveCredentialsCheckbox.IsChecked = true;
        _useHashedPass = true;
        _hashedPassword = pass;
    }

    private void SaveCredentials()
    {
        string? username = default, password = default;

        if (SaveCredentialsCheckbox.IsChecked == true)
        {
            username = UsernameInput.Text.Trim();
            password = _useHashedPass ? _hashedPassword : PasswordUtils.ComputePasswordHash(PasswordInput.Password.ToString().Trim());
        }

        Globals.Database?.SavePreference("Username", username);
        Globals.Database?.SavePreference("Password", password);
    }

    private void ConnectAndTryLogin()
    {
        if (Globals.WaitingOnServer)
        {
            InterfaceCore.AlertWindow.ShowError(Strings.Errors.WaitingForServer);
            return;
        }

        if (!FieldChecking.IsValidUsername(UsernameInput.Text, Strings.Regex.Username))
        {
            InterfaceCore.AlertWindow.ShowError(Strings.Errors.UsernameInvalid);
            return;
        }

        if (!_useHashedPass && !FieldChecking.IsValidPassword(PasswordInput.Password.ToString(), Strings.Regex.Password))
        {
            InterfaceCore.AlertWindow.ShowError(Strings.Errors.PasswordInvalid);
            return;
        }

        Networking.Network.Socket.ReceivedConfiguration += TryLogin;
        Networking.Network.Socket.ConnectionFailed += RemoveLoginEvents;
        Networking.Network.Socket.Disconnected += RemoveLoginEvents;
        Networking.Network.TryConnect();
    }

    private void TryLogin(object? sender = default, EventArgs? args = default)
    {
        RemoveLoginEvents();

        if (!Networking.Network.IsConnected)
        {
            InterfaceCore.AlertWindow.ShowError(Strings.Errors.NotConnected);
            return;
        }

        var password = _hashedPassword;
        if (!_useHashedPass)
        {
            password = PasswordUtils.ComputePasswordHash(PasswordInput.Password.ToString().Trim());
        }

        PacketSender.SendLogin(UsernameInput.Text, password);
        InterfaceCore.AlertWindow.OnOpened += OnLoginError;
        SaveCredentials();
    }

    private void RemoveLoginEvents(INetworkLayerInterface? network, ConnectionEventArgs? args, bool denied)
    {
        Networking.Network.Socket.ReceivedConfiguration -= TryLogin;
        Networking.Network.Socket.ConnectionFailed -= RemoveLoginEvents;
        Networking.Network.Socket.Disconnected -= RemoveLoginEvents;
    }

    private void RemoveLoginEvents(INetworkLayerInterface? network = default, ConnectionEventArgs? args = default)
        => RemoveLoginEvents(network, args, false);

    private void OnLoginError(object? sender, EventArgs e)
    {
        // if the alert window shows an error, we want to reset the login window
        InterfaceCore.AlertWindow.OnOpened -= OnLoginError;

        // keep waiting until debounce resets
        Globals.WaitingOnServer = true;
        Networking.Network.DebounceClose("login_failed");
    }

    private void ShowRegisterWindow(object? sender = default, EventArgs? e = default)
    {
        RemoveRegisterEvents();
        IsVisible = false;
        MainMenuInterface.MainMenuWindow.RegisterWindow.IsVisible = true;
    }

    private void RemoveRegisterEvents(INetworkLayerInterface? network, ConnectionEventArgs? args, bool denied)
    {
        Networking.Network.Socket.ReceivedConfiguration -= ShowRegisterWindow;
        Networking.Network.Socket.ConnectionFailed -= RemoveRegisterEvents;
        Networking.Network.Socket.Disconnected -= RemoveRegisterEvents;
    }

    private void RemoveRegisterEvents(INetworkLayerInterface? network = default, ConnectionEventArgs? args = default)
        => RemoveRegisterEvents(network, args, false);

    public void Update()
    {
        if (!IsVisible)
        {
            return;
        }

        bool shouldEnableButtons =
            !Globals.WaitingOnServer &&
            MonoSocket.Instance.CurrentNetworkStatus == Network.NetworkStatus.Online;

        LoginButton.IsEnabled = shouldEnableButtons;
        RegisterButton.IsEnabled = shouldEnableButtons;
    }
}
