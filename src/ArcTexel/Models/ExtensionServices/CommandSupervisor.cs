using ArcTexel.Extensions;
using ArcTexel.Extensions.Commands;
using ArcTexel.Models.Commands;
using ArcTexel.Models.Commands.Commands;

namespace ArcTexel.Models.ExtensionServices;

internal class CommandSupervisor : ICommandSupervisor
{
    public bool ValidateCommandPermissions(string commandName, Extension invoker)
    {
        if (CommandController.Current.Commands.ContainsKey(commandName))
        {
            var command = CommandController.Current.Commands[commandName];

            if (IsOwnedByExtension(command, invoker))
            {
                return true;
            }

            if (command.InvokePermissions == CommandPermissions.Public)
            {
                return true;
            }

            if (command.InvokePermissions == CommandPermissions.Explicit)
            {
                if (command.ExplicitPermissions == null)
                {
                    return false;
                }

                foreach (var extension in command.ExplicitPermissions)
                {
                    if (invoker.Metadata.UniqueName == extension)
                    {
                        return true;
                    }
                }
            }
            else if (command.InvokePermissions == CommandPermissions.Family)
            {
                string[] split = command.InternalName.Split('.');
                if (split.Length < 2)
                {
                    return false;
                }

                string family = split[0];

                if (invoker.Metadata.UniqueName.StartsWith(family))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsOwnedByExtension(Command command, Extension invoker)
    {
        string[] split = command.InternalName.Split(':');
        if (split.Length < 2)
        {
            return false;
        }

        string family = split[0];

        if (invoker.Metadata.UniqueName == family)
        {
            return true;
        }

        return false;
    }
}
