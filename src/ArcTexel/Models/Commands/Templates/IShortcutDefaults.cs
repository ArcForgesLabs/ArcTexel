using System.Collections.Generic;

namespace ArcTexel.Models.Commands.Templates;

internal interface IShortcutDefaults
{
    List<Shortcut> DefaultShortcuts { get; }
}
