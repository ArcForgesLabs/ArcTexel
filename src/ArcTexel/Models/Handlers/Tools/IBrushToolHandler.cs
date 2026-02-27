using Drawie.Backend.Core.Vector;
using Drawie.Numerics;
using ArcTexel.Models.Input;

namespace ArcTexel.Models.Handlers.Tools;

internal interface IBrushToolHandler : IToolHandler
{
    public bool IsCustomBrushTool { get; }
    KeyCombination? DefaultShortcut { get; }
    public VecD LastAppliedPoint { get; set; }
}
