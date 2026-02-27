using System.Collections.ObjectModel;
using ArcTexel.Models.Handlers;
using ArcTexel.UI.Common.Fonts;
using ArcTexel.ViewModels.Tools;

namespace ArcTexel.ViewModels.SubViewModels;

internal class ToolSetViewModel : ArcObservableObject, IToolSetHandler
{
    public string Name { get; }
    public string Icon { get; }
    ICollection<IToolHandler> IToolSetHandler.Tools => Tools;
    IReadOnlyDictionary<IToolHandler, string> IToolSetHandler.IconOverwrites => IconOverwrites;
    public bool IconIsArcPerfect => !Uri.TryCreate(Icon, UriKind.Absolute, out _);

    public ObservableCollection<IToolHandler> Tools { get; } = new();
    public Dictionary<IToolHandler, string> IconOverwrites { get; set; } = new Dictionary<IToolHandler, string>();

    public ToolSetViewModel(string setName, string? icon = null)
    {
        Icon = ArcPerfectIconExtensions.TryGetByName(icon) ?? string.Empty;
        Name = setName;
    }

    public void AddTool(IToolHandler tool)
    {
        Tools.Add(tool);
    }

    public void ApplyToolSetSettings()
    {
        foreach (IToolHandler tool in Tools)
        {
            tool.ApplyToolSetSettings(this);
        }
    }

}
