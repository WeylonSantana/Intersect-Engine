using Intersect.Client.Localization;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Shared;

public partial class InputBox
{
    public enum InputType
    {
        OkayOnly,
        YesNo,
        NumericInput,
        TextInput,
        NumericSliderInput,
    }

    public string TextValue { get; set; } = string.Empty;

    public new object? UserData { get; set; }

    public double Value { get; set; }

    // Events
    private event EventHandler? OkayEventHandler;
    private event EventHandler? CancelEventHandler;

    public InputBox(
        string title,
        string prompt,
        InputType inputType,
        EventHandler? onSuccess,
        EventHandler? onCancel = default,
        object? userData = default,
        int quantity = 0,
        int maxQuantity = int.MaxValue
    )
    {
        OkayEventHandler = onSuccess;
        CancelEventHandler = onCancel;
        UserData = userData;

        ShowDialog(
            title,
            prompt,
            inputType,
            quantity,
            maxQuantity
        );
    }

    private void ShowDialog(string title, string prompt, InputType inputType, int quantity, int maxQuantity)
    {
        Dialog dialog;

        switch (inputType)
        {
            case InputType.YesNo:
                dialog = Dialog.CreateMessageBox(title, prompt);
                (dialog.ButtonOk.Content as Label).Text = Strings.InputBox.Yes;
                (dialog.ButtonCancel.Content as Label).Text = Strings.InputBox.No;
                dialog.ButtonCancel.Visible = true;
                dialog.ButtonOk.Click += (sender, e) => SubmitInput();
                dialog.ButtonCancel.Click += (sender, e) => CancelInput();
                break;

            case InputType.OkayOnly:
                dialog = Dialog.CreateMessageBox(title, prompt);
                dialog.ButtonCancel.Visible = false;
                dialog.ButtonOk.Click += (sender, e) => SubmitInput();
                break;

            case InputType.NumericInput:
                dialog = new Dialog
                {
                    Title = title,
                    Content = new VerticalStackPanel
                    {
                        Widgets =
                        {
                            new Label
                            {
                                Text = prompt,
                            },
                            new TextBox
                            {
                                Text = quantity.ToString(),
                            },
                        },
                    },
                };
                dialog.ButtonOk.Click += (sender, e) => SubmitInput();
                dialog.ButtonCancel.Click += (sender, e) => CancelInput();
                break;

            case InputType.TextInput:
                dialog = new Dialog
                {
                    Title = title,
                    Content = new VerticalStackPanel
                    {
                        Widgets =
                        {
                            new Label
                            {
                                Text = prompt,
                            },
                            new TextBox(),
                        },
                    },
                };
                dialog.ButtonOk.Click += (sender, e) => SubmitInput();
                dialog.ButtonCancel.Click += (sender, e) => CancelInput();
                break;

            case InputType.NumericSliderInput:
                dialog = new Dialog
                {
                    Title = title,
                    Content = new VerticalStackPanel
                    {
                        Widgets =
                        {
                            new Label
                            {
                                Text = prompt,
                            },
                            new HorizontalSlider
                            {
                                Minimum = 1, Maximum = maxQuantity, Value = quantity,
                            },
                        },
                    },
                };
                dialog.ButtonOk.Click += (sender, e) => SubmitInput();
                dialog.ButtonCancel.Click += (sender, e) => CancelInput();
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        dialog.ShowModal(Interface.Desktop);
    }

    private void CancelInput()
    {
        CancelEventHandler?.Invoke(this, EventArgs.Empty);
    }

    private void SubmitInput()
    {
        OkayEventHandler?.Invoke(this, EventArgs.Empty);
    }
}