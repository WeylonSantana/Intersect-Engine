using Intersect.Client.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D;

namespace Intersect.Client.Interface.Components;

public partial class MyraImage(GameTexture texture) : IImage
{
    public Microsoft.Xna.Framework.Point Size { get; set; } = new Microsoft.Xna.Framework.Point(texture.Width, texture.Height);

    private Rectangle _source = new(0, 0, texture.Width, texture.Height);

    private Rectangle _dest;

    // inherited from Myra IBrush, called by the Myra
    public void Draw(RenderContext context, Rectangle dest, Microsoft.Xna.Framework.Color color)
    {
        context.Draw((Texture2D)texture.GetTexture(), _dest, _source, color);
    }

    public void SetTextureRegion(Rectangle source, Rectangle dest)
    {
        _source = source;
        _dest = dest;
    }
}