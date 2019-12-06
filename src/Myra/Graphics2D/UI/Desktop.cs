﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Myra.Graphics2D.UI.Styles;
using Myra.Utility;

#if !XENKO
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
#else
using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Xenko.Input;
#endif

namespace Myra.Graphics2D.UI
{
	public struct MouseInfo
	{
		public Point Position;
		public bool IsLeftButtonDown;
		public bool IsMiddleButtonDown;
		public bool IsRightButtonDown;
		public float Wheel;
	}

	public static class Desktop
	{
		public const int DoubleClickIntervalInMs = 500;

		public static Func<Rectangle> DefaultBoundsFetcher = () =>
		{
			var device = MyraEnvironment.GraphicsDevice;
#if !XENKO
			return new Rectangle(0, 0,
				device.PresentationParameters.BackBufferWidth,
				device.PresentationParameters.BackBufferHeight);
#else
			return new Rectangle(0, 0,
				device.Presenter.BackBuffer.ViewWidth, 
				device.Presenter.BackBuffer.ViewHeight);
#endif
		};

		private static RenderContext _renderContext;

		private static bool _layoutDirty = true;
		private static Rectangle _bounds;
		private static bool _widgetsDirty = true;
		private static Widget _focusedKeyboardWidget;
		private static readonly List<Widget> _widgetsCopy = new List<Widget>();
		private static readonly ObservableCollection<Widget> _widgets = new ObservableCollection<Widget>();
		private static DateTime _lastTouchDown;
		private static DateTime? _lastKeyDown;
		private static int _keyDownCount = 0;
		private static MouseInfo _lastMouseInfo;
		private static IReadOnlyCollection<Keys> _downKeys, _lastDownKeys;
		private static Widget _previousKeyboardFocus;
		private static Widget _previousMouseWheelFocus;
#if !XENKO
		private static TouchCollection _oldTouchState;
#endif
		private static Widget _scheduleMouseWheelFocus;
		private static bool _isTouchDown;
		private static Point _mousePosition, _touchPosition;
		private static Point _lastMousePosition, _lastTouchPosition;
#if MONOGAME
		public static bool HasExternalTextInput = false;
#endif

		public static IReadOnlyCollection<Keys> DownKeys
		{
			get
			{
				return _downKeys;
			}
		}

		internal static Point LastMousePosition
		{
			get
			{
				return _lastMousePosition;
			}
		}

		public static Point MousePosition
		{
			get
			{
				return _mousePosition;
			}

			private set
			{
				if (value == _mousePosition)
				{
					return;
				}

				_lastMousePosition = _mousePosition;
				_mousePosition = value;
				MouseMoved.Invoke();

				for (var i = ChildrenCopy.Count - 1; i >= 0; --i)
				{
					var w = ChildrenCopy[i];
					if (w.Visible && w.Enabled)
					{
						if (w.HandleMouseMovement() || w.IsModal)
						{
							break;
						}
					}
				}

				if (IsTouchDown)
				{
					TouchPosition = MousePosition;
				}
			}
		}

		internal static Point LastTouchPosition
		{
			get
			{
				return _lastTouchPosition;
			}
		}

		public static Point TouchPosition
		{
			get
			{
				return _touchPosition;
			}

			private set
			{
				if (value == _touchPosition)
				{
					return;
				}

				_lastTouchPosition = _touchPosition;
				_touchPosition = value;
				TouchMoved.Invoke();

				for (var i = ChildrenCopy.Count - 1; i >= 0; --i)
				{
					var w = ChildrenCopy[i];
					if (w.Visible && w.Enabled)
					{
						if (w.HandleTouchMovement() || w.IsModal)
						{
							break;
						}
					}
				}
			}
		}

		public static HorizontalMenu MenuBar { get; set; }

		public static Func<MouseInfo> MouseInfoGetter
		{
			get; set;
		}

		public static Func<IReadOnlyCollection<Keys>> DownKeysGetter
		{
			get; set;
		}

