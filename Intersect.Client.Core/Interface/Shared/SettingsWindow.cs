using Intersect.Client.Core;
using Intersect.Client.Core.Controls;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.Framework.Graphics;
using Intersect.Client.General;
using Intersect.Client.Interface.Extensions;
using Intersect.Client.Interface.Menu;
using Intersect.Client.Localization;
using Intersect.Enums;
using Intersect.Utilities;
using Myra.Graphics2D.UI;
using MathHelper = Intersect.Client.Utilities.MathHelper;

namespace Intersect.Client.Interface.Shared;

public class SettingsWindow : IMainMenuWindow
{
    private MenuInterface _menuInterface = default!;
    private Widget? _settingsWindow;

    private TabControl? _tabControl;

    // Game Settings - Interface.
    private CheckButton? _autoCloseWindowsCheckButton;
    private CheckButton? _autoToggleChatLogCheckButton;
    private CheckButton? _showHealthAsPercentageCheckButton;
    private CheckButton? _showManaAsPercentageCheckButton;
    private CheckButton? _showExperienceAsPercentageCheckButton;
    private CheckButton? _typewriterCheckButton;
    private CheckButton? _simplifiedEscapeMenuCheckButton;

    // Game Settings - Overhead Information.
    private CheckButton? _myOverheadInfoCheckButton;
    private CheckButton? _playerOverheadInfoCheckButton;
    private CheckButton? _npcOverheadInfoCheckButton;
    private CheckButton? _friendOverheadInfoCheckbox;
    private CheckButton? _partyOverheadInfoCheckButton;
    private CheckButton? _guildOverheadInfoCheckButton;

    // Game Settings - Overhead HP Bar.
    private CheckButton? _myOverheadHpBarCheckButton;
    private CheckButton? _playerOverheadHpBarCheckButton;
    private CheckButton? _npcOverheadHpBarCheckButton;
    private CheckButton? _friendOverheadHpBarCheckButton;
    private CheckButton? _partyOverheadHpBarCheckButton;
    private CheckButton? _guildOverheadHpBarCheckButton;

    // Game Settings - Targeting.
    private CheckButton? _stickyTargetCheckButton;
    private CheckButton? _autoTurnToTargetCheckButton;

    // Video Settings.
    private ComboView? _resolutionList;
    private ComboView? _fpsList;
    private HorizontalSlider? _worldScale;
    private CheckButton? _fullscreenCheckbox;
    private CheckButton? _lightingEnabledCheckbox;

    // Audio Settings.
    private Label? _musicLabel;
    private HorizontalSlider? _musicSlider;
    private Label? _soundLabel;
    private HorizontalSlider? _soundSlider;

    // Keybinding Settings.
    private Button? _keybindingRestoreBtn;
    private Container? _keybindingControlsContainer;
    private Container? _keybindingControlsRow;
    private Button? _currentListeningButton;

    private Controls _keybindingEditControls = default!;
    private HashSet<Keys> _keysDown = [];
    private Dictionary<Control, Button[]> _keybindingButtons = [];
    private Control _keybindingEditControl;
    private long _keybindingListeningTimer;

    private bool _returnToMenu;

    public bool Visible
    {
        get => _settingsWindow?.Visible ?? false;
        set => _settingsWindow?.SetVisible(value);
    }

    private int _previousMusicVolume;
    private int _previousSoundVolume;

