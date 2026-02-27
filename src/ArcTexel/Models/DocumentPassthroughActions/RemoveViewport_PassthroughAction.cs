using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.ChangeInfos;

namespace ArcTexel.Models.DocumentPassthroughActions;
internal record class RemoveViewport_PassthroughAction(Guid Id) : IAction, IChangeInfo;
