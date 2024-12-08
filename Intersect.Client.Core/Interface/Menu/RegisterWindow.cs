using Intersect.Client.General;
using Intersect.Client.Interface.Extensions;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Security;
using Intersect.Utilities;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Menu;

public partial class RegisterWindow : IMainMenuWindow
{
    private MenuInterface _mainMenu = null!;
    private Widget? _registerWindow;
    private TextBox? _textboxRegisterUsername;
    private TextBox? _textboxEmail;
    private TextBox? _textboxRegisterPassword;
    private TextBox? _textboxPasswordConfirm;
    private Button? _buttonRegister;

    public bool Visible => _registerWindow?.Visible ?? false;

    public void Load(MenuInterface mainMenu)
    {
        _mainMenu = mainMenu;
        _registerWindow = Interface.LoadContent(Path.Combine("menu", "RegisterWindow.xmmp"));
        _registerWindow.FindChildById<Label>(TITLE_LABEL)?.SetText(Strings.Registration.Title);
        _registerWindow.FindChildById<Label>(USERNAME_LABEL)?.SetText(Strings.Registration.Username);
        _registerWindow.FindChildById<Label>(EMAIL_LABEL)?.SetText(Strings.Registration.Email);
        _registerWindow.FindChildById<Label>(PASSWORD_LABEL)?.SetText(Strings.Registration.Password);
        _registerWindow.FindChildById<Label>(CONFIRM_PASSWORD_LABEL)?.SetText(Strings.Registration.ConfirmPassword);

        if (_registerWindow.FindChildById<TextBox>(USERNAME_TEXTBOX, out var textboxUsername))
        {
            _textboxRegisterUsername = textboxUsername;
        }

        if (_registerWindow.FindChildById<TextBox>(EMAIL_TEXTBOX, out var textboxEmail))
        {
            _textboxEmail = textboxEmail;
        }

        if (_registerWindow.FindChildById<TextBox>(PASSWORD_TEXTBOX, out var textboxPassword))
        {
            _textboxRegisterPassword = textboxPassword;
            _textboxRegisterPassword.PasswordField = true;
        }

        if (_registerWindow.FindChildById<TextBox>(CONFIRM_PASSWORD_TEXTBOX, out var textboxPasswordConfirm))
        {
            _textboxPasswordConfirm = textboxPasswordConfirm;
            _textboxPasswordConfirm.PasswordField = true;
        }

        if (_registerWindow.FindChildById<Button>(REGISTER_BUTTON, out var buttonRegister))
        {
            _buttonRegister = buttonRegister;
            _buttonRegister.Click += (sender, args) => TryRegister();
            _buttonRegister.SetText(Strings.Registration.Register);
        }

        if (_registerWindow.FindChildById<Button>(BACK_BUTTON, out var buttonRegisterBack))
        {
            buttonRegisterBack.SetText(Strings.Registration.Back);
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

    public void Show()
    {
        if (_registerWindow == default)
        {
            return;
        }

        _registerWindow.Visible = true;
        Interface.SetInputFocus(_textboxRegisterUsername);
    }

    public void Hide()
    {
        if (_registerWindow == default)
        {
            return;
        }

        _registerWindow.Visible = false;
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

    #region Constants

    private const string TITLE_LABEL = nameof(TITLE_LABEL);
    private const string USERNAME_LABEL = nameof(USERNAME_LABEL);
    private const string USERNAME_TEXTBOX = nameof(USERNAME_TEXTBOX);
    private const string EMAIL_LABEL = nameof(EMAIL_LABEL);
    private const string EMAIL_TEXTBOX = nameof(EMAIL_TEXTBOX);
    private const string PASSWORD_LABEL = nameof(PASSWORD_LABEL);
    private const string PASSWORD_TEXTBOX = nameof(PASSWORD_TEXTBOX);
    private const string CONFIRM_PASSWORD_LABEL = nameof(CONFIRM_PASSWORD_LABEL);
    private const string CONFIRM_PASSWORD_TEXTBOX = nameof(CONFIRM_PASSWORD_TEXTBOX);
    private const string REGISTER_BUTTON = nameof(REGISTER_BUTTON);
    private const string BACK_BUTTON = nameof(BACK_BUTTON);

    #endregion
}