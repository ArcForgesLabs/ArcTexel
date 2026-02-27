using ArcTexel.ChangeableDocument.Changes;

namespace ArcTexel.ChangeableDocument.Actions;

internal interface IMakeChangeAction : IAction
{
    Change CreateCorrespondingChange();
}
