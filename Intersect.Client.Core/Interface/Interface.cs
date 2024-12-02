using System.Reflection;
using Intersect.Client.Core;
using Intersect.Client.Framework.Graphics;
using Intersect.Client.General;
using Intersect.Client.Interface.Game;
using Intersect.Client.Interface.Menu;
using Intersect.Client.Interface.Shared;
using Myra;
using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface;

public static partial class Interface
{
    public static GameInterface? GameUi { get; private set; }

    public static MainMenu? MenuUi { get; private set; }

    public static List<Widget> FocusElements { get; set; } = [];

    public static List<Widget> InputBlockingElements { get; set; } = [];

    public static ErrorHandler ErrorMsgHandler = new();

    private static readonly Queue<KeyValuePair<string, string>> _errorMessages = new();

    private static Desktop Desktop { get; set; } = new Desktop();

    public static bool IsVisible
    {
        get => Desktop.Root.Visible;
        set => Desktop.Root.Visible = value;
    }

    public static bool IsMouseOverGUI => Desktop.IsMouseOverGUI;

    public static bool HideUi;

    #region "Interface Setup"
    public static void Initialization()
    {
        // Preserve the debug window
        MutableInterface.DetachDebugWindow();

        if (Globals.GameState is GameStates.Intro or GameStates.Menu)
        {
            MenuUi = new MainMenu();
            GameUi = null;
        }
        else
        {
            //MYRA-TODO: re-enable this
            //GameUi = new GameInterface();
            MenuUi = null;
        }

        Globals.OnLifecycleChangeState();
    }

    public static void DestroyInterface(bool exiting = false)
    {
        // Preserve the debug window
        MutableInterface.DetachDebugWindow();
        Dispose();

        //MYRA-TODO: Check if this is still needed
        if (Globals.Me != null)
        {
            _ = Globals.Me.ClearTarget();
            Globals.Me.TargetBox?.Dispose();
            Globals.Me.TargetBox = null;
        }

        if (exiting)
        {
            MutableInterface.DisposeDebugWindow();
        }
    }

    public static void SetInputFocus(Widget? widget)
    {
        Desktop.FocusedKeyboardWidget = widget;
    }

    public static bool HasInputFocus()
    {
        if (FocusElements == null || InputBlockingElements == null)
        {
            return false;
        }

        return FocusElements.Any(t => t.IsKeyboardFocused || InputBlockingElements.Any(t => t.IsKeyboardFocused));
    }

    public static void SetRoot(Widget root)
    {
        Desktop.Root = root;
    }

    public static void Render()
    {
        Desktop.Render();
    }

    public static Widget LoadContent(string filepath)
    {
        string data;
        string fullPath = Path.Combine(MyraEnvironment.Game.Content.RootDirectory, filepath);

        if (File.Exists(fullPath))
        {
            data = File.ReadAllText(fullPath);
        }
        else
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName =
                assembly.GetManifestResourceNames().FirstOrDefault(str => str.EndsWith(Path.GetFileName(filepath)))
                ?? throw new FileNotFoundException($"Resource {filepath} not found.");

            using Stream stream = assembly.GetManifestResourceStream(resourceName)!;
            using StreamReader reader = new StreamReader(stream);
            data = reader.ReadToEnd();
        }

        var project = Project.LoadFromXml(data);
        AddElement(project.Root);
        return project.Root;
    }
    #endregion

    #region "Functions"
    public static void Update()
    {
        if (Globals.GameState == GameStates.Menu)
        {
            MenuUi?.Update();
        }
        else if (Globals.GameState == GameStates.InGame)
        {
            GameUi?.Update();
        }

        ErrorMsgHandler.Update();

        //Do not allow hiding of UI under several conditions
        var forceShowUi = Globals.InCraft || Globals.InBank || Globals.InShop || Globals.InTrade || Globals.InBag || Globals.EventDialogs?.Count > 0 || HasInputFocus() || (!Interface.GameUi?.EscapeMenu?.IsHidden ?? true);

        if (HideUi && !forceShowUi && IsVisible)
        {
            IsVisible = false;
        }
        else
        {
            if (!IsVisible)
            {
                IsVisible = true;
            }
        }
    }

    public static T? GetChildById<T>(string id) where T : Widget
    {
        foreach (var widget in Desktop.Widgets)
        {
            var child = widget.FindChildById(id);
            if (child is T t && t.Id == id)
            {
                return t;
            }
        }

        return default;
    }

    public static bool GetChildById<T>(string id, out T widget) where T : Widget
    {
        widget = GetChildById<T>(id);
        return widget != null;
    }

    public static void AddElement(Widget widget)
    {
        Desktop.Widgets.Add(widget);
    }

    public static void Dispose()
    {
        Desktop.Dispose();
    }

    public static string[] WrapText(string input, int width, GameFont font)
    {
        var myOutput = new List<string>();
        if (input == null)
        {
            myOutput.Add("");
        }
        else
        {
            var lastSpace = 0;
            var curPos = 0;
            var curLen = 1;
            var lastOk = 0;
            input = input.Replace("\r\n", "\n");
            float measured;
            string line;
            while (curPos + curLen < input.Length)
            {
                line = input.Substring(curPos, curLen);
                measured = Graphics.Renderer?.MeasureText(line, font, 1).X ?? 0;
                if (measured < width)
                {
                    lastOk = lastSpace;
                    switch (input[curPos + curLen])
                    {
                        case ' ':
                        case '-':
                            lastSpace = curLen;

                            break;

                        case '\n':
                            myOutput.Add(input.Substring(curPos, curLen).Trim());
                            lastSpace = 0;
                            curPos = curPos + curLen + 1;
                            curLen = 1;

                            break;
                    }
                }
                else
                {
                    if (lastOk == 0)
                    {
                        lastOk = curLen - 1;
                    }

                    line = input.Substring(curPos, lastOk).Trim();
                    myOutput.Add(line);
                    curPos += lastOk;
                    lastOk = 0;
                    lastSpace = 0;
                    curLen = 1;
                }

                curLen++;
            }

            myOutput.Add(input[curPos..].Trim());
        }

        return [.. myOutput];
    }
    #endregion

    #region Error Handler
    public static bool TryDequeueErrorMessage(out KeyValuePair<string, string> message) => _errorMessages.TryDequeue(out message);

    public static void ShowError(string message, string? header = default)
    {
        _errorMessages.Enqueue(new KeyValuePair<string, string>(header ?? string.Empty, message));
    }
    #endregion
}
