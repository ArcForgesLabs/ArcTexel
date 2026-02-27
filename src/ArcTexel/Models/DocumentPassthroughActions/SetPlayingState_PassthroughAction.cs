using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.ChangeInfos;

namespace ArcTexel.Models.DocumentPassthroughActions;

public record SetPlayingState_PassthroughAction(bool Play) : IAction, IChangeInfo;
