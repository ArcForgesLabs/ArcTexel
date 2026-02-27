using Drawie.Backend.Core.Surfaces.ImageData;

namespace ArcTexel.ChangeableDocument.ChangeInfos.Properties;

public record ProcessingColorSpace_ChangeInfo(ColorSpace NewColorSpace) : IChangeInfo;