    public void Load(MenuInterface menu)
    {
        _menuInterface = menu;
        _settingsWindow = Interface.LoadContent(Path.Combine("shared", "SettingsWindow.xmmp"));
        _keybindingEditControls = new Controls(Controls.ActiveControls);

        #region Basic Load

        if (_settingsWindow.FindChildById<TabControl>(TAB_CONTROL, out var tabControl))
        {
            _tabControl = tabControl;
            _tabControl.SelectedIndexChanged += (s, e) =>
            {
                _keybindingRestoreBtn.SetVisible(_tabControl.SelectedItem.Id == KEYBINDING_TAB);
            };

            foreach (var tab in _tabControl.Items)
            {
                switch (tab.Id)
                {
                    case GAME_TAB:
                        tab.Text = Strings.Settings.GameSettingsTab;
                        break;

                    case VIDEO_TAB:
                        tab.Text = Strings.Settings.VideoSettingsTab;
                        break;

                    case AUDIO_TAB:
                        tab.Text = Strings.Settings.AudioSettingsTab;
                        break;

                    case KEYBINDING_TAB:
                        tab.Text = Strings.Settings.KeyBindingSettingsTab;
                        break;
                }
            }
        }

        if (_settingsWindow.FindChildById<Button>(SAVE_BUTTON, out var saveButton))
        {
            saveButton.Click += SettingsApplyBtn_Clicked;
            saveButton.SetText(Strings.Settings.Apply);
        }

        if (_settingsWindow.FindChildById<Button>(CANCEL_BUTTON, out var cancelButton))
        {
            cancelButton.Click += SettingsCancelBtn_Clicked;
            cancelButton.SetText(Strings.Settings.Cancel);
        }

        #endregion

        #region Game Settings Load - Interface

        var gameTabContent = _tabControl?.Items.FirstOrDefault(t => t.Id == GAME_TAB)?.Content;

        _autoCloseWindowsCheckButton = gameTabContent?.FindChildById<CheckButton>(AUTO_CLOSE_WINDOW_CHECK);
        _autoCloseWindowsCheckButton?.SetText(Strings.Settings.AutoCloseWindows);
        _autoCloseWindowsCheckButton?.SetValue(Globals.Database.HideOthersOnWindowOpen);

        _autoToggleChatLogCheckButton = gameTabContent?.FindChildById<CheckButton>(AUTO_TOGGLE_CHAT_LOG_CHECK);
        _autoToggleChatLogCheckButton?.SetValue(Globals.Database.AutoToggleChatLog);
        _autoToggleChatLogCheckButton?.SetText(Strings.Settings.AutoToggleChatLog);

        _showHealthAsPercentageCheckButton = gameTabContent?.FindChildById<CheckButton>(SHOW_HEALTH_AS_PERCENTAGE_CHECK);
        _showHealthAsPercentageCheckButton?.SetText(Strings.Settings.ShowHealthAsPercentage);
        _showHealthAsPercentageCheckButton?.SetValue(Globals.Database.ShowHealthAsPercentage);

        _showManaAsPercentageCheckButton = gameTabContent?.FindChildById<CheckButton>(SHOW_MANA_AS_PERCENTAGE_CHECK);
        _showManaAsPercentageCheckButton?.SetText(Strings.Settings.ShowManaAsPercentage);
        _showManaAsPercentageCheckButton?.SetValue(Globals.Database.ShowManaAsPercentage);

        _showExperienceAsPercentageCheckButton = gameTabContent?.FindChildById<CheckButton>(SHOW_EXPERIENCE_AS_PERCENTAGE_CHECK);
        _showExperienceAsPercentageCheckButton?.SetText(Strings.Settings.ShowExperienceAsPercentage);
        _showExperienceAsPercentageCheckButton?.SetValue(Globals.Database.ShowExperienceAsPercentage);

        _typewriterCheckButton = gameTabContent?.FindChildById<CheckButton>(TYPEWRITER_CHECK);
        _typewriterCheckButton?.SetText(Strings.Settings.TypewriterText);
        _typewriterCheckButton?.SetValue(Globals.Database.TypewriterBehavior == TypewriterBehavior.Word);

        _simplifiedEscapeMenuCheckButton = gameTabContent?.FindChildById<CheckButton>(SIMPLIFIED_ESCAPE_MENU_CHECK);
        _simplifiedEscapeMenuCheckButton?.SetText(Strings.Settings.SimplifiedEscapeMenu);
        _simplifiedEscapeMenuCheckButton?.SetValue(Globals.Database.SimplifiedEscapeMenu);

        #endregion

        #region Game Settings Load - Overhead Information

        gameTabContent?.FindChildById<Label>(GAME_OVERHEAD_INFO_LABEL)?.SetText(Strings.Settings.ShowOverheadInformationTitle);
        _myOverheadInfoCheckButton = gameTabContent?.FindChildById<CheckButton>(MY_OVERHEAD_INFO_CHECK);
        _myOverheadInfoCheckButton?.SetText(Strings.Settings.ShowMyOverheadInformation);
        _myOverheadInfoCheckButton?.SetValue(Globals.Database.MyOverheadInfo);

        _playerOverheadInfoCheckButton = gameTabContent?.FindChildById<CheckButton>(PLAYER_OVERHEAD_INFO_CHECK);
        _playerOverheadInfoCheckButton?.SetText(Strings.Settings.ShowPlayerOverheadInformation);
        _playerOverheadInfoCheckButton?.SetValue(Globals.Database.PlayerOverheadInfo);

        _npcOverheadInfoCheckButton = gameTabContent?.FindChildById<CheckButton>(NPC_OVERHEAD_INFO_CHECK);
        _npcOverheadInfoCheckButton?.SetText(Strings.Settings.ShowNpcOverheadInformation);
        _npcOverheadInfoCheckButton?.SetValue(Globals.Database.NpcOverheadInfo);

        _friendOverheadInfoCheckbox = gameTabContent?.FindChildById<CheckButton>(FRIEND_OVERHEAD_INFO_CHECK);
        _friendOverheadInfoCheckbox?.SetValue(Globals.Database.FriendOverheadInfo);
        _friendOverheadInfoCheckbox?.SetText(Strings.Settings.ShowFriendOverheadInformation);

        _partyOverheadInfoCheckButton = gameTabContent?.FindChildById<CheckButton>(PARTY_OVERHEAD_INFO_CHECK);
        _partyOverheadInfoCheckButton?.SetText(Strings.Settings.ShowPartyOverheadInformation);
        _partyOverheadInfoCheckButton?.SetValue(Globals.Database.PartyMemberOverheadInfo);

        _guildOverheadInfoCheckButton = gameTabContent?.FindChildById<CheckButton>(GUILD_OVERHEAD_INFO_CHECK);
        _guildOverheadInfoCheckButton?.SetText(Strings.Settings.ShowGuildOverheadInformation);
        _guildOverheadInfoCheckButton?.SetValue(Globals.Database.GuildMemberOverheadInfo);

        #endregion

        #region Game Settings Load - Overhead HP Bar

        gameTabContent?.FindChildById<Label>(GAME_OVERHEAD_HP_BAR_LABEL)?.SetText(Strings.Settings.ShowOverheadHPBarTitle);
        _myOverheadHpBarCheckButton = gameTabContent?.FindChildById<CheckButton>(MY_OVERHEAD_HP_BAR_CHECK);
        _myOverheadHpBarCheckButton?.SetText(Strings.Settings.ShowMyOverheadHPBar);
        _myOverheadHpBarCheckButton?.SetValue(Globals.Database.MyOverheadHpBar);

        _playerOverheadHpBarCheckButton = gameTabContent?.FindChildById<CheckButton>(PLAYER_OVERHEAD_HP_BAR_CHECK);
        _playerOverheadHpBarCheckButton?.SetText(Strings.Settings.ShowPlayerOverheadHPBar);
        _playerOverheadHpBarCheckButton?.SetValue(Globals.Database.PlayerOverheadHpBar);

        _npcOverheadHpBarCheckButton = gameTabContent?.FindChildById<CheckButton>(NPC_OVERHEAD_HP_BAR_CHECK);
        _npcOverheadHpBarCheckButton?.SetText(Strings.Settings.ShowNpcOverheadHPBar);
        _npcOverheadHpBarCheckButton?.SetValue(Globals.Database.NpcOverheadHpBar);

        _friendOverheadHpBarCheckButton = gameTabContent?.FindChildById<CheckButton>(FRIEND_OVERHEAD_HP_BAR_CHECK);
        _friendOverheadHpBarCheckButton?.SetText(Strings.Settings.ShowFriendOverheadHPBar);
        _friendOverheadHpBarCheckButton?.SetValue(Globals.Database.FriendOverheadHpBar);

        _partyOverheadHpBarCheckButton = gameTabContent?.FindChildById<CheckButton>(PARTY_OVERHEAD_HP_BAR_CHECK);
        _partyOverheadHpBarCheckButton?.SetText(Strings.Settings.ShowPartyOverheadHPBar);
        _partyOverheadHpBarCheckButton?.SetValue(Globals.Database.PartyMemberOverheadHpBar);

        _guildOverheadHpBarCheckButton = gameTabContent?.FindChildById<CheckButton>(GUILD_OVERHEAD_HP_BAR_CHECK);
        _guildOverheadHpBarCheckButton?.SetText(Strings.Settings.ShowGuildOverheadHPBar);
        _guildOverheadHpBarCheckButton?.SetValue(Globals.Database.GuildMemberOverheadHpBar);

        #endregion

        #region Game Settings Load - Targeting

        gameTabContent?.FindChildById<Label>(TARGETING_LABEL)?.SetText(Strings.Settings.TargettingTitle);
        _stickyTargetCheckButton = gameTabContent?.FindChildById<CheckButton>(STICKY_TARGET_CHECK);
        _stickyTargetCheckButton?.SetText(Strings.Settings.StickyTarget);
        _stickyTargetCheckButton?.SetValue(Globals.Database.StickyTarget);

        _autoTurnToTargetCheckButton = gameTabContent?.FindChildById<CheckButton>(AUTO_TURN_TO_TARGET_CHECK);
        _autoTurnToTargetCheckButton?.SetText(Strings.Settings.AutoTurnToTarget);
        _autoTurnToTargetCheckButton?.SetValue(Globals.Database.AutoTurnToTarget);

        #endregion

        #region Video Settings Load

        var videoTabContent = _tabControl?.Items.FirstOrDefault(t => t.Id == VIDEO_TAB)?.Content;

        videoTabContent?.FindChildById<Label>(RESOLUTION_LABEL)?.SetText(Strings.Settings.Resolution);
        _resolutionList = videoTabContent?.FindChildById<ComboView>(RESOLUTION_COMBO_VIEW);
        if (_resolutionList != default)
        {
            // Add valid video modes to the resolution list.
            Graphics.Renderer?.GetValidVideoModes().ForEach(t => { _resolutionList.AddItem(t); });

            // Select the current resolution.
            var validVideoModes = Graphics.Renderer?.GetValidVideoModes();
            if (validVideoModes?.Count > 0)
            {
                var targetResolution = Globals.Database.TargetResolution;
                var resolutionLabel = targetResolution < 0 || validVideoModes.Count <= targetResolution
                    ? Strings.Settings.ResolutionCustom.ToString()
                    : validVideoModes[targetResolution];
                _resolutionList.SelectByText(resolutionLabel);
            }
        }

        videoTabContent?.FindChildById<Label>(FPS_LABEL)?.SetText(Strings.Settings.TargetFps);
        _fpsList = videoTabContent?.FindChildById<ComboView>(FPS_COMBO_VIEW);

        if (_fpsList != default)
        {
            var validFps = new Dictionary<int, string>
            {
                { 0, Strings.Settings.Vsync},
                { 1, Strings.Settings.Fps30},
                { 2, Strings.Settings.Fps60},
                { 3, Strings.Settings.Fps90},
                { 4, Strings.Settings.Fps120},
                { 5, Strings.Settings.UnlimitedFps},
            };

            // Add valid fps to the fps list.
            foreach (var fps in validFps)
            {
                _fpsList.AddItem(fps.Value, fps.Key.ToString());
            }

            // Select the current fps.
            _fpsList.SelectByKey(Globals.Database.TargetFps.ToString());
        }

        videoTabContent?.FindChildById<Label>(WORLD_SCALE_LABEL)?.SetText(Strings.Settings.WorldScale);
        _worldScale = videoTabContent?.FindChildById<HorizontalSlider>(WORLD_SCALE_SLIDER);
        _worldScale?.SetValue(Globals.Database.WorldZoom);

        _fullscreenCheckbox = videoTabContent?.FindChildById<CheckButton>(FULLSCREEN_CHECK);
        _fullscreenCheckbox?.SetText(Strings.Settings.Fullscreen);
        _fullscreenCheckbox?.SetValue(Globals.Database.FullScreen);

        _lightingEnabledCheckbox = videoTabContent?.FindChildById<CheckButton>(LIGHTING_ENABLED_CHECK);
        _lightingEnabledCheckbox?.SetText(Strings.Settings.EnableLighting);
        _lightingEnabledCheckbox?.SetValue(Globals.Database.EnableLighting);

        #endregion

        #region Audio Settings Load

        var audioTabContent = _tabControl?.Items.FirstOrDefault(t => t.Id == AUDIO_TAB)?.Content;

        _previousMusicVolume = Globals.Database.MusicVolume;
        _previousSoundVolume = Globals.Database.SoundVolume;

        _musicSlider = audioTabContent?.FindChildById<HorizontalSlider>(MUSIC_VOLUME_SLIDER);
        if (_musicSlider != default)
        {
            _musicSlider.ValueChangedByUser += MusicSlider_ValueChanged;
            _musicSlider.SetValue(Globals.Database.MusicVolume);
        }

        _musicLabel = audioTabContent?.FindChildById<Label>(MUSIC_VOLUME_LABEL);
        var musicLabelText = Strings.Settings.MusicVolume.ToString((int)(_musicSlider?.Value ?? 0));
        _musicLabel?.SetText(musicLabelText);

        _soundSlider = audioTabContent?.FindChildById<HorizontalSlider>(SOUND_VOLUME_SLIDER);
        if (_soundSlider != default)
        {
            _soundSlider.ValueChangedByUser += SoundSlider_ValueChanged;
            _soundSlider.SetValue(Globals.Database.SoundVolume);
        }

        _soundLabel = audioTabContent?.FindChildById<Label>(SOUND_VOLUME_LABEL);
        var soundLabelText = Strings.Settings.SoundVolume.ToString((int)(_soundSlider?.Value ?? 0));
        _soundLabel?.SetText(soundLabelText);

        #endregion

        #region Keybinding Settings Load

        var keybindingTabContent = _tabControl?.Items.FirstOrDefault(t => t.Id == KEYBINDING_TAB)?.Content;

        _keybindingControlsContainer = keybindingTabContent?.FindChildById<Container>(KEYBINDING_CONTROLS_CONTAINER);
        _keybindingControlsRow = keybindingTabContent?.FindChildById<Container>(KEYBINDING_CONTROLS_ROW);

        if (_keybindingControlsContainer != default && _keybindingControlsRow != default)
        {
            foreach (Control control in Enum.GetValues(typeof(Control)))
            {
                var row = _keybindingControlsRow.Clone() as Container;

                if (row == default)
                {
                    continue;
                }

                // Get elements we need or throw an exception.
                var label = row.Widgets.OfType<Label>().FirstOrDefault();
                var buttonPrimary = row.Widgets.OfType<Button>().FirstOrDefault();
                var buttonSecondary = row.Widgets.OfType<Button>().LastOrDefault();

                if (label == default || buttonPrimary == default || buttonSecondary == default)
                {
                    throw new Exception("Invalid keybinding row.");
                }

                var name = Enum.GetName(typeof(Control), control)?.ToLower() ?? string.Empty;
                label.SetText(Strings.Controls.KeyDictionary[name]);

                buttonPrimary.Click += (s, e) => EditKeyPressed(buttonPrimary);
                buttonPrimary.UserData.Add("control", control.ToString());
                buttonPrimary.UserData.Add("bindingIndex", "0");

                buttonSecondary.Click += (s, e) => EditKeyPressed(buttonSecondary);
                buttonSecondary.UserData.Add("control", control.ToString());
                buttonSecondary.UserData.Add("bindingIndex", "1");

                _keybindingButtons.Add(control, [buttonPrimary, buttonSecondary]);
                _keybindingControlsContainer.Widgets.Add(row);
            }

            // remove the original "template" row.
            _keybindingControlsContainer.Widgets.RemoveAt(0);
            UpdateKeybindingControlsLabels();

            Input.KeyDown += OnKeyDown;
            Input.MouseDown += OnKeyDown;
            Input.KeyUp += OnKeyUp;
            Input.MouseUp += OnKeyUp;
        }

        _keybindingRestoreBtn = _settingsWindow?.FindChildById<Button>(KEYBINDING_RESTORE_BUTTON);
        if (_keybindingRestoreBtn != default)
        {
            _keybindingRestoreBtn.SetText(Strings.Settings.Restore);
            _keybindingRestoreBtn.SetVisible(false);
            _keybindingRestoreBtn.Click += (s, e) =>
            {
                ResetKeybindingListener();
                _keybindingEditControls.ResetDefaults();
                UpdateKeybindingControlsLabels();
            };
        }

        #endregion
    }

