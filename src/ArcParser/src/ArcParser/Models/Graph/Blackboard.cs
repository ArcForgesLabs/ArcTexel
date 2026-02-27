using System.Collections.Generic;
using MessagePack;

namespace ArcTexel.Parser.Graph;

[MessagePackObject]
public class Blackboard
{
    [Key(0)]
    public List<Variable> Variables { get; set; } = new();
}
