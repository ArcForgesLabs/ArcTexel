using Drawie.Numerics;
using ArcTexel.ChangeableDocument.Rendering.ContextData;

namespace ArcTexel.ChangeableDocument.Changeables.Brushes;

public struct RecordedPoint
{
    public VecD Position { get; }
    public PointerInfo PointerInfo { get; }
    public KeyboardInfo KeyboardInfo { get; }
    public EditorData EditorData { get; }

    public RecordedPoint(VecD position, PointerInfo pointerInfo, KeyboardInfo keyboardInfo, EditorData editorData)
    {
        Position = position;
        PointerInfo = pointerInfo;
        KeyboardInfo = keyboardInfo;
        EditorData = editorData;
    }
}
