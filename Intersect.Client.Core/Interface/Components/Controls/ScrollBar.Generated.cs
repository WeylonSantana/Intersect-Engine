//Code for Controls/ScrollBar (Controls/StackPanel)
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

namespace Intersect.Client.Interface.Components;
partial class ScrollBar : StackPanel
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("Controls/ScrollBar");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new ScrollBar(visual);
            return visual;
        });
        MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(ScrollBar)] = template;
        MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(MonoGameGum.Forms.Controls.ScrollBar)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("Controls/ScrollBar", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public enum ScrollBarCategory
    {
    }

    ScrollBarCategory? _scrollBarCategoryState;
    public ScrollBarCategory? ScrollBarCategoryState
    {
        get => _scrollBarCategoryState;
        set
        {
            _scrollBarCategoryState = value;
            if(value != null)
            {
                if(Visual.Categories.ContainsKey("ScrollBarCategory"))
                {
                    var category = Visual.Categories["ScrollBarCategory"];
                    var state = category.States.Find(item => item.Name == value.ToString());
                    this.Visual.ApplyState(state);
                }
                else
                {
                    var category = ((Gum.DataTypes.ElementSave)this.Visual.Tag).Categories.FirstOrDefault(item => item.Name == "ScrollBarCategory");
                    var state = category.States.Find(item => item.Name == value.ToString());
                    this.Visual.ApplyState(state);
                }
            }
        }
    }
    public Button UpButton { get; protected set; }
    public ContainerRuntime TrackInstance { get; protected set; }
    public Button DownButton { get; protected set; }
    public NineSliceRuntime TrackBackground { get; protected set; }
    public Button ThumbInstance { get; protected set; }
    public Icon IconUp { get; protected set; }
    public Icon IconDown { get; protected set; }

    public ScrollBar(InteractiveGue visual) : base(visual) { }
    public ScrollBar()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        //base.AfterFullCreation();
        UpButton = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Button>(this.Visual,"UpButton");
        TrackInstance = this.Visual?.GetGraphicalUiElementByName("TrackInstance") as ContainerRuntime;
        DownButton = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Button>(this.Visual,"DownButton");
        TrackBackground = this.Visual?.GetGraphicalUiElementByName("TrackBackground") as NineSliceRuntime;
        ThumbInstance = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Button>(this.Visual,"ThumbInstance");
        IconUp = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconUp");
        IconDown = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"IconDown");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
