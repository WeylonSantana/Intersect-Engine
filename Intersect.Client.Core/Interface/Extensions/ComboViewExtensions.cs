using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Extensions;

public static class ComboViewExtensions
{
    public static string GetSelectedValue(this ComboView? comboView)
    {
        if (comboView == null || comboView.SelectedItem is not Label label)
        {
            return string.Empty;
        }

        return label.Text;
    }

    public static void SelectByText(this ComboView? comboView, string value)
    {
        if (comboView == null)
        {
            return;
        }

        foreach (var item in comboView.Widgets)
        {
            if (item is not Label label || label.Text != value)
            {
                continue;
            }

            comboView.SelectedItem = label;
            return;
        }
    }

    public static void AddItem(this ComboView? comboView, string value)
    {
        comboView?.Widgets.Add(new Label() { Text = value });
    }
}