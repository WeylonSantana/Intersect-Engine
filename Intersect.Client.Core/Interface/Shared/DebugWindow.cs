using Intersect.Client.Core;
using Intersect.Client.General;
using Intersect.Client.Localization;
using Intersect.Client.Interface.Extensions;
using Intersect.Client.Interface.Menu;
using Intersect.Client.Maps;
using Myra;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Shared;

public sealed partial class DebugWindow : IWindow
{
    private Widget? _debugWindow;
    private Grid? _tableDebugStats;

    public DebugWindow()
    {
        Load();
        Hide();
    }

    public void Load()
    {
        _debugWindow = Interface.LoadContent(Path.Combine("shared", "DebugWindow.xmmp"));

        if (_debugWindow.FindChildById<CheckButton>(DRAW_WIDGET_FRAMES_CHECKBUTTON, out var drawWidgetFramesCheckButton))
        {
            drawWidgetFramesCheckButton.SetText(Strings.Debug.DrawUiBorders);
            drawWidgetFramesCheckButton.IsCheckedChanged += (s, a) =>
            {
                MyraEnvironment.DrawWidgetsFrames = !MyraEnvironment.DrawWidgetsFrames;
            };
        }
        
        if (_debugWindow.FindChildById<CheckButton>(DRAW_MOUSE_HOVERED_WIDGET_FRAME_CHECKBUTTON, out var drawMouseHoveredWidgetFrameCheckButton))
        {
            drawMouseHoveredWidgetFrameCheckButton.SetText(Strings.Debug.DrawUiHoveredBorders);
            drawMouseHoveredWidgetFrameCheckButton.IsCheckedChanged += (s, a) =>
            {
                MyraEnvironment.DrawMouseHoveredWidgetFrame = !MyraEnvironment.DrawMouseHoveredWidgetFrame;
            };
        }

        if (_debugWindow.FindChildById<CheckButton>(DRAW_TEXT_GLYPHS_FRAMES_CHECKBUTTON, out var drawTextGlyphsFramesCheckButton))
        {
            drawTextGlyphsFramesCheckButton.SetText(Strings.Debug.DrawUiTextBorders);
            drawTextGlyphsFramesCheckButton.IsCheckedChanged += (s, a) =>
            {
                MyraEnvironment.DrawTextGlyphsFrames = !MyraEnvironment.DrawTextGlyphsFrames;
            };
        }

        if (_debugWindow.FindChildById<CheckButton>(HOT_RELOAD_CHECKBUTTON, out var hotReloadCheckButton))
        {
            hotReloadCheckButton.SetText(Strings.Debug.EnableLayoutHotReloading);
            hotReloadCheckButton.IsCheckedChanged += (s, a) =>
            {
                Globals.ContentManager.ContentWatcher.Enabled = hotReloadCheckButton.IsChecked;
                //MyraEnvironment.Reset(); TODO: Implement this - it's not a thing in Myra, or does reset work as we want?
            };
        }

        _tableDebugStats = (Grid)_debugWindow.FindChildById(TABLE_DEBUG_STATS);
        InitializeDebugStatsTable();
    }

    private void InitializeDebugStatsTable()
    {
        if (_tableDebugStats == null)
        {
            return;
        }

        _tableDebugStats.FindChildById<Label>(FPS_LABEL).SetText(Strings.Debug.Fps);
        _tableDebugStats.FindChildById<Label>(PING_LABEL).SetText(Strings.Debug.Ping);
        _tableDebugStats.FindChildById<Label>(DRAW_CALLS_LABEL).SetText(Strings.Debug.Draws);
        _tableDebugStats.FindChildById<Label>(MAP_LABEL).SetText(Strings.Debug.Map);
        _tableDebugStats.FindChildById<Label>(COORDINATE_X_LABEL).SetText("X");
        _tableDebugStats.FindChildById<Label>(COORDINATE_Y_LABEL).SetText("Y");
        _tableDebugStats.FindChildById<Label>(COORDINATE_Z_LABEL).SetText("Z");
        _tableDebugStats.FindChildById<Label>(KNOWN_ENTITIES_LABEL).SetText(Strings.Debug.KnownEntities);
        _tableDebugStats.FindChildById<Label>(KNOWN_MAPS_LABEL).SetText(Strings.Debug.KnownMaps);
        _tableDebugStats.FindChildById<Label>(MAPS_DRAWN_LABEL).SetText(Strings.Debug.MapsDrawn);
        _tableDebugStats.FindChildById<Label>(ENTITIES_DRAWN_LABEL).SetText(Strings.Debug.EntitiesDrawn);
        _tableDebugStats.FindChildById<Label>(LIGHTS_DRAWN_LABEL).SetText(Strings.Debug.LightsDrawn);
        _tableDebugStats.FindChildById<Label>(TIME_LABEL).SetText(Strings.Debug.Time);
    }

