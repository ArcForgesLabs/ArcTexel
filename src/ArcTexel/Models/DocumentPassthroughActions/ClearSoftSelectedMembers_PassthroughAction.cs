using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.ChangeInfos;

namespace ArcTexel.Models.DocumentPassthroughActions;
internal record class ClearSoftSelectedMembers_PassthroughAction() : IChangeInfo, IAction;
