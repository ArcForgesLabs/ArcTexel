using Drawie.Backend.Core.Surfaces.ImageData;
using ArcTexel.Parser.Skia;

namespace ArcTexel.Models.Serialization;

public class SerializationConfig
{
    public ImageEncoder Encoder { get; set; }
    public ColorSpace ProcessingColorSpace { get; set; }
    
    public SerializationConfig(ImageEncoder encoder, ColorSpace processingColorSpace)
    {
        Encoder = encoder;
        ProcessingColorSpace = processingColorSpace;
    }
}
