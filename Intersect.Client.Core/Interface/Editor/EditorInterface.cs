namespace Intersect.Client.Interface.Editor;

public partial class EditorInterface
{
    private readonly EditorPanel _editorPanel;

    public EditorInterface()
    {
        _editorPanel = new EditorPanel();
        _editorPanel.Load(this);
        SwitchToWindow<EditorPanel>();
    }

    //Methods
    public void Update()
    {
        if (_editorPanel.Visible)
        {
            _editorPanel.Update();
        }
    }

    public void Reset()
    {
        _editorPanel.Hide();
    }
    
    internal void SwitchToWindow<TEditor>() where TEditor : IEditor
    {
        Reset();
        if (typeof(TEditor) == typeof(EditorPanel))
        {
            _editorPanel.Show();
        }
    }
}