    #region Settings Setup

    // inherited from IMainMenuWindow
    public void Show()
    {
        Show(false);
    }

    public void Show(bool returnToMenu)
    {
        Visible = true;

        // Set up whether we're supposed to return to the previous menu.
        _returnToMenu = returnToMenu;

        // Control Settings.
        _keybindingEditControls = new Controls(Controls.ActiveControls);

        // Load every GUI element to their default state when showing up the settings window (pressed tabs, containers, etc.)
        LoadSettingsWindow();
    }

    public void Hide()
    {
        Visible = false;

        // Return to our previous menus (or not) depending on gamestate and the method we'd been opened.
        if (!_returnToMenu)
        {
            return;
        }

        _returnToMenu = false;
        switch (Globals.GameState)
        {
            case GameStates.Menu:
                _menuInterface?.SwitchToWindow<LoginWindow>();
                break;

            case GameStates.InGame:
                // MYRA-TODO: Show the escape menu.
                // _escapeMenu?.Show();
                break;

            default:
                throw new NotImplementedException();
        }
    }

    private void LoadSettingsWindow()
    {
        if (_tabControl != default)
        {
            _tabControl.SelectedIndex = 0;
        }

        var worldScaleNotches = new float[]
        {
            1, 2, 4,
        }.Select(n => n * Graphics.BaseWorldScale).ToArray();

        Globals.Database.WorldZoom = MathHelper.Clamp(
            Globals.Database.WorldZoom,
            worldScaleNotches.Min(),
            worldScaleNotches.Max()
        );

        if (_worldScale != default)
        {
            _worldScale.SetRange(worldScaleNotches.Min(), worldScaleNotches.Max());
            _worldScale.SetValue(Globals.Database.WorldZoom);
        }
    }

