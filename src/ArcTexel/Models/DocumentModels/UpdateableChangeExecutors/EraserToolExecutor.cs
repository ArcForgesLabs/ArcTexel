#nullable enable
using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.Actions.Generated;
using Drawie.Backend.Core.ColorsImpl;
using Drawie.Backend.Core.Numerics;
using ArcTexel.Extensions.CommonApi.Palettes;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.Handlers.Toolbars;
using ArcTexel.Models.Handlers.Tools;
using ArcTexel.Models.Tools;
using Drawie.Numerics;
using ArcTexel.ChangeableDocument.Changeables.Brushes;
using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.Brushes;
using ArcTexel.ChangeableDocument.Rendering.ContextData;
using ArcTexel.Models.Controllers.InputDevice;

namespace ArcTexel.Models.DocumentModels.UpdateableChangeExecutors;

internal class EraserToolExecutor : BrushBasedExecutor<IEraserToolHandler>
{
    protected override void EnqueueDrawActions()
    {
        var point = GetStabilizedPoint();

        if (handler != null)
        {
            handler.LastAppliedPoint = point;
        }

        Color primaryColor = controller.EditorData.PrimaryColor.WithAlpha(0);
        EditorData data = new EditorData(primaryColor, controller.EditorData.SecondaryColor);
        var action = new LineBasedPen_Action(layerId, point, (float)ToolSize, antiAliasing,
            BrushData, drawOnMask,
            document!.AnimationHandler.ActiveFrameBindable, controller.LastPointerInfo, controller.LastKeyboardInfo, data);

        internals!.ActionAccumulator.AddActions(action);
    }
}
