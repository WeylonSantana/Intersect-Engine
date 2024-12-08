using MessagePack;

namespace Intersect.Network.Packets.Server;

[MessagePackObject]
public class LoginErrorPacket : IntersectPacket
{
    [Key(0)]
    public string Title { get; set; }

    [Key(1)]
    public string Message { get; set; }

    public LoginErrorPacket()
    {
    }

    public LoginErrorPacket(string title, string message)
    {
        Title = title;
        Message = message;
    }
}
