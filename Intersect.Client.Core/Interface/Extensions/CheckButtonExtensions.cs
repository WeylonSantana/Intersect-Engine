using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Extensions;

public static class CheckButtonExtensions
{
    public static string GetText(this CheckButton? checkButton)
    {
        return checkButton?.Content is not Label label ? string.Empty : label.Text;
    }

    public static void SetText(this CheckButton? checkButton, string text)
    {
        if (checkButton?.Content is not Label label)
        {
            return;
        }

        label.Text = text;
    }

    public static void SetValue(this CheckButton? checkButton, bool value)
    {
        if (checkButton == default)
        {
            return;
        }

        checkButton.IsChecked = value;
    }
}