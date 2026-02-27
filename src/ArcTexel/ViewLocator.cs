using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using ArcDocks.Core.Docking;
using ArcTexel.ViewModels.Dock;
using ArcTexel.ViewModels.Nodes.Properties;
using ArcTexel.ViewModels.SubViewModels;
using ArcTexel.ViewModels.Tools.ToolSettings.Settings;
using ArcTexel.Views.Dock;
using ArcTexel.Views.Layers;
using ArcTexel.Views.Nodes.Properties;
using ArcTexel.Views.Tools.ToolSettings.Settings;

namespace ArcTexel;

public class ViewLocator : IDataTemplate
{
    public static Dictionary<Type, Type> ViewBindingsMap = new Dictionary<Type, Type>()
    {
        [typeof(ViewportWindowViewModel)] = typeof(DocumentTemplate),
        [typeof(LazyViewportWindowViewModel)] = typeof(LazyDocumentTemplate),
        [typeof(LayersDockViewModel)] = typeof(LayersManager),
        [typeof(SinglePropertyViewModel)] = typeof(DoublePropertyView),
        [typeof(PaintableSettingViewModel)] = typeof(ColorSettingView)
    };

    public Control Build(object? data)
    {
        Type dataType = data.GetType();
        var name = dataType.FullName.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)Activator.CreateInstance(type);
        }

        if (dataType.IsGenericType)
        {
            string nameWithoutGeneric = data.GetType().FullName.Split('`')[0];
            name = nameWithoutGeneric.Replace("ViewModel", "View");
            type = Type.GetType(name);
            
            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }
        }

        type = data?.GetType() ?? typeof(object);
        if (ViewBindingsMap.TryGetValue(type, out Type viewType))
        {
            var instance = Activator.CreateInstance(viewType);
            if (instance is not null)
            {
                return (Control)instance;
            }
            else
            {
                return new TextBlock { Text = "Create Instance Failed: " + viewType.FullName };
            }
        }

        throw new KeyNotFoundException($"View for {type.FullName} not found");
    }

    public bool Match(object? data)
    {
        return data is ObservableObject or IDockable;
    }
}
