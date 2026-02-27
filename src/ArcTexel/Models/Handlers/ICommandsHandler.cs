using ArcTexel.Models.Commands;

namespace ArcTexel.Models.Handlers;

internal interface ICommandsHandler : IHandler
{
    public CommandController CommandController { get; }
}
