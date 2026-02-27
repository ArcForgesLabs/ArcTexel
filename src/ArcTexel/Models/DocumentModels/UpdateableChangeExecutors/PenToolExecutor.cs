using Avalonia.Input;
using ChunkyImageLib.DataHolders;
using Drawie.Backend.Core;
using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.Actions.Generated;
using Drawie.Backend.Core.ColorsImpl;
using Drawie.Backend.Core.Surfaces;
using ArcTexel.Extensions.CommonApi.Palettes;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.Handlers.Toolbars;
using ArcTexel.Models.Handlers.Tools;
using ArcTexel.Models.Tools;
using Drawie.Numerics;
using ArcTexel.ChangeableDocument.Changeables.Brushes;
using ArcTexel.ChangeableDocument.Changeables.Graph;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.Brushes;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.Shapes.Data;
using ArcTexel.ChangeableDocument.Rendering;
using ArcTexel.Models.BrushEngine;
using ArcTexel.Models.Controllers.InputDevice;

namespace ArcTexel.Models.DocumentModels.UpdateableChangeExecutors;
#nullable enable
internal class PenToolExecutor : BrushBasedExecutor<IPenToolHandler>
{
    private bool pixelPerfect;

    public override ExecutionState Start()
    {
        if (base.Start() == ExecutionState.Error)
            return ExecutionState.Error;

        var penTool = GetHandler<IPenToolHandler>();
        pixelPerfect = penTool.PixelPerfectEnabled;

        if (color.A > 0)
        {
            colorsHandler.AddSwatch(new PaletteColor(color.R, color.G, color.B));
        }

        return ExecutionState.Success;
    }

    protected override void EnqueueDrawActions()
    {
        var point = GetStabilizedPoint();
        if (handler != null)
        {
            handler.LastAppliedPoint = point;
        }

        IAction? action = pixelPerfect switch
        {
            false => new LineBasedPen_Action(layerId, point, (float)ToolSize,
                antiAliasing, BrushData, drawOnMask,
                document!.AnimationHandler.ActiveFrameBindable, controller.LastPointerInfo, controller.LastKeyboardInfo,
                controller.EditorData),
            true => new PixelPerfectPen_Action(layerId, controller!.LastPixelPosition, color, drawOnMask,
                document!.AnimationHandler.ActiveFrameBindable)
        };

        internals!.ActionAccumulator.AddActions(action);
    }

    public override void OnSettingsChanged(string name, object value)
    {
        base.OnSettingsChanged(name, value);
        if (name == nameof(IPenToolHandler.PixelPerfectEnabled) && value is bool bp)
        {
            EnqueueEndDraw();
            pixelPerfect = bp;
        }
    }

    protected override void EnqueueEndDraw()
    {
        firstApply = true;
        IAction? action = pixelPerfect switch
        {
            false => new EndLineBasedPen_Action(),
            true => new EndPixelPerfectPen_Action()
        };

        internals!.ActionAccumulator.AddFinishedActions(action);
    }
}
