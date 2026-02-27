using System.Collections.ObjectModel;
using Drawie.Backend.Core;
using ArcTexel.Models.BrushEngine;
using ArcTexel.Models.Controllers;
using ArcTexel.ViewModels.SubViewModels;

namespace ArcTexel.ViewModels.Tools.ToolSettings.Settings;

internal class TextureSettingViewModel : Setting<Texture>
{
    public TextureSettingViewModel(string name, string label) : base(name)
    {
        Label = label;
    }
}
