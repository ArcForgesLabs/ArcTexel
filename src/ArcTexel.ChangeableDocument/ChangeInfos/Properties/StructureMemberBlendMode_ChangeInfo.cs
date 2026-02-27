using ArcTexel.ChangeableDocument.Enums;

namespace ArcTexel.ChangeableDocument.ChangeInfos.Properties;
public record class StructureMemberBlendMode_ChangeInfo(Guid Id, BlendMode BlendMode) : IChangeInfo
{
}
