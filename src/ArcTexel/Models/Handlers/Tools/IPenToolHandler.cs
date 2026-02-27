namespace ArcTexel.Models.Handlers.Tools;

internal interface IPenToolHandler : IBrushToolHandler
{
    public bool PixelPerfectEnabled { get; }
}
