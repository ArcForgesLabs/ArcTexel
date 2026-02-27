using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.ChangeInfos;

namespace ArcTexel.Models.DocumentPassthroughActions;
internal record class AddSoftSelectedMember_PassthroughAction(Guid Id) : IChangeInfo, IAction;
