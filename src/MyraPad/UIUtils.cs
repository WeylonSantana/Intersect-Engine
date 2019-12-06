﻿using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using System.Collections.Generic;

namespace MyraPad
{
	public static class UIUtils
	{
		public static IEnumerable<Widget> GetRealChildren(this object w)
		{
			IEnumerable<Widget> widgets = null;
			if (w is IContent)
			{
				widgets = new Widget[] { ((IContent)w).Content };
			}
			else if (w is SplitPane)
			{
				widgets = ((SplitPane)w).Widgets;
			}
			else if (w is IMultipleItemsContainer)
			{
				var container = w as IMultipleItemsContainer;
				widgets = container.Widgets;
			}

			return widgets;
		}
	}
}