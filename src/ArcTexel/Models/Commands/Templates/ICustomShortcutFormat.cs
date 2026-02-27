using ArcTexel.Models.Commands.Templates.Providers.Parsers;

namespace ArcTexel.Models.Commands.Templates;

public interface ICustomShortcutFormat
{
    public KeysParser KeysParser { get; }
    public string[] CustomShortcutExtensions { get; }
}