		internal static List<Widget> ChildrenCopy
		{
			get
			{
				UpdateWidgetsCopy();
				return _widgetsCopy;
			}
		}

		public static ObservableCollection<Widget> Widgets
		{
			get { return _widgets; }
		}

		public static Func<Rectangle> BoundsFetcher = DefaultBoundsFetcher;

		internal static Rectangle InternalBounds
		{
			get
			{
				return _bounds;
			}
		}

		public static Widget ContextMenu { get; private set; }

		public static Widget FocusedKeyboardWidget
		{
			get { return _focusedKeyboardWidget; }

			set
			{
				if (value == _focusedKeyboardWidget)
				{
					return;
				}

				if (_focusedKeyboardWidget != null)
				{
					_focusedKeyboardWidget.OnLostKeyboardFocus();
					WidgetLostKeyboardFocus.Invoke(_focusedKeyboardWidget);
				}

				_focusedKeyboardWidget = value;

				if (_focusedKeyboardWidget != null)
				{
					_focusedKeyboardWidget.OnGotKeyboardFocus();
					WidgetGotKeyboardFocus.Invoke(_focusedKeyboardWidget);
				}
			}
		}

		public static Widget FocusedMouseWheelWidget
		{
			get; set;
		}

		private static  RenderContext RenderContext
		{
			get
			{
				EnsureRenderContext();

				return _renderContext;
			}
		}

		/// <summary>
		/// Parameters passed to SpriteBatch.Begin
		/// </summary>
		public static SpriteBatchBeginParams SpriteBatchBeginParams
		{
			get
			{
				return RenderContext.SpriteBatchBeginParams;
			}

			set
			{
				RenderContext.SpriteBatchBeginParams = value;
			}
		}

		public static float Opacity { get; set; }

		public static bool IsMouseOverGUI
		{
			get
			{
				return IsPointOverGUI(MousePosition);
			}
		}

		public static bool IsTouchOverGUI
		{
			get
			{
				return IsPointOverGUI(TouchPosition);
			}
		}

		internal static bool IsShiftDown
		{
			get
			{
				return _downKeys.Contains(Keys.LeftShift) || _downKeys.Contains(Keys.RightShift);
			}
		}

		internal static bool IsControlDown
		{
			get
			{
#if !XENKO
				return _downKeys.Contains(Keys.LeftControl) || _downKeys.Contains(Keys.RightControl);
#else
				return _downKeys.Contains(Keys.LeftCtrl) || _downKeys.Contains(Keys.RightCtrl);
#endif
			}
		}

		internal static bool IsAltDown
		{
			get
			{
#if !XENKO
				return _downKeys.Contains(Keys.LeftAlt) || _downKeys.Contains(Keys.RightAlt);
#else
				return _downKeys.Contains(Keys.LeftAlt) || _downKeys.Contains(Keys.RightAlt);
#endif
			}
		}

		public static bool IsTouchDown
		{
			get
			{
				return _isTouchDown;
			}

			set
			{
				if (value == _isTouchDown)
				{
					return;
				}

				_isTouchDown = value;
				if (_isTouchDown)
				{
					InputOnTouchDown();

					TouchDown.Invoke();

					// Only top active widget can receive touch down
					var activeWidget = GetTopWidget(true);
					if (activeWidget != null && activeWidget.Active)
					{
						var lastWidget = Widgets[Widgets.Count - 1];
						if (activeWidget is Window && lastWidget != activeWidget)
						{
							// Make active window top
							var activeIndex = Widgets.IndexOf(activeWidget);
							var lastIndex = Widgets.IndexOf(lastWidget);

							for(var i = activeIndex; i < lastIndex; ++i)
							{
								Widgets[i] = Widgets[i + 1];
							}

							Widgets[lastIndex] = activeWidget;
						}

						activeWidget.HandleTouchDown();
					}
				}
				else
				{
					TouchUp.Invoke();

					// Only top active widget can receive touch up
					var activeWidget = GetTopWidget(true);
					if (activeWidget != null && activeWidget.Active)
					{
						activeWidget.HandleTouchUp();
					}
				}
			}
		}

