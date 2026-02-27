using System.Threading.Tasks;
using Avalonia.Controls;
using ArcTexel.Models;
using ArcTexel.Extensions.CommonApi.UserPreferences.Settings.ArcTexel;
using ArcTexel.Models.Dialogs;

namespace ArcTexel.Views.Dialogs;

internal class NewFileDialog : CustomDialog
{
    private int width = ArcTexelSettings.File.DefaultNewFileWidth.Value;
    
    private int height = ArcTexelSettings.File.DefaultNewFileHeight.Value;

    public int Width
    {
        get => width;
        set => SetProperty(ref width, value);
    }

    public int Height
    {
        get => height;
        set => SetProperty(ref height, value);
    }

    public NewFileDialog(Window owner) : base(owner)
    {

    }

    public override async Task<bool> ShowDialog()
    {
        NewFilePopup popup = new()
        {
            FileWidth = Width,
            FileHeight = Height
        };

        var result = await popup.ShowDialog<bool>(OwnerWindow);

        Height = popup.FileHeight;
        Width = popup.FileWidth;

        return result;
    }
}
