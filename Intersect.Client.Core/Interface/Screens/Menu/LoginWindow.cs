using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.Client.MonoGame.Network;
using Intersect.Client.Networking;
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
            IsVisible = false;
            MainMenuInterface.MainMenuWindow.RegisterWindow.IsVisible = true;
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
        if (Networking.Network.InterruptDisconnectsIfConnected())
        {
            // Interface.ShowAlert(Strings.Errors.Disconnected, alertType: AlertType.Error);
            return;
        }

        Networking.Network.Socket.ReceivedConfiguration += (s, a) => TryLogin();
        Networking.Network.Socket.ConnectionFailed += (n, a, d) => _removeLoginEvents();
        Networking.Network.Socket.Disconnected += (n, a) => _removeLoginEvents();
        Networking.Network.TryConnect();
    }

    private void _removeLoginEvents()
    {
        Networking.Network.Socket.ReceivedConfiguration -= (s, a) => TryLogin();
        Networking.Network.Socket.ConnectionFailed -= (n, a, d) => _removeLoginEvents();
        Networking.Network.Socket.Disconnected -= (n, a) => _removeLoginEvents();
    }

    private void TryLogin()
    {
        _removeLoginEvents();

        if (Globals.WaitingOnServer)
        {
            return;
        }

        if (!Networking.Network.IsConnected)
        {
            //Interface.ShowAlert(Strings.Errors.NotConnected, alertType: AlertType.Error);
            return;
        }

        if (!FieldChecking.IsValidUsername(UsernameInput.Text, Strings.Regex.Username))
        {
            //Interface.ShowAlert(Strings.Errors.UsernameInvalid, alertType: AlertType.Error);
            return;
        }

        if (!_useHashedPass && !FieldChecking.IsValidPassword(PasswordInput.Password.ToString(), Strings.Regex.Password))
        {
            //Interface.ShowAlert(Strings.Errors.PasswordInvalid, alertType: AlertType.Error);
            return;
        }

        var password = _hashedPassword;
        if (!_useHashedPass)
        {
            password = PasswordUtils.ComputePasswordHash(PasswordInput.Password.ToString().Trim());
        }

        PacketSender.SendLogin(UsernameInput.Text, password);
        SaveCredentials();
    }

    public void Update()
    {
        bool shouldEnableButtons =
            !Globals.WaitingOnServer &&
            MonoSocket.Instance.CurrentNetworkStatus == Network.NetworkStatus.Online;

        LoginButton.IsEnabled = shouldEnableButtons;
        RegisterButton.IsEnabled = shouldEnableButtons;
    }
}
