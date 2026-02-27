using System.ComponentModel;
using Avalonia.Media.Imaging;
using ChunkyImageLib;
using ChunkyImageLib.DataHolders;
using Drawie.Backend.Core;
using Drawie.Backend.Core.Numerics;
using ArcTexel.Models.Layers;
using ArcTexel.Models.Rendering;
using Drawie.Numerics;
using ArcTexel.ViewModels.Document;
using BlendMode = ArcTexel.ChangeableDocument.Enums.BlendMode;

namespace ArcTexel.Models.Handlers;

internal interface IStructureMemberHandler : INodeHandler
{
    public bool HasMaskBindable { get; }
    public TexturePreview? MaskPreview { get; set; }
    public TexturePreview? Preview { get; set; }
    public bool MaskIsVisibleBindable { get; set; }
    public StructureMemberSelectionType Selection { get; set; }
    public float OpacityBindable { get; set; }
    public IDocument Document { get; }
    public bool IsVisibleBindable { get; set; }
    public RectD? TightBounds { get; }
    public ShapeCorners TransformationCorners { get; }
    public bool IsVisibleStructurally { get; }
    public bool ClipToMemberBelowEnabledBindable { get; }
    public void SetMaskIsVisible(bool infoIsVisible);
    public void SetClipToMemberBelowEnabled(bool infoClipToMemberBelow);
    public void SetBlendMode(BlendMode infoBlendMode);
    public void SetHasMask(bool infoHasMask);
    public void SetOpacity(float infoOpacity);
    public void SetIsVisible(bool infoIsVisible);
    public void SetName(string infoName);
    event PropertyChangedEventHandler PropertyChanged;
}
