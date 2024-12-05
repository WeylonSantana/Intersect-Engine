using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Extensions;

public static class ButtonExtensions
{
    public static string GetText(this Button? checkButton)
    {
        return checkButton?.Content is not Label label ? string.Empty : label.Text;
    }

    public static void SetText(this Button? checkButton, string text)
    {
        if (checkButton?.Content is not Label label)
        {
            return;
        }

        label.Text = text;
    }
}