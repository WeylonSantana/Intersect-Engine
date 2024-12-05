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

    // Settings Window.
    private Button? _gameSettingsTab;
    private Button? _videoSettingsTab;
    private Button? _audioSettingsTab;
    private Button? _keybindingSettingsTab;
    private Button? _settingsApplyBtn;
    private Button? _settingsCancelBtn;

    // Game Settings - Interface.
    private Panel? _settingsGamePanel;
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
    private Panel? _settingsVideoPanel;
    private ComboView? _resolutionList;
    private ComboView? _fpsList;
    private HorizontalSlider? _worldScale;
    private CheckButton? _fullscreenCheckbox;
    private CheckButton? _lightingEnabledCheckbox;

    // Audio Settings.
    private Panel? _settingsAudioPanel;
    private HorizontalSlider? _musicSlider;
    private Label? _musicLabel;
    private HorizontalSlider? _soundSlider;
    private Label? _soundLabel;

    // Keybinding Settings.
    private Panel? _settingsKeybindingPanel;
    private Button? _keybindingEditBtn;
    private Button? _keybindingRestoreBtn;
    private Controls _keybindingEditControls = default!;
    private HashSet<Keys> _keysDown = [];
    private Dictionary<Control, Button[]> mKeybindingBtns = [];
    private Control _keybindingEditControl;
    private long _keybindingListeningTimer;
    private int _keyEdit = -1;

    private bool _returnToMenu;

    public bool Visible
    {
        get => _settingsWindow?.Visible ?? false;
        set => _settingsWindow?.ToggleVisibility(value);
    }

    private int _previousMusicVolume;
    private int _previousSoundVolume;

    public void Load(MenuInterface menu)
    {
        _menuInterface = menu;
        _settingsWindow = Interface.LoadContent(Path.Combine("shared", "SettingsWindow.xmmp"));

        #region Basic Load

        _gameSettingsTab = Interface.GetChildById<Button>("settingsGameTab");
        if (_gameSettingsTab != default)
        {
            _gameSettingsTab.Click += (s, e) => SwitchToContainer(_settingsGamePanel, _gameSettingsTab);
            _gameSettingsTab.SetText(Strings.Settings.GameSettingsTab);
        }

        _videoSettingsTab = Interface.GetChildById<Button>("settingsVideoTab");
        if (_videoSettingsTab != default)
        {
            _videoSettingsTab.Click += (s, e) => SwitchToContainer(_settingsVideoPanel, _videoSettingsTab);
            _videoSettingsTab.SetText(Strings.Settings.VideoSettingsTab);
        }

        _audioSettingsTab = Interface.GetChildById<Button>("settingsAudioTab");
        if (_audioSettingsTab != default)
        {
            _audioSettingsTab.Click += (s, e) => SwitchToContainer(_settingsAudioPanel, _audioSettingsTab);
            _audioSettingsTab.SetText(Strings.Settings.AudioSettingsTab);
        }

        _keybindingSettingsTab = Interface.GetChildById<Button>("settingsKeybindingTab");
        if (_keybindingSettingsTab != default)
        {
            _keybindingSettingsTab.Click += (s, e) => SwitchToContainer(_settingsKeybindingPanel, _keybindingSettingsTab);
            _keybindingSettingsTab.SetText(Strings.Settings.KeyBindingSettingsTab);
        }

        _settingsApplyBtn = Interface.GetChildById<Button>("settingsSave");
        if (_settingsApplyBtn != default)
        {
            _settingsApplyBtn.Click += SettingsApplyBtn_Clicked;
            _settingsApplyBtn.SetText(Strings.Settings.Apply);
        }

        _settingsCancelBtn = Interface.GetChildById<Button>("settingsCancel");
        if (_settingsCancelBtn != default)
        {
            _settingsCancelBtn.Click += SettingsCancelBtn_Clicked;
            _settingsCancelBtn.SetText(Strings.Settings.Cancel);
        }

        #endregion

        #region Game Settings Load - Interface

        _settingsGamePanel = Interface.GetChildById<Panel>("settingsGame");

        _autoCloseWindowsCheckButton = Interface.GetChildById<CheckButton>("settingsAutoCloseWindows");
        _autoCloseWindowsCheckButton?.SetText(Strings.Settings.AutoCloseWindows);
        _autoCloseWindowsCheckButton?.SetValue(Globals.Database.HideOthersOnWindowOpen);

        _autoToggleChatLogCheckButton = Interface.GetChildById<CheckButton>("settingsAutoToggleChatLog");
        _autoToggleChatLogCheckButton?.SetValue(Globals.Database.AutoToggleChatLog);
        _autoToggleChatLogCheckButton?.SetText(Strings.Settings.AutoToggleChatLog);

        _showHealthAsPercentageCheckButton = Interface.GetChildById<CheckButton>("settingsShowHealthAsPercentage");
        _showHealthAsPercentageCheckButton?.SetText(Strings.Settings.ShowHealthAsPercentage);
        _showHealthAsPercentageCheckButton?.SetValue(Globals.Database.ShowHealthAsPercentage);

        _showManaAsPercentageCheckButton = Interface.GetChildById<CheckButton>("settingsShowManaAsPercentage");
        _showManaAsPercentageCheckButton?.SetText(Strings.Settings.ShowManaAsPercentage);
        _showManaAsPercentageCheckButton?.SetValue(Globals.Database.ShowManaAsPercentage);

        _showExperienceAsPercentageCheckButton = Interface.GetChildById<CheckButton>("settingsShowExperienceAsPercentage");
        _showExperienceAsPercentageCheckButton?.SetText(Strings.Settings.ShowExperienceAsPercentage);
        _showExperienceAsPercentageCheckButton?.SetValue(Globals.Database.ShowExperienceAsPercentage);

        _typewriterCheckButton = Interface.GetChildById<CheckButton>("settingsTypewriter");
        _typewriterCheckButton?.SetText(Strings.Settings.TypewriterText);
        _typewriterCheckButton?.SetValue(Globals.Database.TypewriterBehavior == TypewriterBehavior.Word);

        _simplifiedEscapeMenuCheckButton = Interface.GetChildById<CheckButton>("settingsSimplifiedEscapeMenu");
        _simplifiedEscapeMenuCheckButton?.SetText(Strings.Settings.SimplifiedEscapeMenu);
        _simplifiedEscapeMenuCheckButton?.SetValue(Globals.Database.SimplifiedEscapeMenu);

        #endregion

        #region Game Settings Load - Overhead Information

        Interface.GetChildById<Label>("settingsShowOverheadInfo")?.SetText(Strings.Settings.ShowOverheadInformationTitle);
        _myOverheadInfoCheckButton = Interface.GetChildById<CheckButton>("settingsMyOverheadInfo");
        _myOverheadInfoCheckButton?.SetText(Strings.Settings.ShowMyOverheadInformation);
        _myOverheadInfoCheckButton?.SetValue(Globals.Database.MyOverheadInfo);

        _playerOverheadInfoCheckButton = Interface.GetChildById<CheckButton>("settingsPlayerOverheadInfo");
        _playerOverheadInfoCheckButton?.SetText(Strings.Settings.ShowPlayerOverheadInformation);
        _playerOverheadInfoCheckButton?.SetValue(Globals.Database.PlayerOverheadInfo);

        _npcOverheadInfoCheckButton = Interface.GetChildById<CheckButton>("settingsNpcOverheadInfo");
        _npcOverheadInfoCheckButton?.SetText(Strings.Settings.ShowNpcOverheadInformation);
        _npcOverheadInfoCheckButton?.SetValue(Globals.Database.NpcOverheadInfo);

        _friendOverheadInfoCheckbox = Interface.GetChildById<CheckButton>("settingsFriendOverheadInfo");
        _friendOverheadInfoCheckbox?.SetValue(Globals.Database.FriendOverheadInfo);
        _friendOverheadInfoCheckbox?.SetText(Strings.Settings.ShowFriendOverheadInformation);

        _partyOverheadInfoCheckButton = Interface.GetChildById<CheckButton>("settingsPartyOverheadInfo");
        _partyOverheadInfoCheckButton?.SetText(Strings.Settings.ShowPartyOverheadInformation);
        _partyOverheadInfoCheckButton?.SetValue(Globals.Database.PartyMemberOverheadInfo);

        _guildOverheadInfoCheckButton = Interface.GetChildById<CheckButton>("settingsGuildOverheadInfo");
        _guildOverheadInfoCheckButton?.SetText(Strings.Settings.ShowGuildOverheadInformation);
        _guildOverheadInfoCheckButton?.SetValue(Globals.Database.GuildMemberOverheadInfo);

        #endregion

        #region Game Settings Load - Overhead HP Bar

        Interface.GetChildById<Label>("settingsShowOverheadHPBar")?.SetText(Strings.Settings.ShowOverheadHPBarTitle);
        _myOverheadHpBarCheckButton = Interface.GetChildById<CheckButton>("settingsMyOverheadHPBar");
        _myOverheadHpBarCheckButton?.SetText(Strings.Settings.ShowMyOverheadHPBar);
        _myOverheadHpBarCheckButton?.SetValue(Globals.Database.MyOverheadHpBar);

        _playerOverheadHpBarCheckButton = Interface.GetChildById<CheckButton>("settingsPlayerOverheadHPBar");
        _playerOverheadHpBarCheckButton?.SetText(Strings.Settings.ShowPlayerOverheadHPBar);
        _playerOverheadHpBarCheckButton?.SetValue(Globals.Database.PlayerOverheadHpBar);

        _npcOverheadHpBarCheckButton = Interface.GetChildById<CheckButton>("settingsNpcOverheadHPBar");
        _npcOverheadHpBarCheckButton?.SetText(Strings.Settings.ShowNpcOverheadHPBar);
        _npcOverheadHpBarCheckButton?.SetValue(Globals.Database.NpcOverheadHpBar);

        _friendOverheadHpBarCheckButton = Interface.GetChildById<CheckButton>("settingsFriendOverheadHPBar");
        _friendOverheadHpBarCheckButton?.SetText(Strings.Settings.ShowFriendOverheadHPBar);
        _friendOverheadHpBarCheckButton?.SetValue(Globals.Database.FriendOverheadHpBar);

        _partyOverheadHpBarCheckButton = Interface.GetChildById<CheckButton>("settingsPartyOverheadHPBar");
        _partyOverheadHpBarCheckButton?.SetText(Strings.Settings.ShowPartyOverheadHPBar);
        _partyOverheadHpBarCheckButton?.SetValue(Globals.Database.PartyMemberOverheadHpBar);

        _guildOverheadHpBarCheckButton = Interface.GetChildById<CheckButton>("settingsGuildOverheadHPBar");
        _guildOverheadHpBarCheckButton?.SetText(Strings.Settings.ShowGuildOverheadHPBar);
        _guildOverheadHpBarCheckButton?.SetValue(Globals.Database.GuildMemberOverheadHpBar);

        #endregion

        #region Game Settings Load - Targeting

        Interface.GetChildById<Label>("settingsTargeting")?.SetText(Strings.Settings.TargettingTitle);
        _stickyTargetCheckButton = Interface.GetChildById<CheckButton>("settingsStickyTarget");
        _stickyTargetCheckButton?.SetText(Strings.Settings.StickyTarget);
        _stickyTargetCheckButton?.SetValue(Globals.Database.StickyTarget);

        _autoTurnToTargetCheckButton = Interface.GetChildById<CheckButton>("settingsAutoTurnToTarget");
        _autoTurnToTargetCheckButton?.SetText(Strings.Settings.AutoTurnToTarget);
        _autoTurnToTargetCheckButton?.SetValue(Globals.Database.AutoTurnToTarget);

        #endregion

        #region Video Settings Load

        _settingsVideoPanel = Interface.GetChildById<Panel>("settingsVideo");
        Interface.GetChildById<Label>("settingsResolutionLabel")?.SetText(Strings.Settings.Resolution);
        _resolutionList = Interface.GetChildById<ComboView>("settingsResolution");
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

        Interface.GetChildById<Label>("settingsFpsLabel")?.SetText(Strings.Settings.TargetFps);
        _fpsList = Interface.GetChildById<ComboView>("settingsFps");


        if (_fpsList != default)
        {
            var validFps = new Dictionary<int, string>
            {
                {
                    0, Strings.Settings.Vsync
                },
                {
                    1, Strings.Settings.Fps30
                },
                {
                    2, Strings.Settings.Fps60
                },
                {
                    3, Strings.Settings.Fps90
                },
                {
                    4, Strings.Settings.Fps120
                },
                {
                    5, Strings.Settings.UnlimitedFps
                },
            };

            // Add valid fps to the fps list.
            foreach (var fps in validFps)
            {
                _fpsList.AddItem(fps.Value, fps.Key.ToString());
            }

            // Select the current fps.
            _fpsList.SelectByKey(Globals.Database.TargetFps.ToString());
        }

        Interface.GetChildById<Label>("settingsWorldScaleLabel")?.SetText(Strings.Settings.WorldScale);
        _worldScale = Interface.GetChildById<HorizontalSlider>("settingsWorldScale");
        _worldScale?.SetValue(Globals.Database.WorldZoom);

        _fullscreenCheckbox = Interface.GetChildById<CheckButton>("settingsFullscreen");
        _fullscreenCheckbox?.SetText(Strings.Settings.Fullscreen);
        _fullscreenCheckbox?.SetValue(Globals.Database.FullScreen);

        _lightingEnabledCheckbox = Interface.GetChildById<CheckButton>("settingsLightingEnabled");
        _lightingEnabledCheckbox?.SetText(Strings.Settings.EnableLighting);
        _lightingEnabledCheckbox?.SetValue(Globals.Database.EnableLighting);

        #endregion

        #region Audio Settings Load

        _settingsAudioPanel = Interface.GetChildById<Panel>("settingsAudio");
        _previousMusicVolume = Globals.Database.MusicVolume;
        _previousSoundVolume = Globals.Database.SoundVolume;

        _musicLabel = Interface.GetChildById<Label>("settingsMusicVolumeLabel");
        _musicLabel?.SetText(Strings.Settings.MusicVolume);

        _musicSlider = Interface.GetChildById<HorizontalSlider>("settingsMusicVolume");
        if (_musicSlider != default)
        {
            _musicSlider.ValueChangedByUser += MusicSlider_ValueChanged;
            _musicSlider.SetValue(Globals.Database.MusicVolume);
        }

        _soundLabel = Interface.GetChildById<Label>("settingsSoundVolumeLabel");
        _soundLabel?.SetText(Strings.Settings.SoundVolume);

        _soundSlider = Interface.GetChildById<HorizontalSlider>("settingsSoundVolume");
        if (_soundSlider != default)
        {
            _soundSlider.ValueChangedByUser += SoundSlider_ValueChanged;
            _soundSlider.SetValue(Globals.Database.SoundVolume);
        }

        var musicLabelText = Strings.Settings.MusicVolume.ToString((int)(_musicSlider?.Value ?? 0));
        var soundLabelText = Strings.Settings.SoundVolume.ToString((int)(_soundSlider?.Value ?? 0));
        Interface.GetChildById<Label>("settingsMusicVolumeLabel")?.SetText(musicLabelText);
        Interface.GetChildById<Label>("settingsSoundVolumeLabel")?.SetText(soundLabelText);

        #endregion

        #region Keybinding Settings Load

        _settingsKeybindingPanel = Interface.GetChildById<Panel>("settingsKeybinding");
        _keybindingRestoreBtn = Interface.GetChildById<Button>("settingsRestoreDefaults");
        _keybindingRestoreBtn?.SetText(Strings.Settings.Restore);

        #endregion

        #region Actions

        _settingsApplyBtn = Interface.GetChildById<Button>("settingsApply");
        if (_settingsApplyBtn != default)
        {
            _settingsApplyBtn.Click += SettingsApplyBtn_Clicked;
            _settingsApplyBtn.SetText(Strings.Settings.Apply);
        }

        _settingsCancelBtn = Interface.GetChildById<Button>("settingsCancel");
        if (_settingsCancelBtn != default)
        {
            _settingsCancelBtn.Click += SettingsCancelBtn_Clicked;
            _settingsCancelBtn.SetText(Strings.Settings.Cancel);
        }

        _keybindingRestoreBtn = Interface.GetChildById<Button>("keybindingRestore");
        if (_keybindingRestoreBtn != default)
        {
            _keybindingRestoreBtn.Click += KeybindingsRestoreBtn_Clicked;
            _keybindingRestoreBtn.SetText(Strings.Settings.Restore);
        }

        #endregion
    }

    #region Settings Setup

    private void SwitchToContainer(Panel? container, Button? tab)
    {
        if (container == default || tab == default)
        {
            return;
        }

        _settingsGamePanel?.ToggleVisibility(false);
        _settingsVideoPanel?.ToggleVisibility(false);
        _settingsAudioPanel?.ToggleVisibility(false);
        _settingsKeybindingPanel?.ToggleVisibility(false);

        _gameSettingsTab?.ToggleEnabled(true);
        _videoSettingsTab?.ToggleEnabled(true);
        _audioSettingsTab?.ToggleEnabled(true);
        _keybindingSettingsTab?.ToggleEnabled(true);

        _keybindingRestoreBtn?.ToggleVisibility(container.Id == _settingsKeybindingPanel?.Id);

        container.Visible = true;
        tab?.ToggleEnabled(false);
    }

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
        // Containers.
        _settingsGamePanel.ToggleVisibility(true);
        _settingsVideoPanel.ToggleVisibility(false);
        _settingsAudioPanel.ToggleVisibility(false);
        _settingsKeybindingPanel.ToggleVisibility(false);

        // Disable the GameSettingsTab to fake it being selected visually by default.
        _gameSettingsTab.ToggleEnabled(false);
        _videoSettingsTab.ToggleEnabled(true);
        _audioSettingsTab.ToggleEnabled(true);
        _keybindingSettingsTab.ToggleEnabled(true);

        // Buttons.
        _settingsApplyBtn.ToggleVisibility(true);
        _settingsCancelBtn.ToggleVisibility(true);
        _keybindingRestoreBtn.ToggleVisibility(false);

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

    #endregion

    public void Update()
    {
        if (Visible &&
            _keybindingEditBtn != null &&
            _keybindingListeningTimer < Timing.Global.MillisecondsUtc)
        {
            OnKeyUp(Keys.None, Keys.None);
        }
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

    #endregion

    private void OnKeyDown(Keys modifier, Keys key)
    {
        if (_keybindingEditBtn != default)
        {
            _ = _keysDown.Add(key);
        }
    }

    private void OnKeyUp(Keys modifier, Keys key)
    {
        if (_keybindingEditBtn == null)
        {
            return;
        }

        if (key != Keys.None && !_keysDown.Remove(key))
        {
            return;
        }

        _keybindingEditControls.UpdateControl(
            _keybindingEditControl,
            _keyEdit,
            modifier,
            key
        );
        _keybindingEditBtn.SetText(Strings.Keys.FormatKeyName(modifier, key));

        if (key != Keys.None)
        {
            foreach (var control in _keybindingEditControls.ControlMapping)
            {
                if (control.Key == _keybindingEditControl)
                {
                    continue;
                }

                var bindings = control.Value.Bindings;
                for (var bindingIndex = 0; bindingIndex < bindings.Count; bindingIndex++)
                {
                    var binding = bindings[bindingIndex];

                    if (binding.Modifier != modifier || binding.Key != key)
                    {
                        continue;
                    }

                    // Remove this mapping.
                    _keybindingEditControls.UpdateControl(
                        control.Key,
                        bindingIndex,
                        Keys.None,
                        Keys.None
                    );

                    // Update UI.
                    var text = Strings.Keys.KeyDictionary[Enum.GetName(typeof(Keys), Keys.None)];
                    mKeybindingBtns[control.Key][bindingIndex].SetText(text);
                }
            }

            // MYRA-TODO: Play sound.
            // _keybindingEditBtn.PlayHoverSound();
        }

        _keybindingEditBtn = null;
        _keysDown.Clear();
    }

    private void Key_Clicked(object? sender, EventArgs _)
    {
        EditKeyPressed((Button)sender);
    }

    private void EditKeyPressed(Button sender)
    {
        if (_keybindingEditBtn != null)
        {
            return;
        }

        // sender.Text = Strings.Controls.Listening;
        // _keyEdit = ((KeyValuePair<Control, int>)sender.UserData).Value;
        // _keybindingEditControl = ((KeyValuePair<Control, int>)sender.UserData).Key;
        // _keybindingEditBtn = sender;
        // //Interface.GwenInput.HandleInput = false;
        // _keybindingListeningTimer = Timing.Global.MillisecondsUtc + 3000;
    }

    private void KeybindingsRestoreBtn_Clicked(object? sender, EventArgs _)
    {
        var t = "";
        // _keybindingEditControls.ResetDefaults();
        // foreach (Control control in Enum.GetValues(typeof(Control)))
        // {
        //     var controlMapping = _keybindingEditControls.ControlMapping[control];
        //     for (var bindingIndex = 0; bindingIndex < controlMapping.Bindings.Count; bindingIndex++)
        //     {
        //         var binding = controlMapping.Bindings[bindingIndex];
        //         mKeybindingBtns[control][bindingIndex].Text = Strings.Keys.FormatKeyName(binding.Modifier, binding.Key);
        //     }
        // }
    }

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
        Audio.UpdateGlobalVolume();
        _keybindingEditControls = new Controls(Controls.ActiveControls);

        // Hide our current window.
        Hide();
    }
}