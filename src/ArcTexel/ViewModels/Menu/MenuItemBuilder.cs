using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using ArcTexel.Extensions.UI;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.ViewModels.Menu;

internal abstract class MenuItemBuilder
{
    protected PixelSize IconDimensions => new PixelSize((int)ArcTexel.Models.Commands.XAML.Menu.IconDimensions, (int)ArcTexel.Models.Commands.XAML.Menu.IconDimensions);
    public abstract void ModifyMenuTree(ICollection<MenuItem> tree);
    public abstract void ModifyMenuTree(ICollection<NativeMenuItem> tree);

    protected bool TryFindMenuItem(ICollection<MenuItem> tree, string header, out MenuItem? menuItem)
    {
        foreach (var item in tree)
        {
            if (item.Header is LocalizedString localizedString && localizedString.Key == header)
            {
                menuItem = item;
                return true;
            }
            
            if(Translator.GetKey(item) == header)
            {
                menuItem = item;
                return true;
            }

            if (item.Header is string headerString && headerString == header)
            {
                menuItem = item;
                return true;
            }
        }

        menuItem = null;
        return false;
    }
    
    protected bool TryFindMenuItem(ICollection<NativeMenuItem> tree, string header, out NativeMenuItem? menuItem)
    {
        foreach (var item in tree)
        {
            if (Models.Commands.XAML.NativeMenu.GetLocalizationKeyHeader(item) == header)
            {
                menuItem = item;
                return true;
            }
            
            if(Translator.GetKey(item) == header)
            {
                menuItem = item;
                return true;
            }

            if (item.Header == header)
            {
                menuItem = item;
                return true;
            }
        }

        menuItem = null;
        return false;
    }
}