    private void UpdateKeybindingControlsLabels()
    {
        foreach (Control control in Enum.GetValues(typeof(Control)))
        {
            var controlMapping = _keybindingEditControls.ControlMapping[control];
            for (var bindingIndex = 0; bindingIndex < controlMapping.Bindings.Count; bindingIndex++)
            {
                var binding = controlMapping.Bindings[bindingIndex];
                var text = Strings.Keys.FormatKeyName(binding.Modifier, binding.Key);
                var button = _keybindingButtons[control][bindingIndex];
                button.SetText(text);

                if (button.UserData.ContainsKey("modifier"))
                {
                    button.UserData["modifier"] = binding.Modifier.ToString();
                }
                else
                {
                    button.UserData.Add("modifier", binding.Modifier.ToString());
                }

                if (button.UserData.ContainsKey("key"))
                {
                    button.UserData["key"] = binding.Key.ToString();
                }
                else
                {
                    button.UserData.Add("key", binding.Key.ToString());
                }
            }
        }
    }

    #endregion

    public void Update()
    {
        if (!Visible)
        {
            return;
        }

        if (_currentListeningButton == default)
        {
            return;
        }

        if (_keybindingListeningTimer >= Timing.Global.MillisecondsUtc)
        {
            return;
        }

        OnKeyUp(Keys.None, Keys.None);
    }

