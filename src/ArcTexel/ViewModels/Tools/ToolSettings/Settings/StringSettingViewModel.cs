using System.Collections.ObjectModel;
using Drawie.Backend.Core;
using ArcTexel.Models.BrushEngine;
using ArcTexel.Models.Controllers;
using ArcTexel.ViewModels.SubViewModels;

namespace ArcTexel.ViewModels.Tools.ToolSettings.Settings;

internal class StringSettingViewModel : Setting<string>
{
    public StringSettingViewModel(string name, string label) : base(name)
    {
        Label = label;
    }
}
