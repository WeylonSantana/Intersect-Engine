﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Myra.Graphics2D.UI.Styles;
using Myra.Utility;
using System.Xml.Serialization;
using Myra.Attributes;

#if !XENKO
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#else
using Xenko.Core.Mathematics;
using Xenko.Input;
#endif

namespace Myra.Graphics2D.UI
{
	public abstract class Menu : SingleItemContainer<Grid>
	{
		private Proportion _imageProportion = Proportion.Auto, _shortcutProportion = Proportion.Auto;
		private ObservableCollection<IMenuItem> _items;
		private bool _dirty = true;
		private bool _internalSetSelectedIndex = false;

		[Browsable(false)]
		[XmlIgnore]
		public abstract Orientation Orientation { get; }

		[Browsable(false)]
		[XmlIgnore]
		public MenuStyle MenuStyle { get; private set; }

		[Browsable(false)]
		[XmlIgnore]
		internal MenuItem OpenMenuItem { get; private set; }

		[Browsable(false)]
		[XmlIgnore]
		public bool IsOpen
		{
			get
			{
				return OpenMenuItem != null;
			}
		}

		[Browsable(false)]
		[Content]
		public ObservableCollection<IMenuItem> Items
		{
			get { return _items; }

			internal set
			{
				if (_items == value)
				{
					return;
				}

				if (_items != null)
				{
					_items.CollectionChanged -= ItemsOnCollectionChanged;

					foreach (var menuItem in _items)
					{
						RemoveItem(menuItem);
					}
				}

				_items = value;

				if (_items != null)
				{
					_items.CollectionChanged += ItemsOnCollectionChanged;

					foreach (var menuItem in _items)
					{
						AddItem(menuItem);
					}
				}

				_dirty = true;
			}
		}

		public override bool IsPlaced
		{
			get
			{
				return base.IsPlaced;
			}

			internal set
			{
				if (IsPlaced)
				{
					Desktop.ContextMenuClosing -= DesktopOnContextMenuClosing;
					Desktop.ContextMenuClosed -= DesktopOnContextMenuClosed;
				}

				base.IsPlaced = value;

				if (IsPlaced)
				{
					Desktop.ContextMenuClosing += DesktopOnContextMenuClosing;
					Desktop.ContextMenuClosed += DesktopOnContextMenuClosed;
				}
			}
		}

		[Browsable(false)]
		[XmlIgnore]
		public int? HoverIndex
		{
			get
			{
				if (Orientation == Orientation.Horizontal)
				{
					return InternalChild.HoverColumnIndex;
				}

				return InternalChild.HoverRowIndex;
			}

			set
			{

				if (Orientation == Orientation.Horizontal)
				{
					InternalChild.HoverColumnIndex = value;
				}
				else
				{
					InternalChild.HoverRowIndex = value;
				}
			}
		}

		[Browsable(false)]
		[XmlIgnore]
		public int? SelectedIndex
		{
			get
			{
				if (Orientation == Orientation.Horizontal)
				{
					return InternalChild.SelectedColumnIndex;
				}

				return InternalChild.SelectedRowIndex;
			}

			set
			{

				if (Orientation == Orientation.Horizontal)
				{
					InternalChild.SelectedColumnIndex = value;
				}
				else
				{
					InternalChild.SelectedRowIndex = value;
				}
			}
		}

		private MenuItem SelectedMenuItem
		{
			get
			{
				return GetMenuItem(SelectedIndex);
			}
		}

		private bool HasImage
		{
			get
			{
				if (Orientation == Orientation.Horizontal)
				{
					return false;
				}

				return InternalChild.ColumnsProportions[0] == _imageProportion;
			}

			set
			{
				if (Orientation == Orientation.Horizontal)
				{
					return;
				}

				var hasImage = HasImage;
				if (hasImage == value)
				{
					return;
				}

				if (hasImage && !value)
				{
					InternalChild.ColumnsProportions.RemoveAt(0);
				} else if(!hasImage && value)
				{
					InternalChild.ColumnsProportions.Insert(0, _imageProportion);
				}

				_dirty = true;
			}
		}

		private bool HasShortcut
		{
			get
			{
				if (Orientation == Orientation.Horizontal)
				{
					return false;
				}

				return InternalChild.ColumnsProportions[InternalChild.ColumnsProportions.Count - 1] == _shortcutProportion;
			}

			set
			{
				if (Orientation == Orientation.Horizontal)
				{
					return;
				}

				var hasShortcut = HasShortcut;
				if (hasShortcut == value)
				{
					return;
				}

				if (hasShortcut && !value)
				{
					InternalChild.ColumnsProportions.RemoveAt(InternalChild.ColumnsProportions.Count - 1);
				}
				else if (!hasShortcut && value)
				{
					InternalChild.ColumnsProportions.Add(_shortcutProportion);
				}

				_dirty = true;
			}
		}

