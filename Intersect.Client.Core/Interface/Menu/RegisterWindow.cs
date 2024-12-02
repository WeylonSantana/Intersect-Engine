using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Security;
using Intersect.Utilities;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Menu;

public partial class RegisterWindow : IMainMenuWindow
{
    private MainMenu _mainMenu = null!;
    private Widget? _registerWindow;
    private TextBox? _textboxRegisterUsername;
    private TextBox? _textboxEmail;
    private TextBox? _textboxRegisterPassword;
    private TextBox? _textboxPasswordConfirm;
    private Button? _buttonRegister;

    public bool IsHidden => _registerWindow?.Visible == false;

    public void Load(MainMenu mainMenu)
    {
        _mainMenu = mainMenu;
        _registerWindow = Interface.LoadContent(Path.Combine("menu", "RegisterWindow.xmmp"));

        if (Interface.GetChildById<Label>("_labelRegisterTitle", out var labelRegisterTitle))
        {
            labelRegisterTitle.Text = Strings.Registration.Title;
        }

        if (Interface.GetChildById<Label>("_labelRegisterUsername", out var labelRegisterUsername))
        {
            labelRegisterUsername.Text = Strings.Registration.Username;
        }

        if (Interface.GetChildById<TextBox>("_textboxRegisterUsername", out var textboxUsername))
        {
            _textboxRegisterUsername = textboxUsername;
        }

        if (Interface.GetChildById<Label>("_labelEmail", out var labelEmail))
        {
            labelEmail.Text = Strings.Registration.Email;
        }

        if (Interface.GetChildById<TextBox>("_textboxEmail", out var textboxEmail))
        {
            _textboxEmail = textboxEmail;
        }

        if (Interface.GetChildById<Label>("_labelRegisterPassword", out var labelRegisterPassword))
        {
            labelRegisterPassword.Text = Strings.Registration.Password;
        }

        if (Interface.GetChildById<TextBox>("_textboxRegisterPassword", out var textboxPassword))
        {
            _textboxRegisterPassword = textboxPassword;
            _textboxRegisterPassword.PasswordField = true;
        }

        if (Interface.GetChildById<Label>("_labelPasswordConfirm", out var labelPasswordConfirm))
        {
            labelPasswordConfirm.Text = Strings.Registration.ConfirmPassword;
        }

        if (Interface.GetChildById<TextBox>("_textboxPasswordConfirm", out var textboxPasswordConfirm))
        {
            _textboxPasswordConfirm = textboxPasswordConfirm;
            _textboxPasswordConfirm.PasswordField = true;
        }

        if (Interface.GetChildById<Label>("_labelRegister", out var labelRegister))
        {
            labelRegister.Text = Strings.Registration.Register;
        }

        if (Interface.GetChildById<Button>("_buttonRegister", out var buttonRegister))
        {
            _buttonRegister = buttonRegister;
            _buttonRegister.Click += (sender, args) => TryRegister();
        }

        if (Interface.GetChildById<Label>("_labelRegisterBack", out var labelRegisterBack))
        {
            labelRegisterBack.Text = Strings.Registration.Back;
        }

        if (Interface.GetChildById<Button>("_buttonRegisterBack", out var buttonRegisterBack))
        {
            buttonRegisterBack.Click += (sender, args) =>
            {
                Networking.Network.DebounceClose("returning_to_main_menu");
                _mainMenu.SwitchToWindow<LoginWindow>();
            };
        }

        _registerWindow.Visible = false;
    }

    public void Update()
    {
        if (!Networking.Network.IsConnected)
        {
            _mainMenu.SwitchToWindow<LoginWindow>();
        }

        if (_buttonRegister != default)
        {
            _buttonRegister.Enabled = !Globals.WaitingOnServer;
        }
    }

    public void Toggle(bool value)
    {
        if(_registerWindow == default)
        {
            return;
        }

        _registerWindow.Visible = value;
        if (value)
        {
            Interface.SetInputFocus(_textboxRegisterUsername);
        }
    }

    private void TryRegister()
    {
        if (Globals.WaitingOnServer)
        {
            return;
        }

        if (!Networking.Network.IsConnected)
        {
            Interface.ShowError(Strings.Errors.NotConnected);
            return;
        }

        if (!FieldChecking.IsValidUsername(_textboxRegisterUsername?.Text, Strings.Regex.Username))
        {
            Interface.ShowError(Strings.Errors.UsernameInvalid);
            return;
        }

        if (!FieldChecking.IsWellformedEmailAddress(_textboxEmail?.Text, Strings.Regex.Email))
        {
            Interface.ShowError(Strings.Registration.EmailInvalid);
            return;
        }

        if (!FieldChecking.IsValidPassword(_textboxRegisterPassword?.Text, Strings.Regex.Password))
        {
            Interface.ShowError(Strings.Errors.PasswordInvalid);
            return;
        }

        if (_textboxRegisterPassword?.Text != _textboxPasswordConfirm?.Text)
        {
            Interface.ShowError(Strings.Registration.PasswordMismatch);
            return;
        }

        PacketSender.SendCreateAccount(
            _textboxRegisterUsername!.Text,
            PasswordUtils.ComputePasswordHash(_textboxRegisterPassword!.Text.Trim()),
            _textboxEmail!.Text
        );

        Globals.WaitingOnServer = true;
        if (_buttonRegister != default)
        {
            _buttonRegister.Enabled = false;
        }
    }
}