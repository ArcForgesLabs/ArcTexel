namespace ArcTexel.ChangeableDocument.ChangeInfos.NodeGraph;

public record ConnectProperty_ChangeInfo(
    Guid? OutputNodeId,
    Guid InputNodeId,
    string? OutputProperty,
    string InputProperty) : IChangeInfo;
