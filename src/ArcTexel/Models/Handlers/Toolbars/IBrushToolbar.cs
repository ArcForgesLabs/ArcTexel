using System.ComponentModel;
using ArcTexel.ChangeableDocument.Changeables.Brushes;
using ArcTexel.Helpers;
using ArcTexel.Helpers.Decorators;
using ArcTexel.Models.BrushEngine;
using ArcTexel.Views.Overlays.BrushShapeOverlay;

namespace ArcTexel.Models.Handlers.Toolbars;

internal interface IBrushToolbar : IToolbar, IToolSizeToolbar
{
    public bool AntiAliasing { get; set; }
    public Brush Brush { get; set; }
    public BrushData CreateBrushData();
    public BrushData LastBrushData { get; }
    public double Stabilization { get; set; }
    public StabilizationMode StabilizationMode { get; set; }
}

public enum StabilizationMode
{
    [Description("NONE_STABILIZATION")]
    [IconName(ArcPerfectIcons.Minus)]
    None,
    [Description("TIME_BASED_STABILIZATION")]
    [IconName(ArcPerfectIcons.TimeStabilizer)]
    TimeBased,
    [Description("DISTANCE_BASED_STABILIZATION")]
    [IconName(ArcPerfectIcons.DistanceStabilizer)]
    CircleRope
}
