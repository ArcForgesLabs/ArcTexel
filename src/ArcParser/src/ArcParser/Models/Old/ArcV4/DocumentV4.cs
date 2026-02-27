using System;
using System.Linq;
using MessagePack;
using ArcTexel.Parser.Collections;
using ArcTexel.Parser.Old.ArcV4.Helpers;

namespace ArcTexel.Parser.Old.ArcV4;

[MessagePackObject]
public sealed class DocumentV4 : IArcDocument
{
    [IgnoreMember] private string DebuggerDisplay => $"{Width}x{Height}, {RootFolder.GetChildrenRecursive().Count()} members";

    [IgnoreMember] private ColorCollection swatches;
    [IgnoreMember] private ColorCollection palette;

    /// <summary>
    /// The .arc version of this document
    /// </summary>
    [IgnoreMember]
    public Version Version { get; internal set; }

    /// <summary>
    /// The minimum .arc version required to parse this document
    /// </summary>
    [IgnoreMember]
    public Version MinVersion { get; internal set; }

    [IgnoreMember] public byte[] PreviewImage { get; set; }

    /// <summary>
    /// The width of the doucment
    /// </summary>
    [Key(0)]
    public int Width { get; set; }

    /// <summary>
    /// The height of the document
    /// </summary>
    [Key(1)]
    public int Height { get; set; }

    [Key(2)]
    internal ColorCollection SwatchesInternal
    {
        get => swatches;
        set => swatches = value;
    }

    [Key(3)]
    internal ColorCollection PaletteInternal
    {
        get => palette;
        set => palette = value;
    }

    [IgnoreMember]
    public ColorCollection Swatches
    {
        get => GetColorCollection(ref swatches);
#if NET5_0_OR_GREATER
            init => swatches = value;
#endif
    }

    [IgnoreMember]
    public ColorCollection Palette
    {
        get => GetColorCollection(ref palette);
#if NET5_0_OR_GREATER
            init => palette = value;
#endif
    }

    [Key(4)] 
    public Folder RootFolder { get; set; }

    [Key(5)] 
    public ReferenceLayerV4 ReferenceLayer { get; set; }

    private ColorCollection GetColorCollection(ref ColorCollection variable)
    {
        if (variable is null)
        {
            return variable = new();
        }

        return variable;
    }

    public IArcParser GetParser() => ArcParser.V4;
}
