using System.Diagnostics.CodeAnalysis;
using ArcTexel.ChangeableDocument.Changes.NodeGraph;

namespace ArcTexel.ChangeableDocument.ChangeInfos.NodeGraph;

public record NodePropertyInfo(
    string PropertyName,
    string DisplayName,
    Type ValueType,
    bool IsInput,
    object? InputValue,
    Guid NodeId,
    IReadOnlyList<(Guid NodeId, string PropertyName)> ConnectedProperties);
