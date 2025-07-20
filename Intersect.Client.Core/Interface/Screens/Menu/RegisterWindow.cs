using Intersect.Client.Framework.Input;
using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.Client.MonoGame.Network;
using Intersect.Client.Networking;
using Intersect.Security;
using Intersect.Utilities;
using Microsoft.Xna.Framework.Input;
using MonoGameGum.Forms.Controls;

namespace Intersect.Client.Interface.Screens;

public partial class RegisterWindow
{
    partial void CustomInitialize()
    {
        IsVisible = false;

        WindowTitle.Text = Strings.Registration.Title;
        UsernameLabel.Text = Strings.Registration.Username;
        EmailLabel.Text = Strings.Registration.Email;
        PasswordLabel.Text = Strings.Registration.Password;
        ConfirmPasswordLabel.Text = Strings.Registration.ConfirmPassword;
        RegisterButton.Text = Strings.Registration.Register;
        BackButton.Text = Strings.Registration.Back;

        UsernameInput.KeyDown += (sender, args) =>
        {
            if (args.Key == Keys.Enter) TryRegister();
        };

        UsernameInput.GotFocus += (sender, args) =>
        {
            OpenKeyboardForInput(UsernameInput, KeyboardType.Normal, Strings.Registration.Username);
        };

        EmailInput.KeyDown += (sender, args) =>
        {
            if (args.Key == Keys.Enter) TryRegister();
        };

        EmailInput.GotFocus += (sender, args) =>
        {
            OpenKeyboardForInput(EmailInput, KeyboardType.Email, Strings.Registration.Email);
        };

        PasswordInput.KeyDown += (sender, args) =>
        {
            if (args.Key == Keys.Enter) TryRegister();
        };

        PasswordInput.GotFocus += (sender, args) =>
        {
            OpenKeyboardForInput(PasswordInput, KeyboardType.Password, Strings.Registration.Password);
        };

        ConfirmPasswordInput.KeyDown += (sender, args) =>
        {
            if (args.Key == Keys.Enter) TryRegister();
        };

        ConfirmPasswordInput.GotFocus += (sender, args) =>
        {
            OpenKeyboardForInput(ConfirmPasswordInput, KeyboardType.Password, Strings.Registration.ConfirmPassword);
        };

        RegisterButton.Click += (sender, args) =>
        {
            TryRegister();
        };

        BackButton.Click += (sender, args) =>
        {
            IsVisible = false;
            MainMenuInterface.MainMenuWindow.LoginWindow.IsVisible = true;
            Networking.Network.DebounceClose("returning_to_main_menu");
        };
    }

    private static void OpenKeyboardForInput(TextBox textbox, KeyboardType keyboardType, string description)
    {
        Globals.InputManager.OpenKeyboard(
            keyboardType: keyboardType,
            inputHandler: text => textbox.Text = text ?? string.Empty,
            description: description,
            text: textbox.Text ?? string.Empty,
            inputBounds: new Framework.GenericClasses.Rectangle((int)textbox.X, (int)textbox.Y, (int)textbox.Width, (int)textbox.Height)
        );
    }

    private static void OpenKeyboardForInput(PasswordBox passwordBox, KeyboardType keyboardType, string description)
    {
        Globals.InputManager.OpenKeyboard(
            keyboardType: keyboardType,
            inputHandler: text => passwordBox.Password = text ?? string.Empty,
            description: description,
            text: passwordBox.Password.ToString() ?? string.Empty,
            inputBounds: new Framework.GenericClasses.Rectangle((int)passwordBox.X, (int)passwordBox.Y, (int)passwordBox.Width, (int)passwordBox.Height)
        );
    }

    private void TryRegister()
    {
        if (Globals.WaitingOnServer)
        {
            return;
        }

        if (!Networking.Network.IsConnected)
        {
            InterfaceCore.AlertWindow.ShowError(Strings.Errors.NotConnected);
            return;
        }

        var username = UsernameInput.Text?.Trim();
        if (!FieldChecking.IsValidUsername(username, Strings.Regex.Username))
        {
            InterfaceCore.AlertWindow.ShowError(Strings.Errors.UsernameInvalid);
            return;
        }

        var email = EmailInput.Text?.Trim();
        if (!FieldChecking.IsWellformedEmailAddress(email, Strings.Regex.Email))
        {
            InterfaceCore.AlertWindow.ShowError(Strings.Registration.EmailInvalid);
            return;
        }

        var password = PasswordInput.Password.ToString()?.Trim();
        if (!FieldChecking.IsValidPassword(password, Strings.Regex.Password))
        {
            InterfaceCore.AlertWindow.ShowError(Strings.Errors.PasswordInvalid);
            return;
        }

        if (password != ConfirmPasswordInput.Password.ToString())
        {
            InterfaceCore.AlertWindow.ShowError(Strings.Registration.PasswordMismatch);
            return;
        }

        var passwordHash = PasswordUtils.ComputePasswordHash(password);
        PacketSender.SendUserRegistration(username, passwordHash, email);
        Globals.WaitingOnServer = true;
    }

    public void Update()
    {
        if (!IsVisible)
        {
            return;
        }

        if (!Networking.Network.IsConnected)
        {
            IsVisible = false;
            MainMenuInterface.MainMenuWindow.LoginWindow.IsVisible = true;
            return;
        }

        bool shouldEnableButtons =
            !Globals.WaitingOnServer &&
            MonoSocket.Instance.CurrentNetworkStatus == Network.NetworkStatus.Online;

        RegisterButton.IsEnabled = shouldEnableButtons;
        BackButton.IsEnabled = shouldEnableButtons;
    }
}
