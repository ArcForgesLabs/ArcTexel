using Avalonia;
using ArcTexel.Models.Handlers;
using ArcTexel.Views.Nodes.Properties;

namespace ArcTexel.Views.Nodes;

public class SocketsInfo
{
    public Dictionary<string, INodePropertyHandler> Sockets { get; } = new();
    public Func<INodePropertyHandler, Point> GetSocketPosition { get; set; }

    public SocketsInfo(Func<INodePropertyHandler, Point> getSocketPosition)
    {
        GetSocketPosition = getSocketPosition;
    }
}
