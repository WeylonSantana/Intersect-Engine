using Intersect.Client.Framework.Graphics;
using Intersect.Client.Interface.Game.Chat;
using Intersect.Client.Localization;
using Intersect.Compression;
using Intersect.IO.Files;
using Intersect.Logging;
using Intersect.Utilities;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D;

namespace Intersect.Client.MonoGame.Graphics;

public partial class MonoTexture : GameTexture, IImage
{
    private readonly GraphicsDevice _graphicsDevice;
    private readonly string _path = "";
    private readonly string _realPath = "";
    private readonly string _name = "";
    private readonly Func<Stream>? _createStream;
    private readonly GameTexturePackFrame? _packFrame;
    private Texture2D? _texture;
    private Rectangle _source = new();
    private Rectangle _dest = new();
    private bool _doNotFree = false;
    private long _lastAccessTime;
    private bool _loadError;

    public Microsoft.Xna.Framework.Point Size { get; set; } = new();

    private MonoTexture(Texture2D texture2D, string assetName)
    {
        _graphicsDevice = texture2D.GraphicsDevice;
        _path = assetName;
        _name = assetName;
        _texture = texture2D;
        _doNotFree = true;
    }

    public MonoTexture(GraphicsDevice graphicsDevice, string filename, string realPath)
    {
        _graphicsDevice = graphicsDevice;
        _path = filename;
        _realPath = realPath;
        _name = Path.GetFileName(filename);
    }

    public MonoTexture(GraphicsDevice graphicsDevice, string assetName, Func<Stream> createStream)
    {
        _graphicsDevice = graphicsDevice;
        _path = assetName;
        _name = assetName;
        _createStream = createStream;
    }

    public MonoTexture(GraphicsDevice graphicsDevice, string filename, GameTexturePackFrame packFrame)
    {
        _graphicsDevice = graphicsDevice;
        _path = filename;
        _name = Path.GetFileName(filename);
        _packFrame = packFrame;
        Size = new(packFrame.SourceRect.Width, packFrame.SourceRect.Height);
    }

    public override string GetName() => _name;

    public override GameTexturePackFrame GetTexturePackFrame() => _packFrame;

    public void ResetAccessTime() => _lastAccessTime = Timing.Global.MillisecondsUtc + 15000;

    public static MonoTexture CreateFromTexture2D(Texture2D texture2D, string assetName) => new(texture2D, assetName);

    private void Load(Stream stream)
    {
        _texture = Texture2D.FromStream(_graphicsDevice, stream);
        if (_texture == null)
        {
            throw new InvalidDataException("Failed to load texture, received no data.");
        }

        Size = new(_texture.Width, _texture.Height);
        _loadError = false;
    }

    public void LoadTexture()
    {
        if (_texture != null)
        {
            return;
        }

        if (_createStream != null)
        {
            using var stream = _createStream();
            Load(stream);
            return;
        }

        if (_packFrame != null)
        {
            ((MonoTexture)_packFrame.PackTexture)?.LoadTexture();

            return;
        }

        _loadError = true;
        if (string.IsNullOrWhiteSpace(_realPath))
        {
            Log.Error("Invalid texture path (empty/null).");

            return;
        }

        var relativePath = FileSystemHelper.RelativePath(Directory.GetCurrentDirectory(), _path);

        if (!File.Exists(_realPath))
        {
            Log.Error($"Texture does not exist: {relativePath}");

            return;
        }

        using var fileStream = File.Open(_realPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        try
        {
            if (Path.GetExtension(_path) == ".asset")
            {
                using var gzip = GzipCompression.CreateDecompressedFileStream(fileStream);
                Load(gzip);
            }
            else
            {
                Load(fileStream);
            }
        }
        catch (Exception exception)
        {
            Log.Error(
                exception,
                $"Failed to load texture ({FileSystemHelper.FormatSize(fileStream.Length)}): {relativePath}"
            );

            ChatboxMsg.AddMessage(
                new ChatboxMsg(
                    Strings.Errors.LoadFile.ToString(Strings.Words.LcaseSprite) + " [" + _name + "]",
                    new Color(0xBF, 0x0, 0x0), Enums.ChatMessageType.Error
                )
            );
        }
    }

    public override int GetWidth()
    {
        ResetAccessTime();
        if (Size.X != -1)
        {
            return Size.X;
        }

        if (_texture == null)
        {
            LoadTexture();
        }

        if (_loadError)
        {
            Size = new Microsoft.Xna.Framework.Point(0, Size.Y);
        }

        return Size.X;
    }

    public override int GetHeight()
    {
        ResetAccessTime();
        if (Size.Y != -1)
        {
            return Size.Y;
        }

        if (_texture == null)
        {
            LoadTexture();
        }

        if (_loadError)
        {
            Size = new Microsoft.Xna.Framework.Point(Size.X, 0);
        }

        return Size.Y;
    }

    public override object GetTexture()
    {
        if (_packFrame != null)
        {
            return _packFrame.PackTexture.GetTexture();
        }

        ResetAccessTime();

        if (_texture == null)
        {
            LoadTexture();
        }

        return _texture!;
    }

    public override Color GetPixel(int x1, int y1)
    {
        if (_texture == null)
        {
            LoadTexture();
        }

        if (_loadError)
        {
            return Color.White;
        }

        var tex = _texture;
        var pack = GetTexturePackFrame();

        if (pack != null)
        {
            tex = (Texture2D)_packFrame?.PackTexture.GetTexture();

            if (pack.Rotated)
            {
                var z = x1;
                x1 = pack.Rect.Right - y1 - pack.Rect.Height;
                y1 = pack.Rect.Top + z;
            }
            else
            {
                x1 += pack.Rect.X;
                y1 += pack.Rect.Y;
            }
        }

        var pixel = new Microsoft.Xna.Framework.Color[1];
        tex?.GetData(0, new Rectangle(x1, y1, 1, 1), pixel, 0, 1);
        return new Color(pixel[0].A, pixel[0].R, pixel[0].G, pixel[0].B);
    }

    public void Update()
    {
        if (_doNotFree)
        {
            return;
        }

        if (_texture == null)
        {
            return;
        }

        if (_lastAccessTime >= Timing.Global.MillisecondsUtc)
        {
            return;
        }

        _texture.Dispose();
        _texture = null;
    }

    // inherited from Myra IBrush, called by the Myra
    public void Draw(RenderContext context, Rectangle dest, Microsoft.Xna.Framework.Color color)
    {
        if (_texture == null)
        {
            return;
        }

        if (Size.X == 0 || Size.Y == 0)
        {
            return;
        }

        if (_source.Width == 0 || _source.Height == 0)
        {
            return;
        }

        if (_dest.Width == 0 || _dest.Height == 0)
        {
            return;
        }

        context.Draw(_texture, _dest, _source, color);
    }

    public void SetTextureRegion(Rectangle source, Rectangle dest)
    {
        if (_texture == null)
        {
            LoadTexture();
        }

        _source = source;
        _dest = dest;
        _doNotFree = true;
    }
}
