using Myra.Graphics2D.UI;

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
}