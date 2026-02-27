using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Avalonia.Interactivity;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using ArcTexel.Helpers;
using ArcTexel.Models.Dialogs;
using ArcTexel.Models.Handlers;
using ArcTexel.Models.IO;
using ArcTexel.OperatingSystem;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.ViewModels.Nodes.Properties;

internal class StringPropertyViewModel : NodePropertyViewModel<string>
{
    private ObservableCollection<string>? availableOptions;
    private string kind = "txt";

    public string StringValue
    {
        get => Value;
        set
        {
            Value = value;
        }
    }

    public string StringNotNullValue
    {
        get => Value ?? string.Empty;
        set
        {
            if (value == null)
                return;

            Value = value;
        }
    }

    public string Kind
    {
        get => kind;
        set => SetProperty(ref kind, value);
    }

    public ObservableCollection<string>? AvailableOptions
    {
        get => availableOptions;
        set => SetProperty(ref availableOptions, value);
    }


    public StringPropertyViewModel(NodeViewModel node, Type valueType) : base(node, valueType)
    {
        PropertyChanged += StringPropertyViewModel_PropertyChanged;
    }

    private void StringPropertyViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Value))
        {
            OnPropertyChanged(nameof(StringValue));
            OnPropertyChanged(nameof(StringNotNullValue));
        }
    }
}
