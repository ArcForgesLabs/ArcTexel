namespace ArcTexel.ChangeableDocument.ChangeInfos.NodeGraph.Blackboard;

public record RenameBlackboardVariable_ChangeInfo(string OldName, string NewName) : IChangeInfo;
