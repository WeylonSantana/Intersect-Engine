using System.Diagnostics.CodeAnalysis;
using Myra.Graphics2D.UI;
using Myra.MML;

namespace Intersect.Client.Interface.Extensions;

public static class WidgetExtensions
{
    public static void SetVisible(this Widget? widget, bool visible)
    {
        if (widget == default)
        {
            return;
        }

        widget.Visible = visible;
    }

    public static void SetEnabled(this Widget? widget, bool enabled)
    {
        if (widget == default)
        {
            return;
        }

        widget.Enabled = enabled;
    }

    public static bool FindChildById<T>(this Widget parent, string id, [NotNullWhen(true)] out T? widget) where T : BaseObject
    {
        var foundWidget = parent.FindChildById(id);
        if (foundWidget is T typedWidget)
        {
            widget = typedWidget;
            return true;
        }

        widget = default;
        return false;
    }
}