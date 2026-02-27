namespace ArcDocks.Core.Docking.Events;

public interface IDockableCloseEvents
{
    public Task<bool> OnClose();
}