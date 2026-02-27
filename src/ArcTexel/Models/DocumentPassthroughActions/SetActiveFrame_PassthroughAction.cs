using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.ChangeInfos;

namespace ArcTexel.Models.DocumentPassthroughActions;

internal record SetActiveFrame_PassthroughAction(int Frame) : IChangeInfo, IAction;
