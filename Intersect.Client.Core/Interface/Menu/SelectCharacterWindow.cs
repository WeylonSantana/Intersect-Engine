using Intersect.Client.Core;
using Intersect.Client.Framework.Content;
using Intersect.Client.General;
using Intersect.Client.Interface.Editor;
using Intersect.Client.Interface.Extensions;
using Intersect.Client.Interface.Shared;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Network.Packets.Server;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Menu;

public partial class SelectCharacterWindow : IMainMenuWindow
{
    private MenuInterface _mainMenu = null!;
    private Widget? _selectCharacterWindow;
    private Label? _labelCharname;
    private Label? _labelInfo;
    private Button? _buttonNextChar;
    private Button? _buttonPrevChar;
    private Button? _editorMode;
    private Button? _buttonPlay;
    private Button? _buttonDelete;
    private Button? _buttonNew;
    private Button? _buttonLogout;
    private Panel? _charContainer;
    private Image[]? _renderLayers;
    public Character[]? Characters;
    public int SelectedChar = 0;

    public bool Visible => _selectCharacterWindow?.Visible ?? false;

    public void Load(MenuInterface mainMenu)
    {
        _mainMenu = mainMenu;
        _selectCharacterWindow = Interface.LoadContent(Path.Combine("menu", "SelectCharacterWindow.xmmp"));

        //Menu Header
        if (Interface.GetChildById<Label>(SELECT_CHARACTER_TITLE_LABEL, out var labelSelectCharacterTitle))
        {
            labelSelectCharacterTitle.Text = Strings.CharacterSelection.Title;
        }

        //Character Name
        _labelCharname = Interface.GetChildById<Label>(CHARNAME_LABEL);
        if (_labelCharname != default)
        {
            _labelCharname.Text = Strings.CharacterSelection.Empty;
        }

        //Character Info
        _labelInfo = Interface.GetChildById<Label>(CHARINFO_LABEL);
        if (_labelInfo != default)
        {
            _labelInfo.Text = Strings.CharacterSelection.New;
        }

        //Character Container
        _charContainer = Interface.GetChildById<Panel>(CHAR_CONTAINER);

        //Next char Button
        _buttonNextChar = Interface.GetChildById<Button>(NEXT_CHAR_BUTTON);
        if (_buttonNextChar != default)
        {
            _buttonNextChar.Click += _buttonNextChar_Clicked;
        }

        //Prev Char Button
        _buttonPrevChar = Interface.GetChildById<Button>(PREV_CHAR_BUTTON);
        if (_buttonPrevChar != default)
        {
            _buttonPrevChar.Click += _buttonPrevChar_Clicked;
        }

        //EditorMode Button
        _editorMode = Interface.GetChildById<Button>(EDITOR_MODE_BUTTON);
        if (_editorMode != default)
        {
            _editorMode.SetText("Editor Mode");
            _editorMode.Click += (sender, args) =>
            {
                _mainMenu.Reset();
                _ = new EditorInterface();
            };
            //_editorMode.Visible = Globals.Me.AccessLevel = 2;
        }

        //Play Button
        _buttonPlay = Interface.GetChildById<Button>(PLAY_BUTTON);
        if (_buttonPlay != default)
        {
            _buttonPlay.SetText(Strings.CharacterSelection.Play);
            _buttonPlay.Visible = false;
            _buttonPlay.Click += ButtonPlay_Clicked;
        }

        //Delete Button
        _buttonDelete = Interface.GetChildById<Button>(DELETE_BUTTON);
        if (_buttonDelete != default)
        {
            _buttonDelete.SetText(Strings.CharacterSelection.Delete);
            _buttonDelete.Visible = false;
            _buttonDelete.Click += _buttonDelete_Clicked;
        }

        //Create new char Button
        _buttonNew = Interface.GetChildById<Button>(NEWCHAR_BUTTON);
        if (_buttonNew != default)
        {
            _buttonNew.SetText(Strings.CharacterSelection.New);
            _buttonNew.Click += _buttonNew_Clicked;
        }

        //Logout Button
        _buttonLogout = Interface.GetChildById<Button>(LOGOUT_BUTTON);
        if (_buttonLogout != default)
        {
            _buttonLogout.SetText(Strings.CharacterSelection.Logout);
            _buttonLogout.Visible = true;
            _buttonLogout.Click += _buttonLogout_Clicked;
        }
    }

