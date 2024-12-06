using Intersect.Client.Framework.GenericClasses;

namespace Intersect.Client.Framework.Input;

public interface IGameInput
{
    bool IsKeyDown(Keys key);

    bool IsPointerDown(MouseButtons mb);
}