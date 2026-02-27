using Drawie.Backend.Core.Surfaces.ImageData;
using ArcTexel.ChangeableDocument.Changeables.Animations;
using Drawie.Numerics;

namespace ArcTexel.ChangeableDocument.Changeables.Graph.Interfaces;

public interface IChunkRenderable
{
    public void RenderChunk(VecI chunkPos, ChunkResolution resolution, KeyFrameTime frameTime, ColorSpace processingColorSpace);
}
