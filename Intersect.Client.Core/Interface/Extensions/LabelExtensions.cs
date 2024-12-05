using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Extensions;

public static class LabelExtensions
{
    public static void GetText(this Label? label, out string text)
    {
        text = label?.Text ?? string.Empty;
    }

    public static void SetText(this Label? label, string text)
    {
        if (label == default)
        {
            return;
        }

        label.Text = text;
    }
}