    public void Update()
    {
        if (_tableDebugStats == null)
        {
            return;
        }

        var mapId = Globals.Me?.MapId ?? default;

        _tableDebugStats.FindChildById<Label>(FPS_VALUE_LABEL).SetText(Graphics.Renderer?.GetFps().ToString() ?? "0");
        _tableDebugStats.FindChildById<Label>(PING_VALUE_LABEL).SetText(Networking.Network.Ping.ToString());
        _tableDebugStats.FindChildById<Label>(DRAW_CALLS_VALUE_LABEL).SetText(Graphics.DrawCalls.ToString());
        _tableDebugStats.FindChildById<Label>(MAP_VALUE_LABEL).SetText(mapId == default ? "N/A" : MapInstance.Get(mapId).Name ?? "N/A");
        _tableDebugStats.FindChildById<Label>(COORDINATE_X_VALUE_LABEL).SetText(Globals.Me?.X.ToString() ?? "-1");
        _tableDebugStats.FindChildById<Label>(COORDINATE_Y_VALUE_LABEL).SetText(Globals.Me?.Y.ToString() ?? "-1");
        _tableDebugStats.FindChildById<Label>(COORDINATE_Z_VALUE_LABEL).SetText(Globals.Me?.Z.ToString() ?? "-1");
        _tableDebugStats.FindChildById<Label>(KNOWN_ENTITIES_VALUE_LABEL).SetText(Graphics.DrawCalls.ToString());
        _tableDebugStats.FindChildById<Label>(KNOWN_MAPS_VALUE_LABEL).SetText(MapInstance.Lookup.Count.ToString());
        _tableDebugStats.FindChildById<Label>(MAPS_DRAWN_VALUE_LABEL).SetText(Graphics.MapsDrawn.ToString());
        _tableDebugStats.FindChildById<Label>(ENTITIES_DRAWN_VALUE_LABEL).SetText(Graphics.EntitiesDrawn.ToString());
        _tableDebugStats.FindChildById<Label>(LIGHTS_DRAWN_VALUE_LABEL).SetText(Graphics.LightsDrawn.ToString());
        _tableDebugStats.FindChildById<Label>(TIME_VALUE_LABEL).SetText(Time.ServerTime);
    }

    public bool Visible
    {
        get => _debugWindow?.Visible ?? false;
        set => _debugWindow?.SetVisible(value);
    }

    public void Show()
    {
        Visible = true;
    }

    public void Hide()
    {
        Visible = false;
    }

    public void Toggle()
    {
        if (Visible)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    #region Constants

    private const string DRAW_WIDGET_FRAMES_CHECKBUTTON = nameof(DRAW_WIDGET_FRAMES_CHECKBUTTON);
    private const string DRAW_MOUSE_HOVERED_WIDGET_FRAME_CHECKBUTTON = nameof(DRAW_MOUSE_HOVERED_WIDGET_FRAME_CHECKBUTTON);
    private const string DRAW_TEXT_GLYPHS_FRAMES_CHECKBUTTON = nameof(DRAW_TEXT_GLYPHS_FRAMES_CHECKBUTTON);
    private const string HOT_RELOAD_CHECKBUTTON = nameof(HOT_RELOAD_CHECKBUTTON);

    private const string TABLE_DEBUG_STATS = nameof(TABLE_DEBUG_STATS);

    private const string FPS_LABEL = nameof(FPS_LABEL);
    private const string FPS_VALUE_LABEL = nameof(FPS_VALUE_LABEL);

    private const string PING_LABEL = nameof(PING_LABEL);
    private const string PING_VALUE_LABEL = nameof(PING_VALUE_LABEL);

    private const string DRAW_CALLS_LABEL = nameof(DRAW_CALLS_LABEL);
    private const string DRAW_CALLS_VALUE_LABEL = nameof(DRAW_CALLS_VALUE_LABEL);

    private const string MAP_LABEL = nameof(MAP_LABEL);
    private const string MAP_VALUE_LABEL = nameof(MAP_VALUE_LABEL);

    private const string COORDINATE_X_LABEL = nameof(COORDINATE_X_LABEL);
    private const string COORDINATE_X_VALUE_LABEL = nameof(COORDINATE_X_VALUE_LABEL);

    private const string COORDINATE_Y_LABEL = nameof(COORDINATE_Y_LABEL);
    private const string COORDINATE_Y_VALUE_LABEL = nameof(COORDINATE_Y_VALUE_LABEL);

    private const string COORDINATE_Z_LABEL = nameof(COORDINATE_Z_LABEL);
    private const string COORDINATE_Z_VALUE_LABEL = nameof(COORDINATE_Z_VALUE_LABEL);

    private const string KNOWN_ENTITIES_LABEL = nameof(KNOWN_ENTITIES_LABEL);
    private const string KNOWN_ENTITIES_VALUE_LABEL = nameof(KNOWN_ENTITIES_VALUE_LABEL);

    private const string KNOWN_MAPS_LABEL = nameof(KNOWN_MAPS_LABEL);
    private const string KNOWN_MAPS_VALUE_LABEL = nameof(KNOWN_MAPS_VALUE_LABEL);

    private const string MAPS_DRAWN_LABEL = nameof(MAPS_DRAWN_LABEL);
    private const string MAPS_DRAWN_VALUE_LABEL = nameof(MAPS_DRAWN_VALUE_LABEL);

    private const string ENTITIES_DRAWN_LABEL = nameof(ENTITIES_DRAWN_LABEL);
    private const string ENTITIES_DRAWN_VALUE_LABEL = nameof(ENTITIES_DRAWN_VALUE_LABEL);

    private const string LIGHTS_DRAWN_LABEL = nameof(LIGHTS_DRAWN_LABEL);
    private const string LIGHTS_DRAWN_VALUE_LABEL = nameof(LIGHTS_DRAWN_VALUE_LABEL);

    private const string TIME_LABEL = nameof(TIME_LABEL);
    private const string TIME_VALUE_LABEL = nameof(TIME_VALUE_LABEL);

    #endregion
}