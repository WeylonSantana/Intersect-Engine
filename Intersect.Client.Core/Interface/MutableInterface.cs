using Intersect.Client.Interface.Debugging;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface;

public abstract partial class MutableInterface
{
    internal Widget Root { get; }

    private static DebugWindow? _debugWindow;

    public static void DetachDebugWindow()
    {
        if (_debugWindow != null)
        {
            _debugWindow.Parent = default;
        }
    }

    internal static void DisposeDebugWindow()
    {
        _debugWindow?.Dispose();
    }

    protected internal MutableInterface(Project project)
    {
        // MYRA-TODO: re-enable debug window
        //_ = new DebugWindow()
    }

    public static bool ToggleDebug()
    {
        _debugWindow?.ToggleHidden();
        return _debugWindow?.IsVisible ?? false;
    }
}
