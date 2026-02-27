using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.ChangeInfos;

namespace ArcTexel.Models.DocumentPassthroughActions;

internal enum DocumentMarkType
{
    Saved,
    Unsaved,
    Autosaved,
    UnAutosaved
}

internal record class MarkAsAutosaved_PassthroughAction(DocumentMarkType Type) : IChangeInfo, IAction;