    public void Show()
    {
        if (_selectCharacterWindow == default)
        {
            return;
        }

        _selectCharacterWindow.Visible = true;
        if (_renderLayers == default)
        {
            _renderLayers = new Image[Options.Equipment.Paperdoll.Down.Count];
            for (var i = 0; i < _renderLayers.Length; i++)
            {
                _renderLayers[i] = new Image
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                _charContainer?.Widgets.Add(_renderLayers[i]);
            }
        }

        Characters ??= new Character[Options.MaxCharacters];
        SelectedChar = 0;
        UpdateDisplay();
    }

    public void Hide()
    {
        if (_selectCharacterWindow == default)
        {
            return;
        }

        _selectCharacterWindow.Visible = false;
    }

    public void Update()
    {
        if (!Networking.Network.IsConnected)
        {
            _mainMenu.SwitchToWindow<LoginWindow>();
        }

        // Re-Enable our buttons if we're not waiting for the server anymore with it disabled.
        _buttonPlay.ToggleEnabled(!Globals.WaitingOnServer);
        _buttonNew.ToggleEnabled(!Globals.WaitingOnServer);
        _buttonDelete.ToggleEnabled(!Globals.WaitingOnServer);
        _buttonLogout.ToggleEnabled(!Globals.WaitingOnServer);
    }

    private void UpdateDisplay()
    {
        if (_renderLayers == default || Characters == default)
        {
            return;
        }
        if (Characters == default)
        {
            return;
        }

        var charCount = Characters.Length >= 1;
        _buttonNextChar.ToggleVisible(charCount);
        _buttonPrevChar.ToggleVisible(charCount);
        if (charCount)
        {
            _buttonNextChar?.BringToFront();
            _buttonPrevChar?.BringToFront();
        }

        foreach (var paperdollPortrait in _renderLayers)
        {
            paperdollPortrait.Visible = false;
        }

        if (Characters?[SelectedChar] == default)
        {
            _buttonPlay.ToggleVisible(false);
            _buttonDelete.ToggleVisible(false);
            _buttonNew.ToggleVisible(true);
            _labelCharname.SetText(Strings.CharacterSelection.Empty);
            _labelInfo.SetText(string.Empty);
            return;
        }

        _labelCharname.SetText(Strings.CharacterSelection.Name.ToString(Characters[SelectedChar].Name));
        _labelInfo.SetText(Strings.CharacterSelection.Info.ToString(Characters[SelectedChar].Level, Characters[SelectedChar].Class));
        _buttonPlay.ToggleVisible(true);
        _buttonDelete.ToggleVisible(true);
        _buttonNew.ToggleVisible(false);
        if (_renderLayers == default)
        {
            _renderLayers = new Image[Options.Equipment.Paperdoll.Down.Count];
            for (var i = 0; i < _renderLayers.Length; i++)
            {
                _renderLayers[i] = new Image
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                _charContainer?.Widgets.Add(_renderLayers[i]);
            }
        }

        for (var i = 0; i < Options.Equipment.Paperdoll.Down.Count; i++)
        {
            var equipment = Options.Equipment.Paperdoll.Down[i];
            var paperdollContainer = _renderLayers[i];
            var isFace = false;
            if (string.Equals("Player", equipment, StringComparison.Ordinal))
            {
                var faceSource = Characters[SelectedChar].Face;
                var spriteSource = Characters[SelectedChar].Sprite;
                var faceTex = (IImage)Globals.ContentManager.GetTexture(TextureType.Face, faceSource);
                var spriteTex = (IImage)Globals.ContentManager.GetTexture(TextureType.Entity, spriteSource);
                isFace = faceTex != default;
                paperdollContainer.Renderable = isFace ? faceTex : spriteTex;
            }
            else
            {
                if (i >= Characters[SelectedChar].Equipment.Length)
                {
                    continue;
                }

                var equipFragment = Characters[SelectedChar].Equipment[i];
                if (equipFragment == default)
                {
                    paperdollContainer.Renderable = default;
                    continue;
                }

                paperdollContainer.Renderable = (IImage)Globals.ContentManager.GetTexture(TextureType.Paperdoll, equipFragment.Name);
                if (paperdollContainer.Renderable != default)
                {
                    paperdollContainer.Color = new Microsoft.Xna.Framework.Color(
                        equipFragment.RenderColor.R,
                        equipFragment.RenderColor.G,
                        equipFragment.RenderColor.B,
                        equipFragment.RenderColor.A
                    );
                }
            }

            if (paperdollContainer.Renderable == default)
            {
                paperdollContainer.Visible = false;
                continue;
            }

            paperdollContainer.Visible = true;
        }
    }

