using System.Collections.ObjectModel;
using Avalonia.Input;
using Drawie.Numerics;
using ArcDocks.Core.Docking;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.ViewModels.Dock;

namespace ArcTexel.ViewModels.SubViewModels;

internal class LayoutViewModel : SubViewModel<ViewModelMain>
{
    private LayoutManager layoutManagerManager;
    public LayoutManager LayoutManager
    {
        get => layoutManagerManager;
        private init => SetProperty(ref layoutManagerManager, value);
    }

    public LayoutViewModel(ViewModelMain owner, LayoutManager layoutManager) : base(owner)
    {
        LayoutManager = layoutManager;
        owner.WindowSubViewModel.ViewportAdded += WindowSubViewModel_ViewportAdded;
        owner.WindowSubViewModel.LazyViewportAdded += WindowSubViewModel_LazyViewportAdded;
        owner.WindowSubViewModel.ViewportClosed += WindowSubViewModel_ViewportRemoved;
        owner.WindowSubViewModel.LazyViewportRemoved += WindowSubViewModel_LazyViewportRemoved;
    }

    [Command.Internal("ArcTexel.Layout.SplitActiveDockable")]
    [Command.Internal("ArcTexel.Layout.SplitActiveDockableLeft", Parameter = DockingDirection.Left)]
    [Command.Internal("ArcTexel.Layout.SplitActiveDockableRight", Parameter = DockingDirection.Right)]
    [Command.Internal("ArcTexel.Layout.SplitActiveDockableUp", Parameter = DockingDirection.Top)]
    [Command.Internal("ArcTexel.Layout.SplitActiveDockableDown", Parameter = DockingDirection.Bottom)]
    public void SplitActiveDockable(DockingDirection direction)
    {
        if (LayoutManager.DockContext.FocusedTarget is IDockableHost host)
        {
            if (direction == DockingDirection.Bottom)
            {
                host.SplitDown(host.ActiveDockable);
            }
            else if (direction == DockingDirection.Top)
            {
                host.SplitUp(host.ActiveDockable);
            }
            else if (direction == DockingDirection.Left)
            {
                host.SplitLeft(host.ActiveDockable);
            }
            else if (direction == DockingDirection.Right)
            {
                host.SplitRight(host.ActiveDockable);
            }
        }
    }

    private void WindowSubViewModel_ViewportAdded(ViewportWindowViewModel obj)
    {
        LayoutManager.AddViewport(obj);
    }

    private void WindowSubViewModel_ViewportRemoved(ViewportWindowViewModel obj)
    {
        LayoutManager.RemoveViewport(obj);
    }

    private void WindowSubViewModel_LazyViewportAdded(LazyViewportWindowViewModel obj)
    {
        LayoutManager.AddViewport(obj);
    }

    private void WindowSubViewModel_LazyViewportRemoved(LazyViewportWindowViewModel obj)
    {
        LayoutManager.RemoveViewport(obj);
    }
}
