using Drawie.Backend.Core.Numerics;

namespace ArcTexel.ChangeableDocument.ChangeInfos.Drawing;

public record class LayerImageArea_ChangeInfo(Guid Id, AffectedArea Area) : IChangeInfo;
