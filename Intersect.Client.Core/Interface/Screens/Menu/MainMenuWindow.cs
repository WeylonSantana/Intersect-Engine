using Intersect.Client.Localization;
using Intersect.Client.MonoGame.Network;
using Intersect.Client.Networking;
using MonoGameGum;

namespace Intersect.Client.Interface.Screens;

public partial class MainMenuWindow
{
    public LoginWindow LoginWindow { get; set; } = new();
    public RegisterWindow RegisterWindow { get; set; } = new();
    public CreditsWindow CreditsWindow { get; set; } = new();

    partial void CustomInitialize()
    {
        AddChild(LoginWindow);
        AddChild(RegisterWindow);
        AddChild(CreditsWindow);
        this.AddToRoot();
    }

    public void Initialize()
    {
        CustomInitialize();
    }

    public void Update()
    {
        var text = Strings.ServerStatus.StatusLabel.ToString(
            MonoSocket.Instance.CurrentNetworkStatus.ToLocalizedString()
        );

        if (ServerStatusLabel.Text != text)
        {
            ServerStatusLabel.Text = text;
        }

        LoginWindow.Update();
        RegisterWindow.Update();
    }
}
