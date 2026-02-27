using System.Collections.Immutable;
using System.Reflection;
using ArcTexel.ChangeableDocument.ChangeInfos.NodeGraph;
using ArcTexel.ChangeableDocument.Enums;
using Drawie.Numerics;

namespace ArcTexel.ChangeableDocument.ChangeInfos.Structure;

public abstract record class CreateStructureMember_ChangeInfo(
    string InternalName,
    float Opacity,
    bool IsVisible,
    bool ClipToMemberBelow,
    string Name,
    BlendMode BlendMode,
    Guid Id,
    bool HasMask,
    bool MaskIsVisible,
    ImmutableArray<NodePropertyInfo> InputProperties,
    ImmutableArray<NodePropertyInfo> OutputProperties,
    VecD position,
    NodeMetadata Metadata
) : CreateNode_ChangeInfo(InternalName, Name, position, Id, InputProperties, OutputProperties, Metadata)
{
    public ImmutableArray<NodePropertyInfo> InputProperties { get; init; } = InputProperties;
    public ImmutableArray<NodePropertyInfo> OutputProperties { get; init; } = OutputProperties;
}
