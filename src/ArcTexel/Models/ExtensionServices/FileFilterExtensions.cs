using System.Collections.Generic;
using Avalonia.Platform.Storage;
using ArcTexel.Extensions.IO;

namespace ArcTexel.Models.ExtensionServices;

public static class FileFilterExtensions
{
    public static FilePickerOpenOptions ToAvaloniaFileFilters(this FileFilter filter)
    {
        FilePickerOpenOptions options = new();
        var avaloniaFilters = new List<FilePickerFileType>();
        foreach (var filterItem in filter.Filters)
        {
            avaloniaFilters.Add(new FilePickerFileType(filterItem.Name) { Patterns = filterItem.Extensions });
        }

        options.FileTypeFilter = avaloniaFilters;
        return options;
    }
}