    private void _buttonLogout_Clicked(object? sender, EventArgs? arguments)
    {
        Main.Logout(false, skipFade: true);
        _mainMenu.SwitchToWindow<LoginWindow>();
    }

    private void _buttonPrevChar_Clicked(object? sender, EventArgs? arguments)
    {
        if (Characters == default)
        {
            return;
        }

        SelectedChar--;
        if (SelectedChar < 0)
        {
            SelectedChar = Characters.Length - 1;
        }

        UpdateDisplay();
    }

    private void _buttonNextChar_Clicked(object? sender, EventArgs? arguments)
    {
        if (Characters == default)
        {
            return;
        }

        SelectedChar++;
        if (SelectedChar >= Characters.Length)
        {
            SelectedChar = 0;
        }

        UpdateDisplay();
    }

    private void _buttonDelete_Clicked(object? sender, EventArgs? arguments)
    {
        if (Globals.WaitingOnServer || Characters == default)
        {
            return;
        }

        _ = new InputBox(
            Strings.CharacterSelection.DeleteTitle.ToString(Characters[SelectedChar].Name),
            Strings.CharacterSelection.DeletePrompt.ToString(Characters[SelectedChar].Name),
            InputBox.InputType.YesNo,
            DeleteCharacter
        );
    }

    private void DeleteCharacter(object? sender, EventArgs? arguments)
    {
        if (Globals.WaitingOnServer || Characters == default)
        {
            return;
        }

        PacketSender.SendDeleteCharacter(Characters[SelectedChar].Id);
        Globals.WaitingOnServer = true;
        _buttonPlay.ToggleEnabled(false);
        _buttonNew.ToggleEnabled(false);
        _buttonDelete.ToggleEnabled(false);
        _buttonLogout.ToggleEnabled(false);
        SelectedChar = 0;
        UpdateDisplay();
    }

    private void _buttonNew_Clicked(object? sender, EventArgs? arguments)
    {
        if (Globals.WaitingOnServer)
        {
            return;
        }

        PacketSender.SendNewCharacter();
        Globals.WaitingOnServer = true;
        _buttonPlay.ToggleEnabled(false);
        _buttonNew.ToggleEnabled(false);
        _buttonDelete.ToggleEnabled(false);
        _buttonLogout.ToggleEnabled(false);
    }

    public void ButtonPlay_Clicked(object? sender, EventArgs? arguments)
    {
        if (Globals.WaitingOnServer || Characters == default)
        {
            return;
        }

        //ChatboxMsg.ClearMessages();
        PacketSender.SendSelectCharacter(Characters[SelectedChar].Id);
        Globals.WaitingOnServer = true;
        _buttonPlay.ToggleEnabled(false);
        _buttonNew.ToggleEnabled(false);
        _buttonDelete.ToggleEnabled(false);
        _buttonLogout.ToggleEnabled(false);
    }

    #region Constants

    private const string SELECT_CHARACTER_TITLE_LABEL = nameof(SELECT_CHARACTER_TITLE_LABEL);
    private const string CHARNAME_LABEL = nameof(CHARNAME_LABEL);
    private const string CHARINFO_LABEL = nameof(CHARINFO_LABEL);
    private const string NEXT_CHAR_BUTTON = nameof(NEXT_CHAR_BUTTON);
    private const string PREV_CHAR_BUTTON = nameof(PREV_CHAR_BUTTON);
    private const string EDITOR_MODE_BUTTON = nameof(EDITOR_MODE_BUTTON);
    private const string PLAY_BUTTON = nameof(PLAY_BUTTON);
    private const string DELETE_BUTTON = nameof(DELETE_BUTTON);
    private const string NEWCHAR_BUTTON = nameof(NEWCHAR_BUTTON);
    private const string LOGOUT_BUTTON = nameof(LOGOUT_BUTTON);
    private const string CHAR_CONTAINER = nameof(CHAR_CONTAINER);

    #endregion
}

public partial class Character(
    Guid id,
    string name,
    string sprite,
    string face,
    int level,
    string charClass,
    EquipmentFragment[] equipment)
{
    public Guid Id = id;
    public readonly string Class = charClass;
    public EquipmentFragment?[] Equipment = equipment;
    public string Face = face;
    public readonly int Level = level;
    public readonly string Name = name;
    public string Sprite = sprite;
}
