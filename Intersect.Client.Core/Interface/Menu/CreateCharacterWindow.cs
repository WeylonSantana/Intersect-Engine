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
    private ComboBox? _classComboBox;
    private CheckButton? _chkMale;
    private CheckButton? _chkFemale;
    private Image? _charPortrait;
    private Button? _prevSpriteButton;
    private Button? _nextSpriteButton;
    private Button? _createButton;
    private Button? _backButton;
    private int _displaySpriteIndex = -1;
    private readonly List<KeyValuePair<int, ClassSprite>> _femaleSprites = new();
    private readonly List<KeyValuePair<int, ClassSprite>> _maleSprites = new();

    public bool Visible => _createCharacterWindow?.Visible ?? false;

    public void Load(MenuInterface mainMenu)
    {
        _mainMenu = mainMenu;
        _createCharacterWindow = Interface.LoadContent(Path.Combine("menu", "CreateCharacterWindow.xmmp"));

        //Menu Header
        if (Interface.GetChildById<Label>("_labelCreateCharacterTitle", out var labelSelectCharacterTitle))
        {
            labelSelectCharacterTitle.Text = Strings.CharacterCreation.Title;
        }

        //Character Name
        _charNameTextbox = Interface.GetChildById<TextBox>("_characterNameField");
        if (_charNameTextbox != default)
        {
            Interface.SetInputFocus(_charNameTextbox);
            _charNameTextbox.TouchDown += _textboxCharactername_Clicked;
        }

        // Class Combobox
        _classComboBox = Interface.GetChildById<ComboBox>("_classComboBox");
        if (_classComboBox != default)
        {
            _classComboBox.SelectedIndexChanged += ClassComboBoxItemSelected;
        }

        // Male Checkbox
        _chkMale = Interface.GetChildById<CheckButton>("_maleCheckbutton");
        if (_chkMale != default)
        {
            _chkMale.TouchDown += maleChk_Checked;
        }

        // Female Checkbox
        _chkFemale = Interface.GetChildById<CheckButton>("_femaleCheckbutton");
        if (_chkFemale != default)
        {
            _chkFemale.TouchDown += femaleChk_Checked;
        }

        // Character Portrait
        _charPortrait = Interface.GetChildById<Image>("_characterPortrait");
        if (_charPortrait != default)
        {
            //_charPortrait = charPortrait;
        }

        // Previous Sprite Button
        _prevSpriteButton = Interface.GetChildById<Button>("_previousSpriteButton");
        if (_prevSpriteButton != default)
        {
            _prevSpriteButton.Click += _prevSpriteButton_Clicked;
        }

        // Next Sprite Button
        _nextSpriteButton = Interface.GetChildById<Button>("_nextSpriteButton");
        if (_nextSpriteButton != default)
        {
            _nextSpriteButton.Click += _nextSpriteButton_Clicked;
        }

        // Create Button
        _createButton = Interface.GetChildById<Button>("_createButton");
        if (_createButton != default)
        {
            _createButton.Click += CreateButton_Clicked;
        }

        // Back Button
        _backButton = Interface.GetChildById<Button>("_backButton");
        if (_backButton != default)
        {
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
        _classComboBox?.Items.Clear();
        var classCount = 0;
        foreach (ClassBase cls in ClassBase.Lookup.Values.Cast<ClassBase>())
        {
            if (cls.Locked)
            {
                continue;
            }

            _classComboBox?.Items.Add(new ListItem(cls.Name));
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
            _charPortrait.ToggleVisible(false);
            return;
        }

        _charPortrait.ToggleVisible(true);
        if (cls.Sprites.Count <= 0)
        {
            return;
        }

        bool isFace;
        var source = _chkMale.IsChecked ? _maleSprites[_displaySpriteIndex] : _femaleSprites[_displaySpriteIndex];
        var faceTex = Globals.ContentManager.GetTexture(TextureType.Face, source.Value.Face).GetTexture();
        var entityTex = Globals.ContentManager.GetTexture(TextureType.Entity, source.Value.Sprite).GetTexture();

        isFace = faceTex != null;
        _charPortrait.Renderable = isFace ? (IImage)faceTex : (IImage)entityTex;

        if (_charPortrait.Renderable == null)


        if (_charPortrait.Renderable == null)
        {
            return;
        }

        var imgWidth = _charPortrait.Width;
        var imgHeight = _charPortrait.Height;
        var textureWidth = isFace ? imgWidth : imgWidth / Options.Instance.Sprites.NormalFrames;
        var textureHeight = isFace ? imgHeight : imgHeight / Options.Instance.Sprites.Directions;

        //_charPortrait.Renderable.SetTextureRect(0, 0, textureWidth, textureHeight);

        var scale = Math.Min(_charPortrait.Width / (double?)imgWidth ?? 0, _charPortrait.Height / (double?)imgHeight ?? 0);
        var sizeX = isFace ? (int)(imgWidth * scale) : textureWidth;
        var sizeY = isFace ? (int)(imgHeight * scale) : textureHeight;
        _charPortrait.Width = sizeX;
        _charPortrait.Height = sizeY;

        var centerX = (_charPortrait.Parent.Width / 2) - (_charPortrait.Width / 2);
        var centerY = (_charPortrait.Parent.Height / 2) - (_charPortrait.Height / 2);
        _charPortrait.Left = centerX ?? 0;
        _charPortrait.Top = centerY ?? 0;
        
    }

    private ClassBase? GetClass()
    {
        if (_classComboBox?.SelectedItem == null)
        {
            return null;
        }

        return ClassBase.Lookup.Values.OfType<ClassBase>().FirstOrDefault(
            descriptor => !descriptor.Locked && string.Equals(
                _classComboBox.SelectedItem.ToString(),
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
        if (_chkMale.IsChecked)
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
        if (_chkMale.IsChecked)
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
        if (_chkMale.IsChecked)
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
        var spriteKey = _chkMale.IsChecked
            ? _maleSprites[_displaySpriteIndex].Key
            : _femaleSprites[_displaySpriteIndex].Key;
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
}