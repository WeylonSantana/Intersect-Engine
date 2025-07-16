using MonoGameGum;

namespace Intersect.Client.Interface.Screens;

public partial class MainMenuInterface
{
    public MainMenuWindow? MainMenuWindow { get; set; } = new();

    public MainMenuInterface()
    {
    }

    public void Update()
    {
    }

    public void Clear()
    {
        MainMenuWindow?.RemoveFromRoot();
        MainMenuWindow = null;
    }
}
