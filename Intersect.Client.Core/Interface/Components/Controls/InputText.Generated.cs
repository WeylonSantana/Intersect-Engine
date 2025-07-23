//Code for Controls/InputText (Container)
using GumRuntime;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

namespace Intersect.Client.Interface.Components;
partial class InputText : MonoGameGum.Forms.Controls.TextBox
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("Controls/InputText");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new InputText(visual);
            return visual;
        });
        MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(InputText)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("Controls/InputText", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public enum TextBoxCategory
    {
        Enabled,
        Disabled,
        Highlighted,
        Focused,
    }
    public enum LineModeCategory
    {
        Single,
        Multi,
    }

    TextBoxCategory? _textBoxCategoryState;
    public TextBoxCategory? TextBoxCategoryState
    {
        get => _textBoxCategoryState;
        set
        {
            _textBoxCategoryState = value;
            if(value != null)
            {
                if(Visual.Categories.ContainsKey("TextBoxCategory"))
                {
                    var category = Visual.Categories["TextBoxCategory"];
                    var state = category.States.Find(item => item.Name == value.ToString());
                    this.Visual.ApplyState(state);
                }
                else
                {
                    var category = ((Gum.DataTypes.ElementSave)this.Visual.Tag).Categories.FirstOrDefault(item => item.Name == "TextBoxCategory");
                    var state = category.States.Find(item => item.Name == value.ToString());
                    this.Visual.ApplyState(state);
                }
            }
        }
    }

    LineModeCategory? _lineModeCategoryState;
    public LineModeCategory? LineModeCategoryState
    {
        get => _lineModeCategoryState;
        set
        {
            _lineModeCategoryState = value;
            if(value != null)
            {
                if(Visual.Categories.ContainsKey("LineModeCategory"))
                {
                    var category = Visual.Categories["LineModeCategory"];
                    var state = category.States.Find(item => item.Name == value.ToString());
                    this.Visual.ApplyState(state);
                }
                else
                {
                    var category = ((Gum.DataTypes.ElementSave)this.Visual.Tag).Categories.FirstOrDefault(item => item.Name == "LineModeCategory");
                    var state = category.States.Find(item => item.Name == value.ToString());
                    this.Visual.ApplyState(state);
                }
            }
        }
    }
    public NineSliceRuntime Background { get; protected set; }
    public NineSliceRuntime SelectionInstance { get; protected set; }
    public TextRuntime TextInstance { get; protected set; }
    public TextRuntime PlaceholderTextInstance { get; protected set; }
    public NineSliceRuntime FocusedIndicator { get; protected set; }
    public SpriteRuntime CaretInstance { get; protected set; }

    public string PlaceholderText
    {
        get => PlaceholderTextInstance.Text;
        set => PlaceholderTextInstance.Text = value;
    }

    public int? MaxLettersToShow
    {
        get => TextInstance.MaxLettersToShow;
        set => TextInstance.MaxLettersToShow = value;
    }

    public int? MaxNumberOfLines
    {
        get => TextInstance.MaxNumberOfLines;
        set => TextInstance.MaxNumberOfLines = value;
    }

    public string Text
    {
        get => TextInstance.Text;
        set => TextInstance.Text = value;
    }

    public InputText(InteractiveGue visual) : base(visual) { }
    public InputText()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        Background = this.Visual?.GetGraphicalUiElementByName("Background") as NineSliceRuntime;
        SelectionInstance = this.Visual?.GetGraphicalUiElementByName("SelectionInstance") as NineSliceRuntime;
        TextInstance = this.Visual?.GetGraphicalUiElementByName("TextInstance") as TextRuntime;
        PlaceholderTextInstance = this.Visual?.GetGraphicalUiElementByName("PlaceholderTextInstance") as TextRuntime;
        FocusedIndicator = this.Visual?.GetGraphicalUiElementByName("FocusedIndicator") as NineSliceRuntime;
        CaretInstance = this.Visual?.GetGraphicalUiElementByName("CaretInstance") as SpriteRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
