using System.Diagnostics.CodeAnalysis;
using Myra.Graphics2D.UI;
using Myra.MML;

namespace Intersect.Client.Interface.Extensions;

public static class WidgetExtensions
{
    public static void ToggleVisible(this Widget? widget, bool? visible = null)
    {
        if (widget == default)
        {
            return;
        }

        widget.Visible = visible ?? !widget.Visible;
    }

    public static void ToggleEnabled(this Widget? widget, bool? enabled = null)
    {
        if (widget == default)
        {
            return;
        }

        widget.Enabled = enabled ?? !widget.Enabled;
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