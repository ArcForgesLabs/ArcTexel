using System.Collections.Generic;

namespace ArcTexel.Parser;

/// <summary>
/// A structure member containing other structure members
/// </summary>
public interface IStructureChildrenContainer : IStructureMember
{
    public List<IStructureMember> Children { get; }
}