namespace ArcTexel.ChangeableDocument.Changeables.Graph;

[AttributeUsage(AttributeTargets.Class)]
public class NodeInfoAttribute : Attribute
{
    public string UniqueName { get; }
    
    public NodeInfoAttribute(string uniqueName)
    {
        if (!uniqueName.StartsWith("ArcTexel"))
        {
            uniqueName = $"ArcTexel.{uniqueName}";
        }
        
        UniqueName = uniqueName;
    }
}
