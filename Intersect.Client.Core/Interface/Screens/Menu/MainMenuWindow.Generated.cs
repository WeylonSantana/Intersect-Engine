//Code for Menu/MainMenuWindow
using GumRuntime;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using Intersect.Client.Interface.Components;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

namespace Intersect.Client.Interface.Screens;
partial class MainMenuWindow : MonoGameGum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("Menu/MainMenuWindow");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new MainMenuWindow(visual);
            visual.Width = 0;
            visual.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(MainMenuWindow)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("Menu/MainMenuWindow", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public Panel Container { get; protected set; }
    public SpriteRuntime Logo { get; protected set; }
    public Panel ServerStatusContainer { get; protected set; }
    public ColoredRectangleRuntime ServerStatusLabelBackground { get; protected set; }
    public Label ServerStatusLabel { get; protected set; }

    public MainMenuWindow(InteractiveGue visual) : base(visual) { }
    public MainMenuWindow()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        Container = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Panel>(this.Visual,"Container");
        Logo = this.Visual?.GetGraphicalUiElementByName("Logo") as SpriteRuntime;
        ServerStatusContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Panel>(this.Visual,"ServerStatusContainer");
        ServerStatusLabelBackground = this.Visual?.GetGraphicalUiElementByName("ServerStatusLabelBackground") as ColoredRectangleRuntime;
        ServerStatusLabel = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"ServerStatusLabel");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
