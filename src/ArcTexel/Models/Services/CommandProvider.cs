using System.Windows.Input;
using Avalonia.Media;
using ArcTexel.Models.Commands;
using ArcTexel.Models.Commands.CommandContext;
using ArcTexel.Models.Commands.Commands;
using ArcTexel.Models.Commands.Evaluators;
using XAMLCommand = ArcTexel.Models.Commands.XAML.Command;

namespace ArcTexel.Models.Services;

internal class CommandProvider
{
    private readonly CommandController _controller;

    public CommandProvider(CommandController controller)
    {
        _controller = controller;
    }

    public Command GetCommand(string name) => _controller.Commands[name];

    public CanExecuteEvaluator GetCanExecute(string name) => _controller.CanExecuteEvaluators[name];

    public bool CanExecute(string name, Command command, object argument) =>
        _controller.CanExecuteEvaluators[name].CallEvaluate(command, argument);

    public IconEvaluator GetIconEvaluator(string name) => _controller.IconEvaluators[name];

    public IImage GetIcon(string name, Command command, object argument) =>
        _controller.IconEvaluators[name].CallEvaluate(command, argument);

    public ICommand GetICommand(string name, ICommandExecutionSourceInfo source, bool useProvidedArgument = false) => Commands.XAML.Command.GetICommand(_controller.Commands[name], source, useProvidedArgument);
}
