using ArcTexel.ChangeableDocument.Actions;
using ArcTexel.ChangeableDocument.ChangeInfos;
using ArcTexel.Models.Position;

namespace ArcTexel.Models.DocumentPassthroughActions;

internal record class RefreshPreview_PassthroughAction(Guid Id, Guid? SubId = null, string ElementToRender = null) : IAction, IChangeInfo;
