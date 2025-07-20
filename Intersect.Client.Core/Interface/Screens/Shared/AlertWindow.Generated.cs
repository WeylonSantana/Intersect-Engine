//Code for Shared/AlertWindow
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
partial class AlertWindow : MonoGameGum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("Shared/AlertWindow");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new AlertWindow(visual);
            visual.Width = 0;
            visual.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(AlertWindow)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("Shared/AlertWindow", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public enum AlertType
    {
        Error,
        Success,
        Warning,
        Info,
    }

    AlertType? _alertTypeState;
    public AlertType? AlertTypeState
    {
        get => _alertTypeState;
        set
        {
            _alertTypeState = value;
            if(value != null)
            {
                if(Visual.Categories.ContainsKey("AlertType"))
                {
                    var category = Visual.Categories["AlertType"];
                    var state = category.States.Find(item => item.Name == value.ToString());
                    this.Visual.ApplyState(state);
                }
                else
                {
                    var category = ((Gum.DataTypes.ElementSave)this.Visual.Tag).Categories.FirstOrDefault(item => item.Name == "AlertType");
                    var state = category.States.Find(item => item.Name == value.ToString());
                    this.Visual.ApplyState(state);
                }
            }
        }
    }
    public Window WindowInstance { get; protected set; }
    public Panel WindowIconContainer { get; protected set; }
    public NineSliceRuntime WindowIconBackground { get; protected set; }
    public Icon WindowIcon { get; protected set; }
    public Label WindowTitle { get; protected set; }
    public StackPanel TitleContainer { get; protected set; }
    public StackPanel ContentContainer { get; protected set; }
    public Label ContentLabel { get; protected set; }
    public Button SubmitButton { get; protected set; }

    public AlertWindow(InteractiveGue visual) : base(visual) { }
    public AlertWindow()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        WindowInstance = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Window>(this.Visual,"WindowInstance");
        WindowIconContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Panel>(this.Visual,"WindowIconContainer");
        WindowIconBackground = this.Visual?.GetGraphicalUiElementByName("WindowIconBackground") as NineSliceRuntime;
        WindowIcon = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"WindowIcon");
        WindowTitle = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"WindowTitle");
        TitleContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<StackPanel>(this.Visual,"TitleContainer");
        ContentContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<StackPanel>(this.Visual,"ContentContainer");
        ContentLabel = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"ContentLabel");
        SubmitButton = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Button>(this.Visual,"SubmitButton");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
