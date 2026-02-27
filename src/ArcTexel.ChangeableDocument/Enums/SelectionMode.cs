using System.ComponentModel;

namespace ArcTexel.ChangeableDocument.Enums;
public enum SelectionMode
{
    [Description("NEW")]
    New,
    [Description("ADD")]
    Add,
    [Description("SUBTRACT")]
    Subtract,
    [Description("INTERSECT")]
    Intersect
}