		protected Menu(string styleName)
		{
			InternalChild = new Grid
			{
				CanSelectNothing = true
			};

			if (Orientation == Orientation.Horizontal)
			{
				InternalChild.GridSelectionMode = GridSelectionMode.Column;
				InternalChild.DefaultColumnProportion = Proportion.Auto;
				InternalChild.DefaultRowProportion = Proportion.Auto;
			}
			else
			{
				InternalChild.GridSelectionMode = GridSelectionMode.Row;
				InternalChild.ColumnsProportions.Add(Proportion.Fill);
				InternalChild.DefaultRowProportion = Proportion.Auto;
			}

			InternalChild.HoverIndexChanged += OnHoverIndexChanged;
			InternalChild.SelectedIndexChanged += OnSelectedIndexChanged;
			InternalChild.TouchUp += InternalChild_TouchUp;

			OpenMenuItem = null;

			HorizontalAlignment = HorizontalAlignment.Stretch;
			VerticalAlignment = VerticalAlignment.Stretch;

			Items = new ObservableCollection<IMenuItem>();

			SetStyle(styleName);
		}

		private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			if (args.Action == NotifyCollectionChangedAction.Add)
			{
				var index = args.NewStartingIndex;
				foreach (IMenuItem item in args.NewItems)
				{
					InsertItem(item, index);
					++index;
				}
			}
			else if (args.Action == NotifyCollectionChangedAction.Remove)
			{
				foreach (IMenuItem item in args.OldItems)
				{
					RemoveItem(item);
				}
			}

			_dirty = true;
		}

		/// <summary>
		/// Recursively search for the menu item by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>null if not found</returns>
		public MenuItem FindMenuItemById(string id)
		{
			foreach (var item in _items)
			{
				var asMenuItem = item as MenuItem;
				if (asMenuItem == null)
				{
					continue;
				}

				var result = asMenuItem.FindMenuItemById(id);
				if (result != null)
				{
					return result;
				}
			}

			return null;
		}

		private void AddItem(Widget menuItem, int index)
		{
			if (Orientation == Orientation.Vertical)
			{
				menuItem.HorizontalAlignment = HorizontalAlignment.Stretch;
				menuItem.VerticalAlignment = VerticalAlignment.Stretch;
			}

			InternalChild.Widgets.Insert(index, menuItem);
		}

		private void UpdateWidgets()
		{
			var hasImage = false;
			var hasShortcut = false;
			foreach(var item in _items)
			{
				var menuItem = item as MenuItem;
				if (menuItem == null)
				{
					continue;
				}

				if (menuItem.Image != null)
				{
					hasImage = true;
				}

				if (!string.IsNullOrEmpty(menuItem.ShortcutText))
				{
					hasShortcut = true;
				}
			}

			HasImage = hasImage;
			HasShortcut = hasShortcut;
		}

		private void SetMenuItem(MenuItem menuItem)
		{
			menuItem.ImageWidget.Renderable = menuItem.Image;
			if (menuItem.ImageWidget.Renderable != null && !InternalChild.Widgets.Contains(menuItem.ImageWidget))
			{
				InternalChild.Widgets.Add(menuItem.ImageWidget);
			}
			else if (menuItem.ImageWidget.Renderable == null && InternalChild.Widgets.Contains(menuItem.ImageWidget))
			{
				InternalChild.Widgets.Remove(menuItem.ImageWidget);
			}

			menuItem.Shortcut.Text = menuItem.ShortcutText;
			if (menuItem.ShortcutColor != null)
			{
				menuItem.Shortcut.TextColor = menuItem.ShortcutColor.Value;
			}
			else if (MenuStyle != null && MenuStyle.ShortcutStyle != null)
			{
				menuItem.Shortcut.TextColor = MenuStyle.ShortcutStyle.TextColor;
			}

			if (!string.IsNullOrEmpty(menuItem.ShortcutText) && !InternalChild.Widgets.Contains(menuItem.Shortcut))
			{
				InternalChild.Widgets.Add(menuItem.Shortcut);
			}
			else if (string.IsNullOrEmpty(menuItem.ShortcutText) && InternalChild.Widgets.Contains(menuItem.Shortcut))
			{
				InternalChild.Widgets.Remove(menuItem.Shortcut);
			}

			menuItem.Label.Text = menuItem.DisplayText;
			if (menuItem.Color != null)
			{
				menuItem.Label.TextColor = menuItem.Color.Value;
			}
			else if (MenuStyle != null && MenuStyle.LabelStyle != null)
			{
				menuItem.Label.TextColor = MenuStyle.LabelStyle.TextColor;
			}

			UpdateWidgets();
		}

