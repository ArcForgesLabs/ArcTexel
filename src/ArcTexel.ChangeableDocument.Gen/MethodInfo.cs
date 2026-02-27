using System.Collections.Generic;

namespace ArcTexel.ChangeableDocument.Gen
{
    internal record struct MethodInfo(string Name, List<TypeWithName> Arguments, NamespacedType ContainingClass);
}
