using ArcTexel.ChangeableDocument.Changes;

namespace ArcTexel.ChangeableDocument.Actions;

internal interface IStartOrUpdateChangeAction : IAction
{
    bool IsChangeTypeMatching(Change change);
    void UpdateCorrespodingChange(UpdateableChange change);
    UpdateableChange CreateCorrespondingChange();
}
