using Drawie.Numerics;

namespace ArcTexel.ChangeableDocument.ChangeInfos.Objects;

public record TransformObject_ChangeInfo(Guid NodeGuid, AffectedArea Area) : IChangeInfo;
