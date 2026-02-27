using Avalonia.Input;
using ArcTexel.ChangeableDocument.Enums;
using Drawie.Backend.Core.ColorsImpl;
using Drawie.Backend.Core.Numerics;
using ArcTexel.Models.Events;
using ArcTexel.Models.Handlers;
using Drawie.Numerics;
using ArcTexel.Models.Controllers.InputDevice;
using ArcTexel.Views.Overlays.SymmetryOverlay;

namespace ArcTexel.Models.DocumentModels.Public;
internal class DocumentEventsModule
{
    private IDocument DocumentsHandler { get; }
    private DocumentInternalParts Internals { get; }

    public DocumentEventsModule(IDocument documentsHandler, DocumentInternalParts internals)
    {
        DocumentsHandler = documentsHandler;
        Internals = internals;
    }

    public void OnKeyDown(Key args) { }
    public void OnKeyUp(Key args) { }

    public void OnConvertedKeyDown(FilteredKeyEventArgs args)
    {
        Internals.ChangeController.ConvertedKeyDownInlet(args);
        DocumentsHandler.TransformHandler.KeyModifiersInlet(args.IsShiftDown, args.IsCtrlDown, args.IsAltDown);
    }
    public void OnConvertedKeyUp(FilteredKeyEventArgs args)
    {
        Internals.ChangeController.ConvertedKeyUpInlet(args);
        DocumentsHandler.TransformHandler.KeyModifiersInlet(args.IsShiftDown, args.IsCtrlDown, args.IsAltDown);
    }

    public void OnCanvasLeftMouseButtonDown(MouseOnCanvasEventArgs args) => Internals.ChangeController.LeftMouseButtonDownInlet(args);
    public void OnCanvasMouseMove(MouseOnCanvasEventArgs args)
    {
        DocumentsHandler.CoordinatesString = $"X: {(int)args.Point.PositionOnCanvas.X} Y: {(int)args.Point.PositionOnCanvas.Y}";
        Internals.ChangeController.MouseMoveInlet(args);
    }

    public void OnCanvasLeftMouseButtonUp(VecD argsPositionOnCanvas) => Internals.ChangeController.LeftMouseButtonUpInlet(argsPositionOnCanvas);
    public void OnCanvasRightMouseButtonDown(MouseOnCanvasEventArgs args) => Internals.ChangeController.RightMouseButtonDownInlet(args);
    public void OnCanvasRightMouseButtonUp(VecD argsPositionOnCanvas) => Internals.ChangeController.RightMouseButtonUpInlet(argsPositionOnCanvas);
    public void OnOpacitySliderDragStarted() => Internals.ChangeController.OpacitySliderDragStartedInlet();
    public void OnOpacitySliderDragged(float newValue) => Internals.ChangeController.OpacitySliderDraggedInlet(newValue);
    public void OnOpacitySliderDragEnded() => Internals.ChangeController.OpacitySliderDragEndedInlet();
    public void OnApplyTransform() => Internals.ChangeController.TransformAppliedInlet();
    public void SettingsChanged(string name, object value) => Internals.ChangeController.SettingsChangedInlet(name, value);
    public void PrimaryColorChanged(Color color) => Internals.ChangeController.PrimaryColorChangedInlet(color);
    public void SecondaryColorChanged(Color color) => Internals.ChangeController.SecondaryColorChangedInlet(color);
    public void OnSymmetryDragStarted(SymmetryAxisDirection dir) => Internals.ChangeController.SymmetryDragStartedInlet(dir);
    public void OnSymmetryDragged(SymmetryAxisDragInfo info) => Internals.ChangeController.SymmetryDraggedInlet(info);
    public void OnSymmetryDragEnded(SymmetryAxisDirection dir) => Internals.ChangeController.SymmetryDragEndedInlet(dir);

    public void QuickToolSwitchInlet() => Internals.ChangeController.QuickToolSwitchInlet();
}
