using System.ComponentModel;
using Drawie.Backend.Core.Text;
using ArcTexel.Models.Controllers;
using ArcTexel.Models.Events;
using ArcTexel.Models.Handlers;

namespace ArcTexel.ViewModels.Nodes.Properties;

internal class FontFamilyNamePropertyViewModel : NodePropertyViewModel<FontFamilyName>
{
    private int index;
    public int FontFamilyIndex
    {
        get => index;
        set
        {
            SetProperty(ref index, value);
            Value = FontLibrary.AllFonts[Math.Clamp(value, 0, FontLibrary.AllFonts.Length - 1)];
        }
    }

    public FontFamilyNamePropertyViewModel(NodeViewModel node, Type valueType) : base(node, valueType)
    {
        ValueChanged += OnValueChanged;
    }

    private void OnValueChanged(INodePropertyHandler property, NodePropertyValueChangedArgs args)
    {
        index = Array.IndexOf(FontLibrary.AllFonts, Value);
        OnPropertyChanged(nameof(FontFamilyIndex));
    }
}
