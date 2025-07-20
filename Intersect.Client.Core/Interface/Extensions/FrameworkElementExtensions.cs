using MonoGameGum.Forms.Controls;
using RenderingLibrary.Graphics;

namespace Intersect.Client.Interface.Extensions;

public static partial class FrameworkElementExtensions
{
    public static void BringToFront(this IRenderableIpso element)
    {
        // to bring a element to the front, we need to put at the end of the children list
        InterfaceCore.Root.Children.Remove(element);
        InterfaceCore.Root.Children.Add(element);
    }

    public static void BringToFront(this FrameworkElement element)
    {
        element.Visual.BringToFront();
    }
}
