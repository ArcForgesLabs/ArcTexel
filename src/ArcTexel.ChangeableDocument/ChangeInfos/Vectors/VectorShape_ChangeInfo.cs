using ArcTexel.ChangeableDocument.Changeables.Graph.Nodes.Shapes.Data;
using Drawie.Numerics;

namespace ArcTexel.ChangeableDocument.ChangeInfos.Vectors;

    public record VectorShape_ChangeInfo(Guid LayerId, AffectedArea Affected) : IChangeInfo;