		private void MenuItemOnChanged(object sender, EventArgs eventArgs)
		{
			SetMenuItem((MenuItem)sender);
		}

		private void InsertItem(IMenuItem item, int index)
		{
			item.Menu = this;

			var menuItem = item as MenuItem;
			if (menuItem != null)
			{
				menuItem.Changed += MenuItemOnChanged;

				if (Orientation == Orientation.Horizontal)
				{
					menuItem.Label.ApplyLabelStyle(MenuStyle.LabelStyle);
				}
				else
				{
					menuItem.ImageWidget.ApplyPressableImageStyle(MenuStyle.ImageStyle);
					menuItem.Label.ApplyLabelStyle(MenuStyle.LabelStyle);
					menuItem.Shortcut.ApplyLabelStyle(MenuStyle.ShortcutStyle);
				}

				// Add only label, as other widgets(image and shortcut) would be optionally added by SetMenuItem
				InternalChild.Widgets.Add(menuItem.Label);
				SetMenuItem((MenuItem)item);
			}
			else
			{
				SeparatorWidget separator;
				if (Orientation == Orientation.Horizontal)
				{
					separator = new VerticalSeparator(null);
				}
				else
				{
					separator = new HorizontalSeparator(null);
				}

				separator.ApplySeparatorStyle(MenuStyle.SeparatorStyle);

				InternalChild.Widgets.Add(separator);

				((MenuSeparator)item).Separator = separator;
			}
		}

		private void AddItem(IMenuItem item)
		{
			InsertItem(item, Items.Count);
		}

		private void RemoveItem(IMenuItem item)
		{
			var menuItem = item as MenuItem;
			if (menuItem != null)
			{
				menuItem.Changed -= MenuItemOnChanged;
				InternalChild.Widgets.Remove(menuItem.ImageWidget);
				InternalChild.Widgets.Remove(menuItem.Label);
				InternalChild.Widgets.Remove(menuItem.Shortcut);
			}
			else
			{
				InternalChild.Widgets.Remove(((MenuSeparator)item).Separator);
			}
		}

		public void Close()
		{
			Desktop.HideContextMenu();
			HoverIndex = SelectedIndex = null;
		}

		private Rectangle GetItemBounds(int index)
		{
			var bounds = InternalChild.Bounds;
			if (Orientation == Orientation.Horizontal)
			{
				return new Rectangle(InternalChild.GetCellLocationX(index),
					bounds.Y,
					InternalChild.GetColumnWidth(index),
					bounds.Height);
			}

			return new Rectangle(bounds.X,
				InternalChild.GetCellLocationY(index),
				bounds.Width,
				InternalChild.GetRowHeight(index));
		}

		private void DesktopOnContextMenuClosing(object sender, ContextMenuClosingEventArgs args)
		{
			// Prevent closing/opening of the context menu
			if (OpenMenuItem != null && GetItemBounds(OpenMenuItem.Index).Contains(Desktop.TouchPosition))
			{
				args.Cancel = true;
			}
		}

		private void DesktopOnContextMenuClosed(object sender, GenericEventArgs<Widget> genericEventArgs)
		{
			OpenMenuItem = null;

			if (!_internalSetSelectedIndex)
			{
				SelectedIndex = null;
			}
		}
		
		private void OnHoverIndexChanged(object sender, EventArgs eventArgs)
		{
			var menuItem = GetMenuItem(HoverIndex);
			if (menuItem == null)
			{
				// Separators couldn't be selected
				HoverIndex = null;
				return;
			}

			if (!IsOpen)
			{
				return;
			}

			if (menuItem.CanOpen && OpenMenuItem != menuItem)
			{
				SelectedIndex = HoverIndex;
			}
		}

		private void ShowSubMenu(MenuItem menuItem)
		{
			var bounds = GetItemBounds(menuItem.Index);
			Desktop.ShowContextMenu(menuItem.SubMenu, new Point(bounds.X, bounds.Bottom));
			OpenMenuItem = menuItem;
		}

		private void OnSelectedIndexChanged(object sender, EventArgs e)
		{
			if (OpenMenuItem != null)
			{
				try
				{
					_internalSetSelectedIndex = true;
					Desktop.HideContextMenu();
				}
				finally
				{
					_internalSetSelectedIndex = false;
				}
			}

			var menuItem = SelectedMenuItem;
			if (menuItem != null && menuItem.CanOpen)
			{
				ShowSubMenu(menuItem);
			}
		}