    #region Controls Handling

    private void MusicSlider_ValueChanged(object? sender, EventArgs arguments)
    {
        var value = (int)(_musicSlider?.Value ?? 0);
        _musicLabel?.SetText(Strings.Settings.MusicVolume.ToString(value));
        Globals.Database.MusicVolume = value;
        Audio.UpdateGlobalVolume();
    }

    private void SoundSlider_ValueChanged(object? sender, EventArgs arguments)
    {
        var value = (int)(_soundSlider?.Value ?? 0);
        _soundLabel?.SetText(Strings.Settings.SoundVolume.ToString(value));
        Globals.Database.SoundVolume = value;
        Audio.UpdateGlobalVolume();
    }

    private void EditKeyPressed(Button sender)
    {
        if (_currentListeningButton != null)
        {
            return;
        }

        sender.SetText(Strings.Controls.Listening);
        _keybindingEditControl = (Control)Enum.Parse(typeof(Control), sender.UserData["control"]);
        _currentListeningButton = sender;
        _keybindingListeningTimer = Timing.Global.MillisecondsUtc + 3000;
        Interface.SetInputFocus(default);
    }

    private void OnKeyDown(Keys modifier, Keys key)
    {
        if (_currentListeningButton == null)
        {
            return;
        }

        if (_keysDown.Count == 2)
        {
            return;
        }

        _keysDown.Add(key);
    }

