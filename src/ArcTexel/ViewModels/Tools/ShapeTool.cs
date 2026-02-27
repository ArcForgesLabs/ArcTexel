using Avalonia.Input;
using Drawie.Backend.Core.Vector;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.Handlers.Tools;
using ArcTexel.ViewModels.Tools.ToolSettings.Toolbars;
using ArcTexel.Views.Overlays.BrushShapeOverlay;

namespace ArcTexel.ViewModels.Tools;

internal abstract class ShapeTool : ToolViewModel, IShapeToolHandler
{

    public override bool UsesColor => true;

    public override bool IsErasable => true;
    public bool DrawEven { get; protected set; }
    public bool DrawFromCenter { get; protected set; }

    public ShapeTool()
    {
        Cursor = new Cursor(StandardCursorType.Cross);
        Toolbar = new FillableShapeToolbar();
    }

    protected override void OnDeselecting(bool transient)
    {
        if (!transient)
        {
            ViewModelMain.Current.DocumentManagerSubViewModel.ActiveDocument?.Operations.TryStopToolLinkedExecutor();
        }
    }
}
