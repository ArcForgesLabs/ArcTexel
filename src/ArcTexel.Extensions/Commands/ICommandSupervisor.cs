using ArcTexel.Extensions.CommonApi.Commands;

namespace ArcTexel.Extensions.Commands;

public interface ICommandSupervisor
{
    public bool ValidateCommandPermissions(string commandName, Extension invoker);
}
