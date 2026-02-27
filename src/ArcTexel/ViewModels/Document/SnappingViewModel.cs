using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;
using ArcTexel.Models.Controllers.InputDevice;
using ArcTexel.Models.Handlers;
using Drawie.Numerics;
using ArcTexel.Models.Commands.Attributes.Commands;
using ArcTexel.UI.Common.Fonts;

namespace ArcTexel.ViewModels.Document;

public class SnappingViewModel : ArcObservableObject, ISnappingHandler
{
    private bool snappingEnabled = true;
    public SnappingController SnappingController { get; } = new SnappingController();

    public SnappingViewModel()
    {
        SnappingController.AddXYAxis("Root", VecD.Zero);
    }
    
    public void AddFromDocumentSize(VecD documentSize)
    {
        SnappingController.AddXYAxis("DocumentSize", documentSize);
        SnappingController.AddXYAxis("DocumentCenter", documentSize / 2);
    }

    public void AddFromBounds(string id, Func<RectD> tightBounds)
    {
        SnappingController.AddBounds(id, tightBounds);
    }

    public void AddFromPoint(string id, Func<VecD> func)
    {
        SnappingController.AddXYAxis(id, func);
    }

    public void Remove(string id)
    {
        SnappingController.RemoveAll(id);
    }
}
