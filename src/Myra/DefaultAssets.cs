﻿using Myra.Graphics2D.Text;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI.Styles;
using Myra.Utility;
using SpriteFontPlus;
using System.IO;
using System.Reflection;

#if !XENKO
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#else
using Xenko.Core.Mathematics;
using Xenko.Graphics;
using Xenko.Graphics.Font;
using Texture2D = Xenko.Graphics.Texture;
using RasterizerState = Xenko.Graphics.RasterizerStateDescription;
#endif

namespace Myra
{
	public static class DefaultAssets
	{
		private const string DefaultFontName = "default_font.fnt";
		private const string DefaultSmallFontName = "default_font_small.fnt";
		private const string DefaultStylesheetName = "default_ui_skin.xml";
		private const string DefaultAtlasName = "default_ui_skin_atlas.xml";
		private const string DefaultAtlasImageName = "default_ui_skin_atlas.png";

		private static SpriteFont _font;
		private static SpriteFont _fontSmall;
		private static TextureRegionAtlas _uiSpritesheet;
		private static Stylesheet _uiStylesheet;
		private static Texture2D _uiBitmap;
		private static RasterizerState _uiRasterizerState;
		private static Texture2D _white;
		private static TextureRegion _whiteRegion;

		private static Assembly Assembly
		{
			get
			{
				return typeof(DefaultAssets).Assembly;
			}
		}

		public static Texture2D White
		{
			get
			{
				if (_white == null)
				{
					_white = CrossEngineStuff.CreateTexture2D(1, 1);
					CrossEngineStuff.SetData(_white, new[] {Color.White});
				}

				return _white;
			}
		}

		public static TextureRegion WhiteRegion
		{
			get
			{
				if (_whiteRegion == null)
				{
					_whiteRegion = new TextureRegion(White);
				}

				return _whiteRegion;
			}
		}

		public static SpriteFont Font
		{
			get
			{
				if (_font != null)
				{
					return _font;
				}

				var region = UISpritesheet.Regions["default"];
				_font = BMFontLoader.LoadText(ReadResourceAsString(DefaultFontName),
					s => new TextureWithOffset(region.Texture, region.Bounds.Location));

				return _font;
			}
		}

		public static SpriteFont FontSmall
		{
			get
			{
				if (_fontSmall != null)
				{
					return _fontSmall;
				}

				var region = UISpritesheet.Regions["font-small"];
				_fontSmall = BMFontLoader.LoadText(ReadResourceAsString(DefaultSmallFontName),
					s => new TextureWithOffset(region.Texture, region.Bounds.Location));

				return _fontSmall;
			}
		}

		public static TextureRegionAtlas UISpritesheet
		{
			get
			{
				if (_uiSpritesheet != null) return _uiSpritesheet;

				_uiSpritesheet = TextureRegionAtlas.FromXml(ReadResourceAsString(DefaultAtlasName), UIBitmap);

				return _uiSpritesheet;
			}
		}

		public static Stylesheet UIStylesheet
		{
			get
			{
				if (_uiStylesheet != null)
				{
					return _uiStylesheet;
				}

				_uiStylesheet = Stylesheet.LoadFromSource(ReadResourceAsString(DefaultStylesheetName),
					s => string.IsNullOrEmpty(s) ? null : UISpritesheet.Regions[s],
					f => f == "default-font" ? Font : FontSmall);

				return _uiStylesheet;
			}
		}

		public static Texture2D UIBitmap
		{
			get
			{
				if (_uiBitmap != null)
				{
					return _uiBitmap;
				}

				using (var stream = OpenResourceStream(DefaultAtlasImageName))
				{
					_uiBitmap = CrossEngineStuff.LoadTexture2D(stream);
				}

				return _uiBitmap;
			}
		}

		public static RasterizerState UIRasterizerState
		{
			get
			{
				if (_uiRasterizerState != null)
				{
					return _uiRasterizerState;
				}

				_uiRasterizerState = new RasterizerState
				{
					ScissorTestEnable = true
				};
				return _uiRasterizerState;
			}
		}

		static DefaultAssets()
		{
#if XENKO
			BMFontLoader.GraphicsDevice = MyraEnvironment.GraphicsDevice;
#endif
		}

		private static string ReadResourceAsString(string name)
		{
			return Assembly.ReadResourceAsString("Resources." + name);
		}

		private static Stream OpenResourceStream(string name)
		{
			return Assembly.OpenResourceStream("Resources." + name);
		}

		internal static void Dispose()
		{	
			_font = null;
			_fontSmall = null;
			_uiSpritesheet = null;
			_uiStylesheet = null;
			Stylesheet.Current = null;

			_whiteRegion = null;
			if (_white != null)
			{
				_white.Dispose();
				_white = null;
			}
		
			if (_uiBitmap != null)
			{
				_uiBitmap.Dispose();
				_uiBitmap = null;
			}

#if !XENKO
			if (_uiRasterizerState != null)
			{
				_uiRasterizerState.Dispose();
				_uiRasterizerState = null;
			}
#endif
		}
	}
}