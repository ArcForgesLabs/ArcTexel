using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.ChangeInfos;
using ArcTexel.Models.Position;

namespace ArcTexel.Models.DocumentPassthroughActions;

internal record class RefreshViewport_PassthroughAction(ViewportInfo Info) : IAction, IChangeInfo;
