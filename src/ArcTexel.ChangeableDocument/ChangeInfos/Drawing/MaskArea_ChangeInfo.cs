using Drawie.Backend.Core.Numerics;

namespace ArcTexel.ChangeableDocument.ChangeInfos.Drawing;

public record class MaskArea_ChangeInfo(Guid Id, AffectedArea Area) : IChangeInfo;
