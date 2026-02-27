using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.ChangeInfos;

namespace ArcTexel.Models.DocumentPassthroughActions;

internal record AddSelectedKeyFrame_PassthroughAction(Guid KeyFrameGuid) : IChangeInfo, IAction;
