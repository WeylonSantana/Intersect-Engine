using Intersect.Client.Core;
using Intersect.Client.Framework.Content;
using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.Client.Networking;
using Intersect.Network.Packets.Server;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Menu;

public partial class SelectCharacterWindow : IMainMenuWindow
{
    private MenuInterface _mainMenu = null!;
    private Widget? _selectCharacterWindow;
    private Label? _labelCharname;
    private Label? _labelInfo;
    //private Panel? _charContainer;
    private Button? _buttonNextChar;
    private Button? _buttonPrevChar;
    private Button? _buttonPlay;
    private Button? _buttonDelete;
    private Button? _buttonNew;
    private Button? _buttonLogout;
    //private Image[]? _renderLayers;
    public Character[] Characters;
    public int mSelectedChar = 0;

    public bool Visible => _selectCharacterWindow?.Visible ?? false;

    public void Load(MenuInterface mainMenu)
    {
        _mainMenu = mainMenu;
        _selectCharacterWindow = Interface.LoadContent(Path.Combine("menu", "SelectCharacterWindow.xmmp"));

        //Menu Header
        if (Interface.GetChildById<Label>("_labelSelectCharacterTitle", out var labelSelectCharacterTitle))
        {
            labelSelectCharacterTitle.Text = Strings.CharacterSelection.Title;
        }

        //Character Name
        if (Interface.GetChildById<Label>("_labelCharname", out var labelCharname))
        {
            labelCharname.Text = Strings.CharacterSelection.Empty;
            _labelCharname = labelCharname;
        }

        //Character Info
        if (Interface.GetChildById<Label>("_labelInfo", out var labelInfo))
        {
            labelInfo.Text = Strings.CharacterSelection.New;
            _labelInfo = labelInfo;
        }

        //Character Container
        if (Interface.GetChildById<Panel>("_charContainer", out var charContainer))
        {
            //_charContainer = charContainer;
        }

        //Next char Button
        if (Interface.GetChildById<Button>("_buttonNextChar", out var buttonNextChar))
        {
            buttonNextChar.Click += _buttonNextChar_Clicked;
            _buttonNextChar = buttonNextChar;
        }

        //Prev Char Button
        if (Interface.GetChildById<Button>("_buttonPrevChar", out var buttonPrevChar))
        {
            buttonPrevChar.Click += _buttonPrevChar_Clicked;
            _buttonPrevChar = buttonPrevChar;
        }

        //Play Button
        if (Interface.GetChildById<Label>("_labelPlay", out var labelPlay))
        {
            labelPlay.Text = Strings.CharacterSelection.Play;
        }

        if (Interface.GetChildById<Button>("_buttonPlay", out var buttonPlay))
        {
            _buttonPlay = buttonPlay;
            _buttonPlay.Visible = false;
            _buttonPlay.Click += ButtonPlay_Clicked;
        }

        //Delete Button
        if (Interface.GetChildById<Label>("_labelDelete", out var labelDelete))
        {
            labelDelete.Text = Strings.CharacterSelection.Delete;
        }

        if (Interface.GetChildById<Button>("_buttonDelete", out var buttonDelete))
        {
            _buttonDelete = buttonDelete;
            _buttonDelete.Visible = false;
            _buttonDelete.Click += _buttonDelete_Clicked;
        }

        //Create new char Button
        if (Interface.GetChildById<Label>("_labelNew", out var labelNew))
        {
            labelNew.Text = Strings.CharacterSelection.New;
        }

        if (Interface.GetChildById<Button>("_buttonNew", out var buttonNew))
        {
            _buttonNew = buttonNew;
            _buttonNew.Click += _buttonNew_Clicked;
        }

        //Logout Button
        if (Interface.GetChildById<Label>("_labelLogout", out var labelLogout))
        {
            labelLogout.Text = Strings.CharacterSelection.Logout;
        }

        if (Interface.GetChildById<Button>("_buttonLogout", out var buttonLogout))
        {
            _buttonLogout = buttonLogout;
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
        /*if (_renderLayers == default)
        {
            _renderLayers = new Image[Options.Equipment.Paperdoll.Down.Count];
            for (var i = 0; i < _renderLayers.Length; i++)
            {
                _renderLayers[i] = new Image
                {
                    HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
                };
                _charContainer.Widgets.Add(_renderLayers[i]);
            }
        }

        if (Characters == default)
        {
            Characters = new Character[Options.MaxCharacters];
        }*/
        mSelectedChar = 0;
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
        _buttonPlay.Enabled = !Globals.WaitingOnServer;
        _buttonNew.Enabled = !Globals.WaitingOnServer;
        _buttonDelete.Enabled = !Globals.WaitingOnServer;
        _buttonLogout.Enabled = !Globals.WaitingOnServer;
    }

    private void UpdateDisplay()
    {
        /*if (_renderLayers == default || Characters == default)
        {
            return;
        }*/
        if (Characters == default)
        {
            return;
        }

        var charCount = Characters.Length >= 1;
        _buttonNextChar.Visible = charCount;
        _buttonPrevChar.Visible = charCount;
        if (charCount)
        {
            _buttonNextChar.BringToFront();
            _buttonPrevChar.BringToFront();
        }

        /*foreach (var paperdollPortrait in _renderLayers)
        {
            paperdollPortrait.Visible = false;
        }*/
        if (Characters[mSelectedChar] == default)
        {
            _buttonPlay.Visible = false;
            _buttonDelete.Visible = false;
            _buttonNew.Visible = true;
            _labelCharname.Text = Strings.CharacterSelection.Empty;
            _labelInfo.Text = string.Empty;
            return;
        }

        _labelCharname.Text = Strings.CharacterSelection.Name.ToString(Characters[mSelectedChar].Name);
        _labelInfo.Text = Strings.CharacterSelection.Info.ToString(
            Characters[mSelectedChar].Level,
            Characters[mSelectedChar].Class
        );
        _buttonPlay.Visible = true;
        _buttonDelete.Visible = true;
        _buttonNew.Visible = false;
        for (var i = 0; i < Options.Equipment.Paperdoll.Down.Count; i++)
        {
            var equipment = Options.Equipment.Paperdoll.Down[i];
            //var paperdollContainer = _renderLayers[i];
            var isFace = false;
            if (string.Equals("Player", equipment, StringComparison.Ordinal))
            {
                var faceSource = Characters[mSelectedChar].Face;
                var spriteSource = Characters[mSelectedChar].Sprite;
                var faceTex = Globals.ContentManager.GetTexture(TextureType.Face, faceSource);
                var spriteTex = Globals.ContentManager.GetTexture(TextureType.Entity, spriteSource);
                isFace = faceTex != default;
                //paperdollContainer.Renderable = isFace ? new TextureRegion(faceTex) : new TextureRegion(spriteTex);
            }
            else
            {
                if (i >= Characters[mSelectedChar].Equipment.Length)
                {
                    continue;
                }

                var equipFragment = Characters[mSelectedChar].Equipment[i];
                if (equipFragment == default)
                {
                    //paperdollContainer.Renderable = default;
                    continue;
                }

                //paperdollContainer.Renderable = new TextureRegion(Globals.ContentManager.GetTexture(TextureType.Paperdoll, equipFragment.Name).Texture);
                /*if (paperdollContainer.Renderable != default)
                {
                    paperdollContainer.Color = new Microsoft.Xna.Framework.Color(
                        equipFragment.RenderColor.R,
                        equipFragment.RenderColor.G,
                        equipFragment.RenderColor.B,
                        equipFragment.RenderColor.A
                    );
                }*/
            }

            /*if (paperdollContainer.Renderable == default)
            {
                paperdollContainer.Visible = false;
                continue;
            }

            paperdollContainer.Visible = true;*/
        }
    }

    private void _buttonLogout_Clicked(object? sender, EventArgs? arguments)
    {
        Main.Logout(false, skipFade: true);
        _mainMenu.SwitchToWindow<LoginWindow>();
    }

    private void _buttonPrevChar_Clicked(object? sender, EventArgs? arguments)
    {
        mSelectedChar--;
        if (mSelectedChar < 0)
        {
            mSelectedChar = Characters!.Length - 1;
        }

        UpdateDisplay();
    }

    private void _buttonNextChar_Clicked(object? sender, EventArgs? arguments)
    {
        mSelectedChar++;
        if (mSelectedChar >= Characters!.Length)
        {
            mSelectedChar = 0;
        }

        UpdateDisplay();
    }

    private void _buttonDelete_Clicked(object? sender, EventArgs? arguments)
    {
        if (Globals.WaitingOnServer || Characters == default)
        {
            return;
        }

        var dialog = Dialog.CreateMessageBox(
            Strings.CharacterSelection.DeleteTitle.ToString(Characters[mSelectedChar].Name),
            Strings.CharacterSelection.DeletePrompt.ToString(Characters[mSelectedChar].Name)
        );
        dialog.Closed += (s, a) =>
        {
            if (dialog.Result)
            {
                DeleteCharacter(Characters[mSelectedChar].Id);
            }
        };
        dialog.ShowModal(Interface.Desktop);
    }

    private void DeleteCharacter(Guid charId)
    {
        PacketSender.SendDeleteCharacter(charId);
        Globals.WaitingOnServer = true;
        _buttonPlay.Enabled = false;
        _buttonNew.Enabled = false;
        _buttonDelete.Enabled = false;
        _buttonLogout.Enabled = false;
        mSelectedChar = 0;
        UpdateDisplay();
    }

    private void _buttonNew_Clicked(object sender, EventArgs arguments)
    {
        if (Globals.WaitingOnServer)
        {
            return;
        }

        PacketSender.SendNewCharacter();
        Globals.WaitingOnServer = true;
        _buttonPlay.Enabled = false;
        _buttonNew.Enabled = false;
        _buttonDelete.Enabled = false;
        _buttonLogout.Enabled = false;
    }

    public void ButtonPlay_Clicked(object sender, EventArgs arguments)
    {
        if (Globals.WaitingOnServer || Characters == default)
        {
            return;
        }

        //ChatboxMsg.ClearMessages();
        PacketSender.SendSelectCharacter(Characters[mSelectedChar].Id);
        Globals.WaitingOnServer = true;
        _buttonPlay.Enabled = false;
        _buttonNew.Enabled = false;
        _buttonDelete.Enabled = false;
        _buttonLogout.Enabled = false;
    }
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
    public string Class = charClass;
    public EquipmentFragment?[] Equipment = equipment;
    public string Face = face;
    public int Level = level;
    public string Name = name;
    public string Sprite = sprite;
}
