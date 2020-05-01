/* Generated by MyraPad at 27.02.2020 21:05:55 */
using Myra.Graphics2D;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using Myra.Graphics2D.Brushes;

#if !STRIDE
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#else
using Stride.Core.Mathematics;
#endif

namespace Myra.Graphics2D.UI
{
	partial class DebugOptionsWindow: Window
	{
		private void BuildUI()
		{
			_checkBoxWidgetFrames = new CheckBox();
			_checkBoxWidgetFrames.Text = "Draw green frame around every widget";
			_checkBoxWidgetFrames.Id = "_checkBoxWidgetFrames";

			_checkBoxKeyboardFocusedWidgetFrame = new CheckBox();
			_checkBoxKeyboardFocusedWidgetFrame.Text = "Draw red frame around the keyboard focused widget";
			_checkBoxKeyboardFocusedWidgetFrame.Id = "_checkBoxKeyboardFocusedWidgetFrame";

			_checkBoxMouseWheelFocusedWidgetFrame = new CheckBox();
			_checkBoxMouseWheelFocusedWidgetFrame.Text = "Draw yellow frame around the mouse wheel focused widget";
			_checkBoxMouseWheelFocusedWidgetFrame.Id = "_checkBoxMouseWheelFocusedWidgetFrame";

			_checkBoxGlyphFrames = new CheckBox();
			_checkBoxGlyphFrames.Text = "Draw white frame around every TextBox letter glyph";
			_checkBoxGlyphFrames.Id = "_checkBoxGlyphFrames";

			_checkBoxDisableClipping = new CheckBox();
			_checkBoxDisableClipping.Text = "Disable clipping";
			_checkBoxDisableClipping.Id = "_checkBoxDisableClipping";

			Root = new VerticalStackPanel();
			Root.Id = "Root";
			Root.Widgets.Add(_checkBoxWidgetFrames);
			Root.Widgets.Add(_checkBoxKeyboardFocusedWidgetFrame);
			Root.Widgets.Add(_checkBoxMouseWheelFocusedWidgetFrame);
			Root.Widgets.Add(_checkBoxGlyphFrames);
			Root.Widgets.Add(_checkBoxDisableClipping);

			
			Title = "Debug Options";
			Left = 263;
			Top = 151;
			Content = Root;
		}

		
		public CheckBox _checkBoxWidgetFrames;
		public CheckBox _checkBoxKeyboardFocusedWidgetFrame;
		public CheckBox _checkBoxMouseWheelFocusedWidgetFrame;
		public CheckBox _checkBoxGlyphFrames;
		public CheckBox _checkBoxDisableClipping;
		public VerticalStackPanel Root;
	}
}