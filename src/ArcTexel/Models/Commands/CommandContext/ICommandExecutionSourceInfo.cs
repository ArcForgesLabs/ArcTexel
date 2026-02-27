using System.Text.Json.Serialization;

namespace ArcTexel.Models.Commands.CommandContext;

[JsonDerivedType(typeof(ShortcutSourceInfo))]
[JsonDerivedType(typeof(MenuSourceInfo))]
[JsonDerivedType(typeof(CommandBindingSourceInfo))]
[JsonDerivedType(typeof(SearchSourceInfo))]
[JsonDerivedType(typeof(ExtensionSourceInfo))]
public interface ICommandExecutionSourceInfo
{
    public CommandExecutionSourceType SourceType { get; }
}
