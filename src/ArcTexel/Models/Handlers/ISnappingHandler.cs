using ArcTexel.Models.Controllers.InputDevice;
using Drawie.Numerics;

namespace ArcTexel.Models.Handlers;

public interface ISnappingHandler
{
    public SnappingController SnappingController { get; }
    public void Remove(string id);
    public void AddFromBounds(string id, Func<RectD> tightBounds);
    public void AddFromPoint(string id, Func<VecD> func);
}
