using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Extensions;

public static class SliderExtensions
{
    public static void SetRange(this Slider? slider, float min = 0, float max = 0)
    {
        if (slider == default)
        {
            return;
        }

        slider.Minimum = min;
        slider.Maximum = max;
    }

    public static void SetValue(this Slider? slider, float value)
    {
        if (slider == default)
        {
            return;
        }

        slider.Value = value;
    }
}