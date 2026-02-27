using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.ChangeInfos;

namespace ArcTexel.Models.DocumentPassthroughActions;

internal record RemoveSelectedKeyFrame_PassthroughAction(Guid KeyFrameGuid) : IChangeInfo, IAction;