		private void InternalChild_TouchUp(object sender, EventArgs e)
		{
			var menuItem = SelectedMenuItem;
			if (menuItem != null && !menuItem.CanOpen)
			{
				Close();
				menuItem.FireSelected();
			}
		}

		private MenuItem GetMenuItem(int? index)
		{
			if (index == null)
			{
				return null;
			}

			return Items[index.Value] as MenuItem;
		}

		private void Click(int? index)
		{
			var menuItem = GetMenuItem(index);
			if (menuItem == null)
			{
				return;
			}

			if (!menuItem.CanOpen)
			{
				Close();
				menuItem.FireSelected();
			} else
			{
				SelectedIndex = index;
			}
		}

		public override void OnKeyDown(Keys k)
		{
			if (k == Keys.Enter || k == Keys.Space)
			{
				int? selectedIndex = HoverIndex;
				if (selectedIndex != null)
				{
					var menuItem = Items[selectedIndex.Value] as MenuItem;
					if (menuItem != null && !menuItem.CanOpen)
					{
						Click(menuItem.Index);
						return;
					}
				}
			}

			var ch = k.ToChar(false);
			if (ch != null)
			{
				var c = char.ToLower(ch.Value);
				foreach (var w in Items)
				{
					var menuItem = w as MenuItem;
					if (menuItem == null)
					{
						continue;
					}

					if (menuItem.UnderscoreChar == c)
					{
						Click(menuItem.Index);
						return;
					}
				}
			}

			if (OpenMenuItem != null)
			{
				OpenMenuItem.SubMenu.OnKeyDown(k);
			}
		}

		public void MoveHover(int delta)
		{
			if (Items.Count == 0)
			{
				return;
			}

			// First step - determine index of currently selected item
			var si = SelectedIndex;
			if (si == null)
			{
				si = HoverIndex;
			}
			var hoverIndex = si != null ? si.Value : -1;
			var oldHover = hoverIndex;

			var iterations = 0;
			while (true)
			{
				if (iterations > Items.Count)
				{
					return;
				}

				hoverIndex += delta;

				if (hoverIndex < 0)
				{
					hoverIndex = Items.Count - 1;
				}

				if (hoverIndex >= Items.Count)
				{
					hoverIndex = 0;
				}

				if (Items[hoverIndex] is MenuItem)
				{
					break;
				}

				++iterations;
			}

			var menuItem = Items[hoverIndex] as MenuItem;
			if (menuItem != null)
			{
				HoverIndex = menuItem.Index;
			}
		}

		private void UpdateGrid()
		{
			if (!_dirty)
			{
				return;
			}

			var index = 0;
			var hasImage = HasImage;
			var hasShortcut = HasShortcut;

			var separatorSpan = 1;
			if (hasImage)
			{
				++separatorSpan;
			}
			if (hasShortcut)
			{
				++separatorSpan;
			}

			foreach (var item in Items)
			{
				var menuItem = item as MenuItem;
				if (menuItem != null)
				{
					if (Orientation == Orientation.Horizontal)
					{
						menuItem.Label.GridColumn = index;
						menuItem.Label.GridRow = 0;
					}
					else
					{
						var colIndex = 0;
						if (hasImage)
						{
							menuItem.ImageWidget.GridColumn = colIndex++;
							menuItem.ImageWidget.GridRow = index;
						}

						menuItem.Label.GridColumn = colIndex++;
						menuItem.Label.GridRow = index;

						if (hasShortcut)
						{
							menuItem.Shortcut.GridColumn = colIndex++;
							menuItem.Shortcut.GridRow = index;
						}
					}
				} else
				{
					var separator = (MenuSeparator)item;
					if (Orientation == Orientation.Horizontal)
					{
						separator.Separator.GridColumn = index;
						separator.Separator.GridRow = 0;
					}
					else
					{
						separator.Separator.GridColumn = 0;
						separator.Separator.GridColumnSpan = separatorSpan;
						separator.Separator.GridRow = index;
					}
				}

				item.Index = index;

				++index;
			}

			_dirty = false;
		}

		protected override Point InternalMeasure(Point availableSize)
		{
			UpdateGrid();

			return base.InternalMeasure(availableSize);
		}

		public void ApplyMenuStyle(MenuStyle style)
		{
			ApplyWidgetStyle(style);

			MenuStyle = style;

			InternalChild.SelectionHoverBackground = style.SelectionHoverBackground;
			InternalChild.SelectionBackground = style.SelectionBackground;
		}
	}
}