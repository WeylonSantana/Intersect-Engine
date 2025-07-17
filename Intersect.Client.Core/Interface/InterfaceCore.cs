using Gum.Wireframe;
using Intersect.Client.Interface.Screens;
using Intersect.Client.MonoGame;
using Intersect.Configuration;
using Microsoft.Xna.Framework;
using MonoGameGum;
using MonoGameGum.Forms.Controls;

namespace Intersect.Client.Interface;

public static class InterfaceCore
{
    public static GumService Gum => GumService.Default;
    public static InteractiveGue Root => Gum.Root;
    public static MainMenuInterface? MainMenuInterface { get; set; }

    internal static void InitializeUI()
    {
        var projectPath = Path.Combine(
            IntersectGame.Instance.Content.RootDirectory,
            ClientConfiguration.ResourcesDirectory,
            "interface",
            "interface.gumx"
        );

        Gum.Initialize(IntersectGame.Instance, projectPath);
        Gum.ContentLoader!.XnaContentManager = IntersectGame.Instance.Content;
        FrameworkElement.KeyboardsForUiControl.Add(Gum.Keyboard);
        InitializeMainMenuInterface();
    }

    public static void InitializeMainMenuInterface()
    {
        //GameInterface?.Clear();
        //GameInterface = default;
        MainMenuInterface ??= new MainMenuInterface();
    }

    public static void InitializeGameInterface()
    {
        MainMenuInterface?.Clear();
        MainMenuInterface = default;
        //GameInterface ??= new GameInterface();
    }

    public static void UpdateUI(GameTime gameTime)
    {
        Gum.Update(gameTime);
        MainMenuInterface?.Update();
    }

    public static void DrawUI()
    {
        Core.Graphics.Renderer.Begin();
        Gum.Draw();
        Core.Graphics.Renderer.End();
    }

    public static void HandleUiSizeChanged(object? sender, EventArgs? e)
    {
        GraphicalUiElement.CanvasWidth = Core.Graphics.Renderer.CurrentView.Width;
        GraphicalUiElement.CanvasHeight = Core.Graphics.Renderer.CurrentView.Height;
        Root.UpdateLayout();
    }

    public static void Clear()
    {
        MainMenuInterface?.Clear();
        MainMenuInterface = null;
        Gum.Root.Children.Clear();
    }
}
