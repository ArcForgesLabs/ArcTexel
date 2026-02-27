namespace ArcDocks.Core.Serialization;

public class SerializedLayout
{
    public Dictionary<string, LayoutTree> LayoutRegions { get; set; } = new();
}