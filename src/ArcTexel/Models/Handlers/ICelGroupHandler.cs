using System.Collections.ObjectModel;
using ChunkyImageLib;

namespace ArcTexel.Models.Handlers;

internal interface ICelGroupHandler : ICelHandler
{
    public ObservableCollection<ICelHandler> Children { get; }
    public bool IsKeyFrameAt(int frame);
}
