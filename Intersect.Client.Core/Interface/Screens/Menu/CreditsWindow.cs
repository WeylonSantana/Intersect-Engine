using Intersect.Client.Interface.Components;
using Intersect.Client.Localization;
using Intersect.Configuration;
using Newtonsoft.Json;
using RenderingLibrary.Graphics;

namespace Intersect.Client.Interface.Screens;

public partial struct CreditsLine
{
    public string Text;
    public string Font;
    public int Size;
    public string Alignment;
    public Color TextColor;
    public int YOffset;
}

public partial class CreditsWindow
{
    private List<CreditsLine> _lines = [];

    partial void CustomInitialize()
    {
        IsVisible = false;
        WindowTitle.Text = Strings.CreditsWindow.Title;
        BackButton.Text = Strings.CreditsWindow.Back;
        BackButton.Click += (sender, args) =>
        {
            IsVisible = false;
            MainMenuInterface.MainMenuWindow.LoginWindow.IsVisible = true;
        };

        var creditsFile = Path.Combine(ClientConfiguration.ResourcesDirectory, "credits.json");

        if (File.Exists(creditsFile))
        {
            _lines = JsonConvert.DeserializeObject<List<CreditsLine>>(File.ReadAllText(creditsFile)) ?? [];
        }
        else
        {
            _lines =
            [
                new()
                {
                    Text = "No credits file found.",
                    Font = "Default",
                    Size = 12,
                    Alignment = "Center",
                    TextColor = Color.White
                }
            ];
        }

        File.WriteAllText(creditsFile, JsonConvert.SerializeObject(_lines, Formatting.Indented));

        foreach (var line in _lines ?? [])
        {
            var lineText = line.Text.Trim();
            if (lineText.Length == 0)
            {
                continue;
            }

            var content = new Label();
            ContentContainer.AddChild(content);

            content.TextInstance.Text = lineText;
            content.TextInstance.Font = line.Font;
            content.TextInstance.FontSize = line.Size;

            content.TextInstance.HorizontalAlignment = line.Alignment switch
            {
                "Left" => HorizontalAlignment.Left,
                "Right" => HorizontalAlignment.Right,
                _ => HorizontalAlignment.Center
            };

            content.TextInstance.Color = new(
                line.TextColor?.R ?? 255,
                line.TextColor?.G ?? 255,
                line.TextColor?.B ?? 255,
                line.TextColor?.A ?? 255
            );

            // we need to set the width of the label to fit the content
            var scrolWidth = ContentContainer.VerticalScrollBarInstance.Width;
            content.TextInstance.Width = ContentContainer.Width - scrolWidth;
            content.TextComponent.WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute;

            content.Y += line.YOffset;
        }
    }
}
