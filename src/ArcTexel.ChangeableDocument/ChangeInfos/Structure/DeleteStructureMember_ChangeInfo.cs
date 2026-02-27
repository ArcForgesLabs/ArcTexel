using ArcTexel.ChangeableDocument.ChangeInfos.NodeGraph;

namespace ArcTexel.ChangeableDocument.ChangeInfos.Structure;

public record class DeleteStructureMember_ChangeInfo(Guid Id) : DeleteNode_ChangeInfo(Id);
