using Drawie.Backend.Core;
using Drawie.Backend.Core.Surfaces;
using Drawie.Backend.Core.Surfaces.ImageData;
using Drawie.Numerics;
using ArcTexel.ChangeableDocument.Changeables.Animations;
using ArcTexel.ChangeableDocument.Changeables.Brushes;
using ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces;
using ArcTexel.ChangeableDocument.Rendering;

namespace ArcTexel.ChangeableDocument.Changeables.Graph.Context;

public class BrushRenderContext : RenderContext
{
    public BrushData BrushData { get; }
    public Texture TargetSampledTexture { get; set; }
    public Texture TargetFullTexture { get; }
    public VecD StartPoint { get; }
    public VecD LastAppliedPoint { get; }

    public BrushRenderContext(Canvas renderSurface, KeyFrameTime frameTime, ChunkResolution chunkResolution, VecI renderOutputSize, VecI documentSize, ColorSpace processingColorSpace, SamplingOptions desiredSampling, BrushData brushData, Texture? targetSampledTexture, Texture? targetFullTexture, IReadOnlyNodeGraph graph, VecD startPoint, VecD lastAppliedPoint, double opacity = 1) : base(renderSurface, frameTime, chunkResolution, renderOutputSize, documentSize, processingColorSpace, desiredSampling, graph, opacity)
    {
        BrushData = brushData;
        StartPoint = startPoint;
        LastAppliedPoint = lastAppliedPoint;
        TargetSampledTexture = targetSampledTexture;
        TargetFullTexture = targetFullTexture;
    }

    public override RenderContext Clone()
    {
        return new BrushRenderContext(RenderSurface, FrameTime, ChunkResolution, RenderOutputSize, DocumentSize,
            ProcessingColorSpace, DesiredSamplingOptions, BrushData, TargetSampledTexture, TargetFullTexture, Graph,
            StartPoint, LastAppliedPoint, Opacity)
        {
            VisibleDocumentRegion = VisibleDocumentRegion,
            AffectedArea = AffectedArea,
            FullRerender = FullRerender,
            TargetOutput = TargetOutput,
            PreviewTextures = PreviewTextures,
            EditorData = EditorData,
            KeyboardInfo = KeyboardInfo,
            PointerInfo = PointerInfo,
            ViewportData = ViewportData,
            CloneDepth = CloneDepth + 1,
        };
    }
}
