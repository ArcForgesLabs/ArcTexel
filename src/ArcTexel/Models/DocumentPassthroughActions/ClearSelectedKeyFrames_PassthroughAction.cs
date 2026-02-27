using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.ChangeInfos;

namespace ArcTexel.Models.DocumentPassthroughActions;

internal record ClearSelectedKeyFrames_PassthroughAction() : IChangeInfo, IAction;