		public static int RepeatKeyDownStartInMs { get; set; } = 500;

		public static int RepeatKeyDownInternalInMs { get; set; } = 50;

		public static bool HasModalWidget
		{
			get
			{
				for (var i = ChildrenCopy.Count - 1; i >= 0; --i)
				{
					var w = ChildrenCopy[i];
					if (w.Visible && w.Enabled && w.IsModal)
					{
						return true;
					}
				}

				return false;
			}
		}

		private static  bool IsMenuBarActive
		{
			get
			{
				return (MenuBar != null && (MenuBar.OpenMenuItem != null || IsAltDown));
			}
		}
		
		public static Action<Keys> KeyDownHandler;

		public static event EventHandler MouseMoved;

		public static event EventHandler TouchMoved;
		public static event EventHandler TouchDown;
		public static event EventHandler TouchUp;
		public static event EventHandler TouchDoubleClick;

		public static event EventHandler<GenericEventArgs<float>> MouseWheelChanged;

		public static event EventHandler<GenericEventArgs<Keys>> KeyUp;
		public static event EventHandler<GenericEventArgs<Keys>> KeyDown;
		public static event EventHandler<GenericEventArgs<char>> Char;

		public static event EventHandler<ContextMenuClosingEventArgs> ContextMenuClosing;
		public static event EventHandler<GenericEventArgs<Widget>> ContextMenuClosed;

		public static event EventHandler<GenericEventArgs<Widget>> WidgetLostKeyboardFocus;
		public static event EventHandler<GenericEventArgs<Widget>> WidgetGotKeyboardFocus;

		static Desktop()
		{
			Opacity = 1.0f;
			_widgets.CollectionChanged += WidgetsOnCollectionChanged;

			MouseInfoGetter = DefaultMouseInfoGetter;
			DownKeysGetter = DefaultDownKeysGetter;

			KeyDownHandler = OnKeyDown;

#if FNA
			TextInputEXT.TextInput += c =>
			{
				OnChar(c);
			};
#endif
		}

#if !XENKO
		public static MouseInfo DefaultMouseInfoGetter()
		{
			var state = Mouse.GetState();

			var result = new MouseInfo
			{
				Position = new Point(state.X, state.Y),
				IsLeftButtonDown = state.LeftButton == ButtonState.Pressed,
				IsMiddleButtonDown = state.MiddleButton == ButtonState.Pressed,
				IsRightButtonDown = state.RightButton == ButtonState.Pressed,
				Wheel = state.ScrollWheelValue
			};

			return result;
		}

		public static IReadOnlyCollection<Keys> DefaultDownKeysGetter()
		{
			return Keyboard.GetState().GetPressedKeys();
		}
#else
		public static MouseInfo DefaultMouseInfoGetter()
		{
			var input = MyraEnvironment.Game.Input;

			var v = input.AbsoluteMousePosition;

			var result = new MouseInfo
			{
				Position = new Point((int)v.X, (int)v.Y),
				IsLeftButtonDown = input.IsMouseButtonDown(MouseButton.Left),
				IsMiddleButtonDown = input.IsMouseButtonDown(MouseButton.Middle),
				IsRightButtonDown = input.IsMouseButtonDown(MouseButton.Right),
				Wheel = input.MouseWheelDelta
			};

			return result;
		}

		public static IReadOnlyCollection<Keys> DefaultDownKeysGetter()
		{
			var input = MyraEnvironment.Game.Input;

			return input.Keyboard.DownKeys;
		}
#endif

		public static Widget GetChild(int index)
		{
			return ChildrenCopy[index];
		}

