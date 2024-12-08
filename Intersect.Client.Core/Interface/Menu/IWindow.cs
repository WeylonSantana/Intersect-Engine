namespace Intersect.Client.Interface.Menu;

public interface IWindow
{
    bool Visible { get; }

    void Show();

    void Hide();
}