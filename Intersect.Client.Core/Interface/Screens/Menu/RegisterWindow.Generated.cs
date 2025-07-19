//Code for Menu/RegisterWindow
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
partial class RegisterWindow : MonoGameGum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("Menu/RegisterWindow");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new RegisterWindow(visual);
            visual.Width = 0;
            visual.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(RegisterWindow)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("Menu/RegisterWindow", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public Window WindowInstance { get; protected set; }
    public Label WindowTitle { get; protected set; }
    public InputContainer UsernameContainer { get; protected set; }
    public Label UsernameLabel { get; protected set; }
    public InputText UsernameInput { get; protected set; }
    public InputContainer EmailContainer { get; protected set; }
    public InputContainer PasswordContainer { get; protected set; }
    public Label PasswordLabel { get; protected set; }
    public InputPassword PasswordInput { get; protected set; }
    public InputContainer ConfirmPasswordContainer { get; protected set; }
    public Label ConfirmPasswordLabel { get; protected set; }
    public InputPassword ConfirmPasswordInput { get; protected set; }
    public Label EmailLabel { get; protected set; }
    public InputText EmailInput { get; protected set; }
    public Panel ButtonsContainer { get; protected set; }
    public Button BackButton { get; protected set; }
    public Button RegisterButton { get; protected set; }
    public StackPanel InputsContainer { get; protected set; }

    public RegisterWindow(InteractiveGue visual) : base(visual) { }
    public RegisterWindow()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        WindowInstance = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Window>(this.Visual,"WindowInstance");
        WindowTitle = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"WindowTitle");
        UsernameContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<InputContainer>(this.Visual,"UsernameContainer");
        UsernameLabel = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"UsernameLabel");
        UsernameInput = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<InputText>(this.Visual,"UsernameInput");
        EmailContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<InputContainer>(this.Visual,"EmailContainer");
        PasswordContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<InputContainer>(this.Visual,"PasswordContainer");
        PasswordLabel = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"PasswordLabel");
        PasswordInput = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<InputPassword>(this.Visual,"PasswordInput");
        ConfirmPasswordContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<InputContainer>(this.Visual,"ConfirmPasswordContainer");
        ConfirmPasswordLabel = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"ConfirmPasswordLabel");
        ConfirmPasswordInput = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<InputPassword>(this.Visual,"ConfirmPasswordInput");
        EmailLabel = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"EmailLabel");
        EmailInput = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<InputText>(this.Visual,"EmailInput");
        ButtonsContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Panel>(this.Visual,"ButtonsContainer");
        BackButton = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Button>(this.Visual,"BackButton");
        RegisterButton = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Button>(this.Visual,"RegisterButton");
        InputsContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<StackPanel>(this.Visual,"InputsContainer");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
