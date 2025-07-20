using Intersect.Client.Localization;
using MonoGameGum;

namespace Intersect.Client.Interface.Screens;

public partial class AlertWindow
{
    partial void CustomInitialize()
    {
        this.AddToRoot();
        IsVisible = false;
        SubmitButton.Text = Strings.General.Okay;
        SubmitButton.Click += (sender, args) =>
        {
            IsVisible = false;
        };
    }

    public void Show(string title, string message, AlertType type = AlertType.Info)
    {
        IsVisible = true;
        WindowTitle.Text = title;
        ContentLabel.Text = message;
        AlertTypeState = type;
    }

    public void ShowError(string message, string? title = default)
        => Show(title ?? Strings.Errors.Title, message, AlertType.Error);

    public void ShowInfo(string message, string? title = default)
        => Show(title ?? Strings.General.Information, message, AlertType.Info);
}
