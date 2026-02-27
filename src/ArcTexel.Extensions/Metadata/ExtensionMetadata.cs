using System.Text.Json.Serialization;
using ArcTexel.UI.Common.Localization;

namespace ArcTexel.Extensions.Metadata;

public class ExtensionMetadata
{
    public string UniqueName { get; init; }
    public string DisplayName { get; init; }
    public string Description { get; init; }
    public Author? Author { get; init; }
    public Author? Publisher { get; init; }
    public Author[]? Contributors { get; init; }
    public string Version { get; init; }
    public string? License { get; init; }
    public string[]? Categories { get; init; }
    public LocalizationData? Localization { get; init; }
    [JsonConverter(typeof(JsonEnumFlagConverter<ExtensionPermissions>))]
    public ExtensionPermissions Permissions { get; init; }
}
