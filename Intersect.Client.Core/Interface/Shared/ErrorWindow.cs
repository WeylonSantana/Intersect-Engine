using Intersect.Client.Localization;

namespace Intersect.Client.Interface.Shared;

public partial class ErrorHandler
{
    private readonly List<InputBox> _windows = [];

    public void Update()
    {
        while (Interface.TryDequeueErrorMessage(out var message))
        {
            _windows.Add(
                new InputBox(
                    title: string.IsNullOrWhiteSpace(message.Key) ? Strings.Errors.Title.ToString() : message.Key,
                    prompt: message.Value,
                    inputType: InputBox.InputType.OkayOnly,
                    onSuccess: (sender, e) =>
                    {
                        // Clear window after iteration is complete
                        _windows.Clear();
                    }
                )
            );
        }
    }
}