    private void OnKeyUp(Keys modifier, Keys key)
    {
        if (_currentListeningButton == null)
        {
            return;
        }

        // if we have no keybinding, then we're done.
        if (key == Keys.None)
        {
            ResetKeybindingListener();
            return;
        }

        // ok, we have a valid keybinding: single keybinding, or a modifier + keybinding.
        _keybindingEditControls.UpdateControl(
            _keybindingEditControl,
            int.Parse(_currentListeningButton.UserData["bindingIndex"]),
            modifier,
            key
        );

        // Check for duplicate keybindings.
        if (key != Keys.None)
        {
            foreach (var control in _keybindingEditControls.ControlMapping)
            {
                // Skip the control we're currently editing.
                if (control.Key == _keybindingEditControl)
                {
                    continue;
                }

                var bindings = control.Value.Bindings;
                for (var bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++)
                {
                    var binding = bindings[bindingIndex];

                    // Remove this mapping.
                    if (binding.Modifier == modifier && binding.Key == key)
                    {
                        _keybindingEditControls.UpdateControl(
                            control.Key,
                            bindingIndex,
                            Keys.None,
                            Keys.None
                        );
                    }
                }
            }

            // MYRA-TODO: Play sound.
            // _keybindingEditBtn.PlayHoverSound();
        }

        ResetKeybindingListener();
        UpdateKeybindingControlsLabels();
    }

    private void ResetKeybindingListener()
    {
        if (_currentListeningButton != default)
        {
            var defaultModifier = Enum.Parse<Keys>(_currentListeningButton.UserData["modifier"]);
            var defaultKey = Enum.Parse<Keys>(_currentListeningButton.UserData["key"]);
            _currentListeningButton.SetText(Strings.Keys.FormatKeyName(defaultModifier, defaultKey));
            _currentListeningButton = default;
        }

        _keysDown.Clear();
        _keybindingListeningTimer = 0;
    }

    #endregion

    #region Settings Actions

