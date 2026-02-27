using Drawie.Backend.Core.Numerics;

namespace ArcTexel.ChangeableDocument.ChangeInfos.Root.ReferenceLayerChangeInfos;

public record class TransformReferenceLayer_ChangeInfo(ShapeCorners Corners) : IChangeInfo;
