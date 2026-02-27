using System.Collections;
using ArcTexel.Common;

namespace ArcTexel.ChangeableDocument.Changeables.Graph;

public interface IReadOnlyBlackboard : ICacheable
{
    public IReadOnlyVariable? GetVariable(string variableName);
    public IReadOnlyDictionary<string, IReadOnlyVariable> Variables { get; }
}
