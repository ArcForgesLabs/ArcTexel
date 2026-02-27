using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.ChangeInfos;

namespace ArcTexel.Models.DocumentModels.Public;

internal class InvokeAction_PassthroughAction(Action action) : IAction, IChangeInfo
{
    public Action Action { get; } = action;
}
