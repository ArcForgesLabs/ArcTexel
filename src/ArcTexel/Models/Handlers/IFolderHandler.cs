using System.Collections.ObjectModel;

namespace ArcTexel.Models.Handlers;

internal interface IFolderHandler : IStructureMemberHandler
{
    internal ObservableCollection<IStructureMemberHandler> Children { get; }
}
