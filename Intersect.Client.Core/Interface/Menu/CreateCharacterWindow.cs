using Intersect.Client.Framework.Content;
using Intersect.Client.Framework.Input;
using Intersect.Client.General;
using Intersect.Client.Interface.Extensions;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.GameObjects;
using Intersect.Logging;
using Intersect.Utilities;
using Myra.Graphics2D.UI;
using Myra.Graphics2D;

namespace Intersect.Client.Interface.Menu;

public partial class CreateCharacterWindow : IMainMenuWindow
{
    // Parent
    private MenuInterface _mainMenu = null!;
    private Widget? _createCharacterWindow;
    private TextBox? _charNameTextbox;
    private ComboView? _classComboView;
    private CheckButton? _chkMale;
    private CheckButton? _chkFemale;
    private Image? _charImage;
    private Button? _prevSpriteButton;
    private Button? _nextSpriteButton;
    private Button? _createButton;
    private Button? _backButton;
    private int _displaySpriteIndex = -1;
    private readonly List<KeyValuePair<int, ClassSprite>> _femaleSprites = [];
    private readonly List<KeyValuePair<int, ClassSprite>> _maleSprites = [];

    public bool Visible => _createCharacterWindow?.Visible ?? false;

    public void Load(MenuInterface mainMenu)
    {
        _mainMenu = mainMenu;
        _createCharacterWindow = Interface.LoadContent(Path.Combine("menu", "CreateCharacterWindow.xmmp"));
        Interface.GetChildById<Label>(CREATE_CHARACTER_TITLE_LABEL)?.SetText(Strings.CharacterCreation.Title);

        //Character Name
        if (Interface.GetChildById<TextBox>(CHARACTER_NAME_TEXTBOX, out var charNameTextbox))
        {
            _charNameTextbox = charNameTextbox;
            _charNameTextbox.TouchDown += _textboxCharactername_Clicked;
        }

        // Class Combobox
        if (Interface.GetChildById<ComboView>(CLASS_COMBO_VIEW, out var classComboView))
        {
            _classComboView = classComboView;
            _classComboView.SelectedIndexChanged += ClassComboBoxItemSelected;
        }

        // Male Checkbox
        if (Interface.GetChildById<CheckButton>(MALE_CHECK, out var maleChk))
        {
            _chkMale = maleChk;
            _chkMale.TouchDown += maleChk_Checked;
        }

        // Female Checkbox
        if (Interface.GetChildById<CheckButton>(FEMALE_CHECK, out var femaleChk))
        {
            _chkFemale = femaleChk;
            _chkFemale.TouchDown += femaleChk_Checked;
        }

        // Character Portrait
        if (Interface.GetChildById<Image>(CHARACTER_IMAGE, out var charPortrait))
        {
            _charImage = charPortrait;
        }

        // Previous Sprite Button
        if (Interface.GetChildById<Button>(PREVIOUS_SPRITE_BUTTON, out var prevSpriteButton))
        {
            _prevSpriteButton = prevSpriteButton;
            _prevSpriteButton.Click += _prevSpriteButton_Clicked;
        }

        // Next Sprite Button
        if (Interface.GetChildById<Button>(NEXT_SPRITE_BUTTON, out var nextSpriteButton))
        {
            _nextSpriteButton = nextSpriteButton;
            _nextSpriteButton.Click += _nextSpriteButton_Clicked;
        }

        // Create Button
        if (Interface.GetChildById<Button>(CREATE_BUTTON, out var createButton))
        {
            _createButton = createButton;
            _createButton.Click += CreateButton_Clicked;
        }

        // Back Button
        if (Interface.GetChildById<Button>(BACK_BUTTON, out var backButton))
        {
            _backButton = backButton;
            _backButton.Click += BackButton_Clicked;
        }
    }

    public void Show()
    {
        if (_createCharacterWindow == default)
        {
            return;
        }

        _createCharacterWindow.ToggleVisible(true);
        _classComboView?.Widgets.Clear();
        var classCount = 0;
        foreach (ClassBase cls in ClassBase.Lookup.Values.Cast<ClassBase>())
        {
            if (cls.Locked)
            {
                continue;
            }

            _classComboView?.Widgets.Add(new Label() { Text = cls.Name });
            classCount++;
        }

        Log.Debug($"Added {classCount} classes to {nameof(CreateCharacterWindow)}");
        LoadClass();
        UpdateDisplay();
    }

    public void Hide()
    {
        _createCharacterWindow?.ToggleVisible(false);
    }