    private void SettingsApplyBtn_Clicked(object? sender, EventArgs _)
    {
        var shouldReset = false;

        // Game Settings.
        Globals.Database.HideOthersOnWindowOpen = _autoCloseWindowsCheckButton?.IsChecked ?? false;
        Globals.Database.AutoToggleChatLog = _autoToggleChatLogCheckButton?.IsChecked ?? false;
        Globals.Database.ShowExperienceAsPercentage = _showExperienceAsPercentageCheckButton?.IsChecked ?? false;
        Globals.Database.ShowHealthAsPercentage = _showHealthAsPercentageCheckButton?.IsChecked ?? false;
        Globals.Database.ShowManaAsPercentage = _showManaAsPercentageCheckButton?.IsChecked ?? false;
        Globals.Database.SimplifiedEscapeMenu = _simplifiedEscapeMenuCheckButton?.IsChecked ?? false;
        Globals.Database.FriendOverheadInfo = _friendOverheadInfoCheckbox?.IsChecked ?? false;
        Globals.Database.GuildMemberOverheadInfo = _guildOverheadInfoCheckButton?.IsChecked ?? false;
        Globals.Database.MyOverheadInfo = _myOverheadInfoCheckButton?.IsChecked ?? false;
        Globals.Database.NpcOverheadInfo = _npcOverheadInfoCheckButton?.IsChecked ?? false;
        Globals.Database.PartyMemberOverheadInfo = _partyOverheadInfoCheckButton?.IsChecked ?? false;
        Globals.Database.PlayerOverheadInfo = _playerOverheadInfoCheckButton?.IsChecked ?? false;
        Globals.Database.FriendOverheadHpBar = _friendOverheadHpBarCheckButton?.IsChecked ?? false;
        Globals.Database.GuildMemberOverheadHpBar = _guildOverheadHpBarCheckButton?.IsChecked ?? false;
        Globals.Database.MyOverheadHpBar = _myOverheadHpBarCheckButton?.IsChecked ?? false;
        Globals.Database.NpcOverheadHpBar = _npcOverheadHpBarCheckButton?.IsChecked ?? false;
        Globals.Database.PartyMemberOverheadHpBar = _partyOverheadHpBarCheckButton?.IsChecked ?? false;
        Globals.Database.PlayerOverheadHpBar = _playerOverheadHpBarCheckButton?.IsChecked ?? false;
        Globals.Database.StickyTarget = _stickyTargetCheckButton?.IsChecked ?? false;
        Globals.Database.AutoTurnToTarget = _autoTurnToTargetCheckButton?.IsChecked ?? false;
        Globals.Database.TypewriterBehavior = (_typewriterCheckButton?.IsChecked ?? false) ? TypewriterBehavior.Word : TypewriterBehavior.Off;

        // Video Settings.
        Globals.Database.WorldZoom = _worldScale?.Value ?? 1;
        Globals.Database.EnableLighting = _lightingEnabledCheckbox?.IsChecked ?? false;

        if (Globals.Database.FullScreen != _fullscreenCheckbox?.IsChecked)
        {
            Globals.Database.FullScreen = _fullscreenCheckbox?.IsChecked ?? false;
            shouldReset = true;
        }

        if (_resolutionList != default)
        {
            var resolution = _resolutionList.GetSelectedValue();
            var validVideoModes = Graphics.Renderer?.GetValidVideoModes();
            var targetResolution = validVideoModes?.FindIndex(videoMode => string.Equals(videoMode, resolution)) ?? -1;

            if (targetResolution > -1)
            {
                shouldReset = Globals.Database.TargetResolution != targetResolution ||
                              Graphics.Renderer?.HasOverrideResolution == true;
                Globals.Database.TargetResolution = targetResolution;
            }
        }

        if (_fpsList != default)
        {
            var newFps = 0;
            var currentFps = _fpsList.GetSelectedValue();

            if (currentFps == Strings.Settings.UnlimitedFps)
            {
                newFps = -1;
            }
            else if (currentFps == Strings.Settings.Fps30)
            {
                newFps = 1;
            }
            else if (currentFps == Strings.Settings.Fps60)
            {
                newFps = 2;
            }
            else if (currentFps == Strings.Settings.Fps90)
            {
                newFps = 3;
            }
            else if (currentFps == Strings.Settings.Fps120)
            {
                newFps = 4;
            }

            if (newFps != Globals.Database.TargetFps)
            {
                shouldReset = true;
                Globals.Database.TargetFps = newFps;
            }
        }


        // Audio Settings.
        _previousMusicVolume = Globals.Database.MusicVolume;
        _previousSoundVolume = Globals.Database.SoundVolume;
        Globals.Database.MusicVolume = (int)(_musicSlider?.Value ?? 0);
        Globals.Database.SoundVolume = (int)(_soundSlider?.Value ?? 0);
        Audio.UpdateGlobalVolume();

        // Control Settings.
        Controls.ActiveControls = _keybindingEditControls;
        Controls.ActiveControls.Save();

        // Save Preferences.
        Globals.Database.SavePreferences();

        if (shouldReset && Graphics.Renderer != default)
        {
            Graphics.Renderer.OverrideResolution = Resolution.Empty;
            Graphics.Renderer.Init();
        }

        // Hide the currently opened window.
        Hide();
    }

    private void SettingsCancelBtn_Clicked(object? sender, EventArgs _)
    {
        // Update previously saved values in order to discard changes.
        Globals.Database.MusicVolume = _previousMusicVolume;
        Globals.Database.SoundVolume = _previousSoundVolume;
        _musicSlider?.SetValue(_previousMusicVolume);
        _soundSlider?.SetValue(_previousSoundVolume);
        _musicLabel?.SetText(Strings.Settings.MusicVolume.ToString(_previousMusicVolume));
        _soundLabel?.SetText(Strings.Settings.SoundVolume.ToString(_previousSoundVolume));
        Audio.UpdateGlobalVolume();
        _keybindingEditControls = new Controls(Controls.ActiveControls);
        Hide();
    }

    #endregion

    #region Constants

    private const string TAB_CONTROL = nameof(TAB_CONTROL);
    private const string GAME_TAB = nameof(GAME_TAB);
    private const string VIDEO_TAB = nameof(VIDEO_TAB);
    private const string AUDIO_TAB = nameof(AUDIO_TAB);
    private const string KEYBINDING_TAB = nameof(KEYBINDING_TAB);
    private const string SAVE_BUTTON = nameof(SAVE_BUTTON);
    private const string CANCEL_BUTTON = nameof(CANCEL_BUTTON);

