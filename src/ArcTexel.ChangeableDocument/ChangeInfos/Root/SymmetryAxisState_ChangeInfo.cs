using ArcTexel.ChangeableDocument.Enums;

namespace ArcTexel.ChangeableDocument.ChangeInfos.Root;
public record class SymmetryAxisState_ChangeInfo(SymmetryAxisDirection Direction, bool State) : IChangeInfo;
