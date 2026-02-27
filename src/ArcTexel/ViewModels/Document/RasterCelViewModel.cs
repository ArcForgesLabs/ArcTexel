using ArcTexel.Models.DocumentModels;
using ArcTexel.Models.Handlers;

namespace ArcTexel.ViewModels.Document;

internal class RasterCelViewModel : CelViewModel, IRasterCelHandler
{
    public RasterCelViewModel(Guid targetLayerGuid, int startFrame, int duration, Guid id, DocumentViewModel doc, DocumentInternalParts internalParts)
        : base(startFrame, duration, targetLayerGuid, id, doc, internalParts)
    {
        
    }

}
