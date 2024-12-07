using Myra.Graphics2D.UI;

namespace Intersect.Client.Interface.Editor;

public partial class EditorPanel : IEditor
{
    private EditorInterface _editorInterface = null!;
    private Widget? _editorPanel;
    //private MenuItem? _menuFileQuit;
    public bool Visible => _editorPanel?.Visible ?? false;

    public void Load(EditorInterface editorInterface)
    {
        _editorInterface = editorInterface;
        _editorPanel = Interface.LoadContent(Path.Combine("editor", "EditorPanel.xmmp"));
        
        // Quit
        /*
        _menuFileQuit.Selected += (sender, args) =>
        {
            Main.Logout(false, skipFade: true);
            _editorInterface.Reset();
            _ = new MenuInterface();
        };
        */
    }

    public void Show()
    {
        if (_editorPanel == default)
        {
            return;
        }

        _editorPanel.Visible = true;
    }

    public void Hide()
    {
        if (_editorPanel == default)
        {
            return;
        }

        _editorPanel.Visible = false;
    }

    public void Update()
    {
    }
}