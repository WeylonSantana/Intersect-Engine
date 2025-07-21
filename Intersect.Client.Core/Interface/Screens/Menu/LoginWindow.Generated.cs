//Code for Menu/LoginWindow
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
partial class LoginWindow : MonoGameGum.Forms.Controls.FrameworkElement
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        var template = new MonoGameGum.Forms.VisualTemplate((vm, createForms) =>
        {
            var visual = new MonoGameGum.GueDeriving.ContainerRuntime();
            var element = ObjectFinder.Self.GetElementSave("Menu/LoginWindow");
            element.SetGraphicalUiElement(visual, RenderingLibrary.SystemManagers.Default);
            if(createForms) visual.FormsControlAsObject = new LoginWindow(visual);
            visual.Width = 0;
            visual.WidthUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
            visual.Height = 0;
            visual.HeightUnits = Gum.DataTypes.DimensionUnitType.RelativeToParent;
            return visual;
        });
        MonoGameGum.Forms.Controls.FrameworkElement.DefaultFormsTemplates[typeof(LoginWindow)] = template;
        ElementSaveExtensions.RegisterGueInstantiation("Menu/LoginWindow", () => 
        {
            var gue = template.CreateContent(null, true) as InteractiveGue;
            return gue;
        });
    }
    public Window WindowInstance { get; protected set; }
    public Label WindowTitle { get; protected set; }
    public StackPanel OptionsButtonsContainer { get; protected set; }
    public InputContainer UsernameContainer { get; protected set; }
    public InputContainer PasswordContainer { get; protected set; }
    public Label UsernameLabel { get; protected set; }
    public Label PasswordLabel { get; protected set; }
    public InputText UsernameInput { get; protected set; }
    public InputPassword PasswordInput { get; protected set; }
    public CheckBox SaveCredentialsCheckbox { get; protected set; }
    public Panel ButtonsContainer { get; protected set; }
    public Button RegisterButton { get; protected set; }
    public Button LoginButton { get; protected set; }
    public StackPanel InputsContainer { get; protected set; }
    public Button ForgotPasswordButton { get; protected set; }
    public Button CreditsButton { get; protected set; }
    public Icon CreditsIcon { get; protected set; }

    public LoginWindow(InteractiveGue visual) : base(visual) { }
    public LoginWindow()
    {



    }
    protected override void ReactToVisualChanged()
    {
        base.ReactToVisualChanged();
        WindowInstance = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Window>(this.Visual,"WindowInstance");
        WindowTitle = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"WindowTitle");
        OptionsButtonsContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<StackPanel>(this.Visual,"OptionsButtonsContainer");
        UsernameContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<InputContainer>(this.Visual,"UsernameContainer");
        PasswordContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<InputContainer>(this.Visual,"PasswordContainer");
        UsernameLabel = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"UsernameLabel");
        PasswordLabel = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Label>(this.Visual,"PasswordLabel");
        UsernameInput = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<InputText>(this.Visual,"UsernameInput");
        PasswordInput = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<InputPassword>(this.Visual,"PasswordInput");
        SaveCredentialsCheckbox = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<CheckBox>(this.Visual,"SaveCredentialsCheckbox");
        ButtonsContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Panel>(this.Visual,"ButtonsContainer");
        RegisterButton = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Button>(this.Visual,"RegisterButton");
        LoginButton = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Button>(this.Visual,"LoginButton");
        InputsContainer = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<StackPanel>(this.Visual,"InputsContainer");
        ForgotPasswordButton = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Button>(this.Visual,"ForgotPasswordButton");
        CreditsButton = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Button>(this.Visual,"CreditsButton");
        CreditsIcon = MonoGameGum.Forms.GraphicalUiElementFormsExtensions.TryGetFrameworkElementByName<Icon>(this.Visual,"CreditsIcon");
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