		private static  void HandleDoubleClick()
		{
			if ((DateTime.Now - _lastTouchDown).TotalMilliseconds < DoubleClickIntervalInMs)
			{
				TouchDoubleClick.Invoke();

				var activeWidget = GetTopWidget(true);
				if (activeWidget != null && activeWidget.Active)
				{
					activeWidget.HandleTouchDoubleClick();
				}

				_lastTouchDown = DateTime.MinValue;
			}
			else
			{
				_lastTouchDown = DateTime.Now;
			}
		}

		private static  void InputOnTouchDown()
		{
			// Handle context menu
			if (ContextMenu != null && !ContextMenu.Bounds.Contains(TouchPosition))
			{
				var ev = ContextMenuClosing;
				if (ev != null)
				{
					var args = new ContextMenuClosingEventArgs(ContextMenu);
					ev(null, args);

					if (args.Cancel)
					{
						return;
					}
				}

				HideContextMenu();
			}

			// Handle focus
			var activeWidget = GetTopWidget(true);
			if (activeWidget == null)
			{
				return;
			}

			// Widgets at the bottom of tree become focused
			Widget focusedWidget = null;
			ProcessWidgets(activeWidget, s =>
			{
				if (s.Enabled && s.IsTouchOver && s.Active && s.AcceptsKeyboardFocus)
				{
					focusedWidget = s;
				}

				return true;
			});
			FocusedKeyboardWidget = focusedWidget;

			focusedWidget = null;
			ProcessWidgets(activeWidget, s =>
			{
				if (s.Enabled && s.IsTouchOver && s.Active && s.AcceptsMouseWheelFocus)
				{
					focusedWidget = s;
				}

				return true;
			});

			if (focusedWidget != null ||
				(FocusedMouseWheelWidget != null && FocusedMouseWheelWidget.MouseWheelFocusCanBeNull))
			{
				FocusedMouseWheelWidget = focusedWidget;
			}
		}

		public static void ShowContextMenu(Widget menu, Point position)
		{
			if (menu == null)
			{
				throw new ArgumentNullException("menu");
			}

			HideContextMenu();

			ContextMenu = menu;

			if (ContextMenu != null)
			{
				ContextMenu.HorizontalAlignment = HorizontalAlignment.Left;
				ContextMenu.VerticalAlignment = VerticalAlignment.Top;

				var measure = ContextMenu.Measure(InternalBounds.Size());

				if (position.X + measure.X > InternalBounds.Right)
				{
					position.X = InternalBounds.Right - measure.X;
				}

				if (position.Y + measure.Y > InternalBounds.Bottom)
				{
					position.Y = InternalBounds.Bottom - measure.Y;
				}

				ContextMenu.Left = position.X;
				ContextMenu.Top = position.Y;

				ContextMenu.Visible = true;

				_widgets.Add(ContextMenu);

				if (ContextMenu.AcceptsKeyboardFocus)
				{
					_previousKeyboardFocus = FocusedKeyboardWidget;
					FocusedKeyboardWidget = ContextMenu;
				}

				_scheduleMouseWheelFocus = ContextMenu;
			}
		}

		public static void HideContextMenu()
		{
			if (ContextMenu == null)
			{
				return;
			}

			_widgets.Remove(ContextMenu);
			ContextMenu.Visible = false;

			ContextMenuClosed.Invoke(ContextMenu);
			ContextMenu = null;

			if (_previousKeyboardFocus != null)
			{
				FocusedKeyboardWidget = _previousKeyboardFocus;
				_previousKeyboardFocus = null;
			}

			if (_previousMouseWheelFocus != null)
			{
				FocusedMouseWheelWidget = _previousMouseWheelFocus;
				_previousMouseWheelFocus = null;
			}
		}

		private static  void WidgetsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (args.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (Widget w in args.NewItems)
				{
					w.IsPlaced = true;
					w.MeasureChanged += WOnMeasureChanged;
				}
			}
			else if (args.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (Widget w in args.OldItems)
				{
					w.MeasureChanged -= WOnMeasureChanged;
					w.IsPlaced = false;
				}
			}

