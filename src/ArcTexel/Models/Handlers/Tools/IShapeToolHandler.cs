namespace ArcTexel.Models.Handlers.Tools;

internal interface IShapeToolHandler : IToolHandler
{
    public bool DrawEven { get; }
    public bool DrawFromCenter { get; }
}
