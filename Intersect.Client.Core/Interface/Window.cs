using Intersect.Client.Interface.Extensions;
using Intersect.Client.Interface.Menu;
using Myra.Graphics2D.UI;
using Serilog;

namespace Intersect.Client.Interface;

public abstract class Window : IWindow
{
    public const string UserDataKeyResourceName = nameof(UserDataKeyResourceName);

    private readonly string? _resourceName;
    private Project? _project;
    private Widget _root;

    protected Window(string resourceName, Desktop? desktop = default)
    {
        _resourceName = resourceName;

        _project = Interface.LoadProjectFrom(_resourceName);
        _root = _project.Root;

        desktop?.Widgets.Add(_root);
    }

    protected Window(Widget root)
    {
        _root = root;
        _resourceName = root.UserData.GetValueOrDefault(UserDataKeyResourceName);
    }

    protected internal Widget Root => _root;

    public bool Reload(Desktop? desktop = default)
    {
        if (string.IsNullOrWhiteSpace(_resourceName))
        {
            return false;
        }

        try
        {
            _project = Interface.LoadProjectFrom(_resourceName);

            var previousRoot = _root;
            _root = _project.Root;

            desktop ??= previousRoot.Desktop;
            desktop?.Replace(previousRoot, _root);

            OnReload();

            return true;
        }
        catch (Exception exception)
        {
            Log.Error(exception, "Exception occurred while reloading window");
            return false;
        }
    }

    protected virtual void OnReload() { }

    public bool IsReloadable => !string.IsNullOrWhiteSpace(_resourceName);

    public bool Visible => _root.Visible;

    public virtual void Show()
    {
        _root.Visible = true;
    }

    public virtual void Hide()
    {
        _root.Visible = false;
    }
}