    public void Update()
    {
        if (!Networking.Network.IsConnected)
        {
            _mainMenu.SwitchToWindow<LoginWindow>();
            return;
        }

        if (!Globals.WaitingOnServer)
        {
            _createButton?.ToggleEnabled(true);
        }
    }

    private void UpdateDisplay()
    {
        var cls = GetClass();
        if (cls == default || _displaySpriteIndex == -1)
        {
            _charImage.ToggleVisible(false);
            return;
        }

        _charImage.ToggleVisible(true);
        if (cls.Sprites.Count <= 0)
        {
            return;
        }

        bool isFace;
        var source = _chkMale?.IsChecked == true ? _maleSprites[_displaySpriteIndex] : _femaleSprites[_displaySpriteIndex];
        var faceTex = Globals.ContentManager.GetTexture(TextureType.Face, source.Value.Face).GetTexture();
        var entityTex = Globals.ContentManager.GetTexture(TextureType.Entity, source.Value.Sprite).GetTexture();

        isFace = faceTex != null;

        if (_charImage?.Renderable == null)
        {
            return;
        }

        _charImage.Renderable = isFace ? (IImage)faceTex : (IImage)entityTex;
        var imgWidth = _charImage.Width;
        var imgHeight = _charImage.Height;
        var textureWidth = isFace ? imgWidth : imgWidth / Options.Instance.Sprites.NormalFrames;
        var textureHeight = isFace ? imgHeight : imgHeight / Options.Instance.Sprites.Directions;

        var scale = Math.Min(_charImage.Width / (double?)imgWidth ?? 0, _charImage.Height / (double?)imgHeight ?? 0);
        var sizeX = isFace ? (int)(imgWidth * scale) : textureWidth;
        var sizeY = isFace ? (int)(imgHeight * scale) : textureHeight;
        _charImage.Width = sizeX;
        _charImage.Height = sizeY;

        var centerX = (_charImage.Parent.Width / 2) - (_charImage.Width / 2);
        var centerY = (_charImage.Parent.Height / 2) - (_charImage.Height / 2);
        _charImage.Left = centerX ?? 0;
        _charImage.Top = centerY ?? 0;
    }

    private ClassBase? GetClass()
    {
        if (_classComboView?.SelectedItem == null)
        {
            return null;
        }

        return ClassBase.Lookup.Values.OfType<ClassBase>().FirstOrDefault(
            descriptor => !descriptor.Locked && string.Equals(
                _classComboView.SelectedItem.ToString(),
                descriptor.Name,
                StringComparison.Ordinal
            )
        );
    }

    private void LoadClass()
    {
        var cls = GetClass();
        _maleSprites.Clear();
        _femaleSprites.Clear();
        _displaySpriteIndex = -1;
        if (cls != null)
        {
            for (var i = 0; i < cls.Sprites.Count; i++)
            {
                if (cls.Sprites[i].Gender == 0)
                {
                    _maleSprites.Add(new KeyValuePair<int, ClassSprite>(i, cls.Sprites[i]));
                }
                else
                {
                    _femaleSprites.Add(new KeyValuePair<int, ClassSprite>(i, cls.Sprites[i]));
                }
            }
        }

        ResetSprite();
    }

    private void ResetSprite()
    {
        _nextSpriteButton.ToggleVisible(true);
        _prevSpriteButton.ToggleVisible(true);
        if (_chkMale?.IsChecked == true)
        {
            if (_maleSprites.Count > 0)
            {
                _displaySpriteIndex = 0;
                if (_maleSprites.Count > 1)
                {
                    _nextSpriteButton.ToggleVisible(true);
                    _prevSpriteButton.ToggleVisible(true);
                }
            }
            else
            {
                _displaySpriteIndex = -1;
            }
        }
        else
        {
            if (_femaleSprites.Count > 0)
            {
                _displaySpriteIndex = 0;
                if (_femaleSprites.Count > 1)
                {
                    _nextSpriteButton.ToggleVisible(true);
                    _prevSpriteButton.ToggleVisible(true);
                }
            }
            else
            {
                _displaySpriteIndex = -1;
            }
        }
    }

    private void _prevSpriteButton_Clicked(object? sender, EventArgs? arguments)
    {
        _displaySpriteIndex--;
        if (_chkMale?.IsChecked == true)
        {
            if (_maleSprites.Count > 0)
            {
                if (_displaySpriteIndex == -1)
                {
                    _displaySpriteIndex = _maleSprites.Count - 1;
                }
            }
            else
            {
                _displaySpriteIndex = -1;
            }
        }
        else
        {
            if (_femaleSprites.Count > 0)
            {
                if (_displaySpriteIndex == -1)
                {
                    _displaySpriteIndex = _femaleSprites.Count - 1;
                }
            }
            else
            {
                _displaySpriteIndex = -1;
            }
        }

        UpdateDisplay();
    }

