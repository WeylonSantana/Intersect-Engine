using MonoGameGum;

namespace Intersect.Client.Interface.Screens;

public partial class MainMenuWindow
{
    public LoginWindow LoginWindow { get; set; } = new();

    partial void CustomInitialize()
    {
        AddChild(LoginWindow);
        this.AddToRoot();
    }
}
