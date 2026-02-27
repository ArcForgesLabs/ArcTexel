using ChunkyImageLib.DataHolders;
using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.Actions.Generated;
using Drawie.Backend.Core.Numerics;
using ArcTexel.Models.Handlers.Tools;
using ArcTexel.Models.Tools;
using Drawie.Numerics;
using ArcTexel.Models.Handlers;

namespace ArcTexel.Models.DocumentModels.UpdateableChangeExecutors;
#nullable enable
internal class RasterRectangleToolExecutor : DrawableShapeToolExecutor<IRasterRectangleToolHandler>
{
    private ShapeData lastData;
    public override ExecutorType Type => ExecutorType.ToolLinked;

    private void DrawRectangle(VecD curPos, double rotationRad, bool firstDraw)
    {
        RectI rect;
        VecI startPos = (VecI)Snap(startDrawingPos, curPos).Floor();
        if (firstDraw)
            rect = new RectI((VecI)curPos, VecI.Zero);
        else
            rect = RectI.FromTwoPixels(startPos, (VecI)curPos);
        
        lastRect = (RectD)rect;
        lastRadians = rotationRad;

        lastData = new ShapeData(rect.Center, rect.Size, toolViewModel.CornerRadius, rotationRad, (float)StrokeWidth, StrokePaintable, FillPaintable)
        {
            AntiAliasing = toolbar.AntiAliasing
        };

        internals!.ActionAccumulator.AddActions(new DrawRasterRectangle_Action(memberId, lastData, drawOnMask,
            document!.AnimationHandler.ActiveFrameBindable));
    }

    protected override bool UseGlobalUndo => false;
    protected override bool ShowApplyButton => true;

    protected override void DrawShape(VecD currentPos, double rotationRad, bool first) =>
        DrawRectangle(currentPos, rotationRad, first);

    protected override IAction SettingsChangedAction(string name, object value)
    {
        lastData = new ShapeData(lastData.Center, lastData.Size, toolViewModel.CornerRadius, lastRadians, (float)StrokeWidth, StrokePaintable, FillPaintable)
        {
            AntiAliasing = toolbar.AntiAliasing
        };
        return new DrawRasterRectangle_Action(memberId, lastData, drawOnMask,
            document!.AnimationHandler.ActiveFrameBindable);
    }

    protected override IAction TransformMovedAction(ShapeData data, ShapeCorners corners)
    {
        lastData = data;

        lastRadians = corners.RectRotation;

        return new DrawRasterRectangle_Action(memberId, data, drawOnMask,
            document!.AnimationHandler.ActiveFrameBindable);
    }

    protected override bool CanEditShape(IStructureMemberHandler layer)
    {
        return true;
    }

    protected override IAction EndDrawAction() => new EndDrawRasterRectangle_Action();
}
