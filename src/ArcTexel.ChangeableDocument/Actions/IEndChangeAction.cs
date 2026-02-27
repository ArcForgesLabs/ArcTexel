using ArcTexel.ChangeableDocument.Changes;

namespace ArcTexel.ChangeableDocument.Actions;

internal interface IEndChangeAction : IAction
{
    bool IsChangeTypeMatching(Change change);
}