    private void _nextSpriteButton_Clicked(object? sender, EventArgs? arguments)
    {
        _displaySpriteIndex++;
        if (_chkMale?.IsChecked == true)
        {
            if (_maleSprites.Count > 0)
            {
                if (_displaySpriteIndex >= _maleSprites.Count)
                {
                    _displaySpriteIndex = 0;
                }
            }
            else
            {
                _displaySpriteIndex = -1;
            }
        }
        else
        {
            if (_femaleSprites.Count > 0)
            {
                if (_displaySpriteIndex >= _femaleSprites.Count)
                {
                    _displaySpriteIndex = 0;
                }
            }
            else
            {
                _displaySpriteIndex = -1;
            }
        }

        UpdateDisplay();
    }

    void TryCreateCharacter()
    {
        var cls = GetClass();
        if (Globals.WaitingOnServer || _displaySpriteIndex == -1 || cls == default)
        {
            return;
        }

        if (!FieldChecking.IsValidUsername(_charNameTextbox?.Text, Strings.Regex.Username))
        {
            Interface.ShowError(Strings.CharacterCreation.InvalidName);
            return;
        }

        var charName = _charNameTextbox?.Text;
        var spriteKey = _chkMale?.IsChecked == true
            ? _maleSprites[_displaySpriteIndex].Key
            : _femaleSprites[_displaySpriteIndex].Key;

        if (string.IsNullOrEmpty(charName))
        {
            return;
        }

        PacketSender.SendCreateCharacter(charName, cls.Id, spriteKey);
        Globals.WaitingOnServer = true;
        _createButton.ToggleEnabled(false);
        //ChatboxMsg.ClearMessages();
    }

    private void _textboxCharactername_Clicked(object? sender, EventArgs? e)
    {
        if (_charNameTextbox == default)
        {
            return;
        }

        Globals.InputManager.OpenKeyboard(
            KeyboardType.Normal,
            text => _charNameTextbox.Text = text ?? string.Empty,
            Strings.LoginWindow.Username,
            _charNameTextbox.Text,
            inputBounds: new Framework.GenericClasses.Rectangle(
                _charNameTextbox.Bounds.X,
                _charNameTextbox.Bounds.Y,
                _charNameTextbox.Bounds.Width,
                _charNameTextbox.Bounds.Height
            )
        );
    }

    void ClassComboBoxItemSelected(object? sender, EventArgs? arguments)
    {
        LoadClass();
        UpdateDisplay();
    }

    void maleChk_Checked(object? sender, EventArgs? arguments)
    {
        _chkMale.SetValue(true);
        _chkFemale.SetValue(false);
        ResetSprite();
        UpdateDisplay();
    }

    void femaleChk_Checked(object? sender, EventArgs? arguments)
    {
        _chkFemale.SetValue(true);
        _chkMale.SetValue(false);
        ResetSprite();
        UpdateDisplay();
    }

    void CreateButton_Clicked(object? sender, EventArgs? arguments)
    {
        if (Globals.WaitingOnServer)
        {
            return;
        }

        TryCreateCharacter();
    }

    private void BackButton_Clicked(object? sender, EventArgs? arguments)
    {
        if (Options.Player.MaxCharacters <= 1)
        {
            _mainMenu.SwitchToWindow<LoginWindow>();
        }
        else
        {
            _mainMenu.SwitchToWindow<SelectCharacterWindow>();
        }
    }

    #region Constants

    private const string CREATE_CHARACTER_TITLE_LABEL = nameof(CREATE_CHARACTER_TITLE_LABEL);
    private const string CHARACTER_NAME_TEXTBOX = nameof(CHARACTER_NAME_TEXTBOX);
    private const string CLASS_COMBO_VIEW = nameof(CLASS_COMBO_VIEW);
    private const string MALE_CHECK = nameof(MALE_CHECK);
    private const string FEMALE_CHECK = nameof(FEMALE_CHECK);
    private const string CHARACTER_IMAGE = nameof(CHARACTER_IMAGE);
    private const string PREVIOUS_SPRITE_BUTTON = nameof(PREVIOUS_SPRITE_BUTTON);
    private const string NEXT_SPRITE_BUTTON = nameof(NEXT_SPRITE_BUTTON);
    private const string CREATE_BUTTON = nameof(CREATE_BUTTON);
    private const string BACK_BUTTON = nameof(BACK_BUTTON);

    #endregion
}