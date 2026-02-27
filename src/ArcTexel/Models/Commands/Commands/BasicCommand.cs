using ArcTexel.Models.Commands.Evaluators;

namespace ArcTexel.Models.Commands.Commands;

internal partial class Command
{
    internal class BasicCommand : Command
    {
        public object Parameter { get; init; }

        public override object GetParameter() => Parameter;

        public BasicCommand(Action<object> onExecute, CanExecuteEvaluator canExecute) : base(onExecute, canExecute) { }
    }
}
