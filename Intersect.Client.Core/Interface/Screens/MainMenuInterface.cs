using MonoGameGum;

namespace Intersect.Client.Interface.Screens;

public partial class MainMenuInterface
{
    public static MainMenuWindow MainMenuWindow { get; set; } = new();

    public static void Initialize()
    {
        MainMenuWindow.Initialize();
    }

    public static void Update()
    {
        MainMenuWindow.Update();
    }

    public static void Clear()
    {
        MainMenuWindow.RemoveFromRoot();
    }
}
