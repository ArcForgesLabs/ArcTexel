using ArcTexel.ChangeableDocument.Enums;

namespace ArcTexel.ChangeableDocument.ChangeInfos.Root;
public record class SymmetryAxisPosition_ChangeInfo(SymmetryAxisDirection Direction, double NewPosition) : IChangeInfo;
