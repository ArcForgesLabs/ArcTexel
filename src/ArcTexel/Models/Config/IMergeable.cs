namespace ArcTexel.Models.Config;

public interface IMergeable<T>
{
    T TryMergeWith(T other);
}
