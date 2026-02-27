using Avalonia.Input;
using ArcTexel.Models.Handlers.Toolbars;
using ArcTexel.Models.Tools;

namespace ArcTexel.Models.Handlers.Tools;

internal interface IBrightnessToolHandler : IBrushToolHandler
{
    public BrightnessMode BrightnessMode { get; }
    public bool Darken { get; }
    public MouseButton UsedWith { get; }
    public float CorrectionFactor { get; }
}
