using MessagePack;

namespace ArcTexel.Models.Serialization;

[MessagePackObject]
public class SerializableKernel
{
    [Key("Width")]
    public int Width { get; set; }
    
    [Key("Height")]
    public int Height { get; set; }
    
    [Key("Values")]
    public float[] Values { get; set; }
}
