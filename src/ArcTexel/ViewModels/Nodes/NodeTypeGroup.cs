using System.Collections.ObjectModel;

namespace ArcTexel.ViewModels.Nodes;

public class NodeTypeGroup : ArcObservableObject
{
    public string Name { get; set; }
    public ObservableCollection<NodeTypeInfo> NodeTypes { get; set; }

    public NodeTypeGroup(string name, IEnumerable<NodeTypeInfo> nodeTypes)
    {
        Name = name;
        NodeTypes = new ObservableCollection<NodeTypeInfo>(nodeTypes);
    }    
}
