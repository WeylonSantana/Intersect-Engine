namespace Intersect.Client.Interface.Menu;

public interface IMainMenuWindow
{
    bool IsHidden { get; }

    void Toggle(bool value);
}