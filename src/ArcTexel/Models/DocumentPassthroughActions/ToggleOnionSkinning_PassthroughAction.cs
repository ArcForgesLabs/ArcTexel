using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.ChangeInfos;

namespace ArcTexel.Models.DocumentPassthroughActions;

internal record ToggleOnionSkinning_PassthroughAction(bool IsOnionSkinningEnabled) : IAction, IChangeInfo;