    private const string AUTO_CLOSE_WINDOW_CHECK = nameof(AUTO_CLOSE_WINDOW_CHECK);
    private const string AUTO_TOGGLE_CHAT_LOG_CHECK = nameof(AUTO_TOGGLE_CHAT_LOG_CHECK);
    private const string SHOW_HEALTH_AS_PERCENTAGE_CHECK = nameof(SHOW_HEALTH_AS_PERCENTAGE_CHECK);
    private const string SHOW_MANA_AS_PERCENTAGE_CHECK = nameof(SHOW_MANA_AS_PERCENTAGE_CHECK);
    private const string SHOW_EXPERIENCE_AS_PERCENTAGE_CHECK = nameof(SHOW_EXPERIENCE_AS_PERCENTAGE_CHECK);
    private const string TYPEWRITER_CHECK = nameof(TYPEWRITER_CHECK);
    private const string SIMPLIFIED_ESCAPE_MENU_CHECK = nameof(SIMPLIFIED_ESCAPE_MENU_CHECK);

    private const string GAME_OVERHEAD_INFO_LABEL = nameof(GAME_OVERHEAD_INFO_LABEL);
    private const string MY_OVERHEAD_INFO_CHECK = nameof(MY_OVERHEAD_INFO_CHECK);
    private const string PLAYER_OVERHEAD_INFO_CHECK = nameof(PLAYER_OVERHEAD_INFO_CHECK);
    private const string NPC_OVERHEAD_INFO_CHECK = nameof(NPC_OVERHEAD_INFO_CHECK);
    private const string FRIEND_OVERHEAD_INFO_CHECK = nameof(FRIEND_OVERHEAD_INFO_CHECK);
    private const string PARTY_OVERHEAD_INFO_CHECK = nameof(PARTY_OVERHEAD_INFO_CHECK);
    private const string GUILD_OVERHEAD_INFO_CHECK = nameof(GUILD_OVERHEAD_INFO_CHECK);

    private const string GAME_OVERHEAD_HP_BAR_LABEL = nameof(GAME_OVERHEAD_HP_BAR_LABEL);
    private const string MY_OVERHEAD_HP_BAR_CHECK = nameof(MY_OVERHEAD_HP_BAR_CHECK);
    private const string PLAYER_OVERHEAD_HP_BAR_CHECK = nameof(PLAYER_OVERHEAD_HP_BAR_CHECK);
    private const string NPC_OVERHEAD_HP_BAR_CHECK = nameof(NPC_OVERHEAD_HP_BAR_CHECK);
    private const string FRIEND_OVERHEAD_HP_BAR_CHECK = nameof(FRIEND_OVERHEAD_HP_BAR_CHECK);
    private const string PARTY_OVERHEAD_HP_BAR_CHECK = nameof(PARTY_OVERHEAD_HP_BAR_CHECK);
    private const string GUILD_OVERHEAD_HP_BAR_CHECK = nameof(GUILD_OVERHEAD_HP_BAR_CHECK);

    private const string TARGETING_LABEL = nameof(TARGETING_LABEL);
    private const string STICKY_TARGET_CHECK = nameof(STICKY_TARGET_CHECK);
    private const string AUTO_TURN_TO_TARGET_CHECK = nameof(AUTO_TURN_TO_TARGET_CHECK);

    private const string RESOLUTION_LABEL = nameof(RESOLUTION_LABEL);
    private const string RESOLUTION_COMBO_VIEW = nameof(RESOLUTION_COMBO_VIEW);
    private const string FPS_LABEL = nameof(FPS_LABEL);
    private const string FPS_COMBO_VIEW = nameof(FPS_COMBO_VIEW);
    private const string WORLD_SCALE_LABEL = nameof(WORLD_SCALE_LABEL);
    private const string WORLD_SCALE_SLIDER = nameof(WORLD_SCALE_SLIDER);
    private const string FULLSCREEN_CHECK = nameof(FULLSCREEN_CHECK);
    private const string LIGHTING_ENABLED_CHECK = nameof(LIGHTING_ENABLED_CHECK);

    private const string MUSIC_VOLUME_LABEL = nameof(MUSIC_VOLUME_LABEL);
    private const string MUSIC_VOLUME_SLIDER = nameof(MUSIC_VOLUME_SLIDER);
    private const string SOUND_VOLUME_LABEL = nameof(SOUND_VOLUME_LABEL);
    private const string SOUND_VOLUME_SLIDER = nameof(SOUND_VOLUME_SLIDER);

    private const string KEYBINDING_RESTORE_BUTTON = nameof(KEYBINDING_RESTORE_BUTTON);
    private const string KEYBINDING_CONTROLS_CONTAINER = nameof(KEYBINDING_CONTROLS_CONTAINER);
    private const string KEYBINDING_CONTROLS_ROW = nameof(KEYBINDING_CONTROLS_ROW);

    #endregion
}