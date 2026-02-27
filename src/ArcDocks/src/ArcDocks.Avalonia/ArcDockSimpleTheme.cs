using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Styling;
using ArcDocks.Avalonia.Controls;
using ArcDocks.Core.Docking;
using ArcDocks.Core.Serialization;

namespace ArcDocks.Avalonia;

public class ArcDockSimpleTheme : Styles
{
    public ArcDockSimpleTheme(IServiceProvider? sp = null)
    {
        AvaloniaXamlLoader.Load(sp, this);

        if (LayoutTree.TypeMap.ContainsKey(typeof(IDockable))) return; // Avoids re-adding the same types

        LayoutTree.TypeMap.Add(typeof(IDockable), typeof(Dockable));
        LayoutTree.TypeMap.Add(typeof(IDockableHost), typeof(DockableArea));
        LayoutTree.TypeMap.Add(typeof(IDockableTree), typeof(DockableTree));

        LayoutTree.TypeResolver.Add("DockableArea", typeof(IDockableHost));
        LayoutTree.TypeResolver.Add("DockableTree", typeof(IDockableTree));
    }
}