			InvalidateLayout();

			_widgetsDirty = true;
		}

		private static  void WOnMeasureChanged(object sender, EventArgs eventArgs)
		{
			InvalidateLayout();
		}

		private static  void EnsureRenderContext()
		{
			if (_renderContext == null)
			{
				var spriteBatch = new SpriteBatch(MyraEnvironment.GraphicsDevice);
				_renderContext = new RenderContext
				{
					Batch = spriteBatch
				};
			}
		}

		public static void Render()
		{
			var newBounds = BoundsFetcher();

			if (_bounds != newBounds)
			{
				InvalidateLayout();
			}

			_bounds = newBounds;

			if (_bounds.IsEmpty)
			{
				return;
			}

			UpdateInput();
			UpdateLayout();

			if (_scheduleMouseWheelFocus != null)
			{
				if (_scheduleMouseWheelFocus.AcceptsMouseWheelFocus)
				{
					_previousMouseWheelFocus = FocusedMouseWheelWidget;
					FocusedMouseWheelWidget = _scheduleMouseWheelFocus;
				}

				_scheduleMouseWheelFocus = null;
			}

			EnsureRenderContext();

			var oldScissorRectangle = CrossEngineStuff.GetScissor();

			_renderContext.Begin();

			CrossEngineStuff.SetScissor(_bounds);
			_renderContext.View = _bounds;
			_renderContext.Opacity = Opacity;

			if (Stylesheet.Current.DesktopStyle != null &&
				Stylesheet.Current.DesktopStyle.Background != null)
			{
				_renderContext.Draw(Stylesheet.Current.DesktopStyle.Background, _bounds);
			}

			foreach (var widget in ChildrenCopy)
			{
				if (widget.Visible)
				{
					widget.Render(_renderContext);
				}
			}

			_renderContext.End();

			CrossEngineStuff.SetScissor(oldScissorRectangle);
		}

		public static void InvalidateLayout()
		{
			_layoutDirty = true;
		}

		public static void UpdateLayout()
		{
			if (!_layoutDirty)
			{
				return;
			}

			foreach (var widget in ChildrenCopy)
			{
				if (widget.Visible)
				{
					widget.Layout(_bounds);
				}
			}

			// Rest processing
			MenuBar = null;
			var active = true;
			for (var i = ChildrenCopy.Count - 1; i >= 0; --i)
			{
				var w = ChildrenCopy[i];
				if (!w.Visible)
				{
					continue;
				}

				ProcessWidgets(w, widget =>
				{
					widget.Active = active;

					if (MenuBar == null && widget is HorizontalMenu)
					{
						// Found MenuBar
						MenuBar = (HorizontalMenu)widget;
					}

					if (FocusedMouseWheelWidget == null && widget is ScrollViewer && widget.AcceptsMouseWheelFocus && active)
					{
						// If focused mouse wheel widget unset, then set first that accepts such focus
						FocusedMouseWheelWidget = widget;
					}

					// Continue
					return true;
				});

				// Everything after first modal widget is not active
				if (w.IsModal)
				{
					active = false;
				}
			}

			_layoutDirty = false;
		}

		public static int CalculateTotalWidgets(bool visibleOnly)
		{
			var result = 0;
			foreach (var w in _widgets)
			{
				if (visibleOnly && !w.Visible)
				{
					continue;
				}

				++result;

				var asContainer = w as Container;
				if (asContainer != null)
				{
					result += asContainer.CalculateTotalChildCount(visibleOnly);
				}
			}

			return result;
		}

		private static  Widget GetTopWidget(bool containsTouch)
		{
			for (var i = ChildrenCopy.Count - 1; i >= 0; --i)
			{
				var w = ChildrenCopy[i];
				if (w.Visible && w.Enabled &&
					(!containsTouch ||
					(containsTouch && w.Bounds.Contains(TouchPosition))))
				{
					return w;
				}
			}

			return null;
		}

