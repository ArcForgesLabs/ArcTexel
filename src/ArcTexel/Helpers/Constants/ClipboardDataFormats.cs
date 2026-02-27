using Avalonia.Input;

namespace ArcTexel.Helpers.Constants;

public static class ClipboardDataFormats
{
    public static readonly DataFormat<byte[]> MacOsPngUti = DataFormat.CreateBytesPlatformFormat("public.png");
    public static readonly DataFormat<byte[]>[] PngFormats = [
        DataFormat.CreateBytesPlatformFormat("PNG"), DataFormat.CreateBytesPlatformFormat("image/png"), MacOsPngUti ];
    public static readonly DataFormat<byte[]> LayerIdList = DataFormat.CreateBytesApplicationFormat("ArcTexel.LayerIdList");
    public static readonly DataFormat<byte[]> PositionFormat = DataFormat.CreateBytesApplicationFormat("ArcTexel.Position");
    public static readonly DataFormat<byte[]> DocumentFormat = DataFormat.CreateBytesApplicationFormat("ArcTexel.Document");
    public static readonly DataFormat<byte[]> NodeIdList = DataFormat.CreateBytesApplicationFormat("ArcTexel.NodeIdList");
    public static readonly DataFormat<byte[]> CelIdList = DataFormat.CreateBytesApplicationFormat("ArcTexel.CelIdList");
    public static readonly DataFormat<byte[]> ArcVectorData = DataFormat.CreateBytesApplicationFormat("ArcTexel.VectorData");
    public static readonly DataFormat<byte[]> UriList = DataFormat.CreateBytesPlatformFormat("text/uri-list");
    public static readonly DataFormat<byte[]> HadSelectionFormat = DataFormat.CreateBytesApplicationFormat("ArcTexel.HadSelection");
}
