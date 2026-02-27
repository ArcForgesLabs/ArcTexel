namespace ArcTexel.Models.Handlers;

public interface IWindowHandler : IHandler
{
    public object ActiveWindow { get; }
}