		public static void HandleButton(bool isDown, bool wasDown, MouseButtons buttons)
		{
			if (isDown && !wasDown)
			{
				TouchPosition = MousePosition;
				IsTouchDown = true;
				HandleDoubleClick();
			}
			else if (!isDown && wasDown)
			{
				IsTouchDown = false;
			}
		}

#if !XENKO
		private static  void UpdateTouch()
		{
			var touchState = TouchPanel.GetState();

			if (!touchState.IsConnected)
			{
				return;
			}

			if (touchState.Count > 0)
			{
				var pos = touchState[0].Position;
				TouchPosition = new Point((int)pos.X, (int)pos.Y);
			}

			if (touchState.Count > 0 && _oldTouchState.Count == 0)
			{
				// Down
				IsTouchDown = true;
				HandleDoubleClick();
			}
			else if (touchState.Count == 0 && _oldTouchState.Count > 0)
			{
				// Up
				IsTouchDown = false;
			}

			_oldTouchState = touchState;
		}
#endif

		public static void UpdateInput()
		{
			if (MouseInfoGetter != null)
			{
				var mouseInfo = MouseInfoGetter();
				var mousePosition = mouseInfo.Position;

				if (SpriteBatchBeginParams.TransformMatrix != null)
				{
					// Apply transform
					var t = Vector2.Transform(
						new Vector2(mousePosition.X, mousePosition.Y),
						SpriteBatchBeginParams.InverseTransform);

					mousePosition = new Point((int)t.X, (int)t.Y);
				}

				MousePosition = mousePosition;

				HandleButton(mouseInfo.IsLeftButtonDown, _lastMouseInfo.IsLeftButtonDown, MouseButtons.Left);
				HandleButton(mouseInfo.IsMiddleButtonDown, _lastMouseInfo.IsMiddleButtonDown, MouseButtons.Middle);
				HandleButton(mouseInfo.IsRightButtonDown, _lastMouseInfo.IsRightButtonDown, MouseButtons.Right);
#if XENKO
				var handleWheel = mouseInfo.Wheel != 0;
#else
				var handleWheel = mouseInfo.Wheel != _lastMouseInfo.Wheel;
#endif

				if (handleWheel)
				{
					var delta = mouseInfo.Wheel;
#if !XENKO
					delta -= _lastMouseInfo.Wheel;
#endif
					MouseWheelChanged.Invoke(delta);

					if (FocusedMouseWheelWidget != null)
					{
						FocusedMouseWheelWidget.OnMouseWheel(delta);
					}
				}

				_lastMouseInfo = mouseInfo;
			}

			if (DownKeysGetter != null)
			{
				_downKeys = DownKeysGetter();

				if (_downKeys != null && _lastDownKeys != null)
				{
					var now = DateTime.Now;
					foreach (var key in _downKeys)
					{
						if (!_lastDownKeys.Contains(key))
						{
							KeyDownHandler?.Invoke(key);

							_lastKeyDown = now;
							_keyDownCount = 0;
						}
					}

					foreach (var key in _lastDownKeys)
					{
						if (!_downKeys.Contains(key))
						{
							// Key had been released
							KeyUp.Invoke(key);
							if (_focusedKeyboardWidget != null && _focusedKeyboardWidget.Active)
							{
								_focusedKeyboardWidget.OnKeyUp(key);
							}

							_lastKeyDown = null;
							_keyDownCount = 0;
						}
						else if (_lastKeyDown != null &&
						  ((_keyDownCount == 0 && (now - _lastKeyDown.Value).TotalMilliseconds > RepeatKeyDownStartInMs) ||
						  (_keyDownCount > 0 && (now - _lastKeyDown.Value).TotalMilliseconds > RepeatKeyDownInternalInMs)))
						{
							KeyDownHandler?.Invoke(key);

							_lastKeyDown = now;
							++_keyDownCount;
						}
					}
				}

				_lastDownKeys = _downKeys.ToArray();
			}

#if !XENKO
			try
			{
				UpdateTouch();
			}
			catch (Exception)
			{
			}
#endif
		}

