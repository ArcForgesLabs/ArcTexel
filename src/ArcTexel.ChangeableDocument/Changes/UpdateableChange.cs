namespace ArcTexel.ChangeableDocument.Changes;

internal abstract class UpdateableChange : Change
{
    public abstract OneOf<None, IChangeInfo, List<IChangeInfo>> ApplyTemporarily(Document target);
}
