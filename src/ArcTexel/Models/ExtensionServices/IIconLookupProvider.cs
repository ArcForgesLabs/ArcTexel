namespace ArcTexel.Models.ExtensionServices;

public interface IIconLookupProvider
{
    public string? LookupIcon(string iconName);
}
