using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Extensions;

public static class DesktopExtensions
{
    public static void Add(this Desktop desktop, Window window)
    {
        desktop.Widgets.Add(window.Root);
    }

    public static void Replace(this Desktop desktop, Widget previous, Widget replacement)
    {
        var indexOfPrevious = desktop.Widgets.IndexOf(previous);
        if (indexOfPrevious < 0)
        {
            desktop.Widgets.Add(replacement);
            return;
        }

        desktop.Widgets.RemoveAt(indexOfPrevious);
        desktop.Widgets.Insert(indexOfPrevious, replacement);
    }
}