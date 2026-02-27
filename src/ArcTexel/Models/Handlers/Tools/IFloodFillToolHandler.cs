using ArcTexel.ChangeableDocument.Enums;

namespace ArcTexel.Models.Handlers.Tools;

internal interface IFloodFillToolHandler : IToolHandler
{
    public bool ConsiderAllLayers { get; }
    public float Tolerance { get; }
    FloodFillMode FillMode { get; }
}