		public static void OnKeyDown(Keys key)
		{
			KeyDown.Invoke(key);

			if (IsMenuBarActive)
			{
				MenuBar.OnKeyDown(key);
			}
			else
			{
				// Small workaround: if key is escape  active widget is window
				// Send it there
				var asWindow = GetTopWidget(false) as Window;
				if (asWindow != null && key == Keys.Escape)
				{
					asWindow.OnKeyDown(key);
				}

				if (_focusedKeyboardWidget != null && _focusedKeyboardWidget.Active)
				{
					_focusedKeyboardWidget.OnKeyDown(key);

#if XENKO
					var ch = key.ToChar(_downKeys.Contains(Keys.LeftShift) ||
										_downKeys.Contains(Keys.RightShift));
					if (ch != null)
					{
						_focusedKeyboardWidget.OnChar(ch.Value);
					}
#endif
				}
			}

			if (key == Keys.Escape && ContextMenu != null)
			{
				HideContextMenu();
			}

#if MONOGAME
			if (!HasExternalTextInput && !IsControlDown && !IsAltDown)
			{
				var c = key.ToChar(IsShiftDown);
				if (c != null)
				{
					OnChar(c.Value);
				}
			}
#endif
		}

		public static void OnChar(char c)
		{
			if (IsMenuBarActive)
			{
				// Don't accept chars if menubar is open
				return;
			}

			if (_focusedKeyboardWidget != null && _focusedKeyboardWidget.Active)
			{
				_focusedKeyboardWidget.OnChar(c);
			}

			Char.Invoke(c);
		}

		private static  bool ProcessWidgets(Widget root, Func<Widget, bool> operation)
		{
			if (!root.Visible)
			{
				return true;
			}

			var result = operation(root);
			if (!result)
			{
				return false;
			}

			var asContainer = root as Container;
			if (asContainer != null)
			{
				foreach (var w in asContainer.ChildrenCopy)
				{
					if (!ProcessWidgets(w, operation))
					{
						return false;
					}
				}
			}

			return true;
		}

		private static  void UpdateWidgetsCopy()
		{
			if (!_widgetsDirty)
			{
				return;
			}

			_widgetsCopy.Clear();
			_widgetsCopy.AddRange(_widgets);

			_widgetsDirty = false;
		}

		private static  bool InternalIsPointOverGUI(Point p, Widget w)
		{
			if (!w.Visible || !w.ActualBounds.Contains(p))
			{
				return false;
			}

			// Non containers are completely solid
			var asContainer = w as Container;
			if (asContainer == null)
			{
				return true;
			}

			// Not real containers are solid as well
			if (!(w is Grid ||
				w is StackPanel ||
				w is Panel ||
				w is SplitPane ||
				w is ScrollViewer))
			{
				return true;
			}

			// Real containers are solid only if backround is set
			if (w.Background != null)
			{
				return true;
			}

			var asScrollViewer = w as ScrollViewer;
			if (asScrollViewer != null)
			{
				// Special case
				if (asScrollViewer._horizontalScrollingOn && asScrollViewer._horizontalScrollbarFrame.Contains(p) ||
					asScrollViewer._verticalScrollingOn && asScrollViewer._verticalScrollbarFrame.Contains(p))
				{
					return true;
				}
			}

			// Or if any child is solid
			foreach (var ch in asContainer.ChildrenCopy)
			{
				if (InternalIsPointOverGUI(p, ch))
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsPointOverGUI(Point p)
		{
			foreach (var widget in ChildrenCopy)
			{
				if (InternalIsPointOverGUI(p, widget))
				{
					return true;
				}
			}

			return false;
		}
	}
}