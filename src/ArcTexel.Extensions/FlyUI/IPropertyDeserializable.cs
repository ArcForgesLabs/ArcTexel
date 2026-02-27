using System.Collections.Immutable;

namespace ArcTexel.Extensions.FlyUI;

public interface IPropertyDeserializable
{
    public IEnumerable<object> GetProperties();
    public void DeserializeProperties(List<object> values);
}
