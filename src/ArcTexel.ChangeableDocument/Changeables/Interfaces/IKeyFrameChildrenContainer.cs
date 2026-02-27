namespace ArcTexel.ChangeableDocument.Changeables.Interfaces;

public interface IKeyFrameChildrenContainer
{
    public IReadOnlyList<IReadOnlyKeyFrame> Children { get; }
}
