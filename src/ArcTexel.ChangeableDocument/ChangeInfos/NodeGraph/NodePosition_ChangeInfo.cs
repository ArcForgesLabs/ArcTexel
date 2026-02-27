using Drawie.Numerics;

namespace ArcTexel.ChangeableDocument.ChangeInfos.NodeGraph;

public record NodePosition_ChangeInfo(Guid NodeId, VecD NewPosition) : IChangeInfo;
