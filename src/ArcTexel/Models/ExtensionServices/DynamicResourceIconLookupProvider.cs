using ArcTexel.Helpers;

namespace ArcTexel.Models.ExtensionServices;

internal class DynamicResourceIconLookupProvider : IIconLookupProvider
{
    public string? LookupIcon(string iconName)
    {
        return ResourceLoader.GetResource<string>(iconName);
    }
}
