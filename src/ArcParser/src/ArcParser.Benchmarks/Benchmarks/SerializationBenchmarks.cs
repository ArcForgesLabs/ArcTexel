using BenchmarkDotNet.Attributes;
using ArcTexel.Parser.Skia;
using SkiaSharp;

namespace ArcTexel.Parser.Benchmarks.Benchmarks;

public partial class Benchmarks
{
    private Document benchmarkDocument;
    private SKBitmap[] bitmaps;

    [Benchmark]
    public byte[] Serialize()
    {
        return ArcParser.V5.Serialize(benchmarkDocument);
    }

    [Benchmark]
    public byte[] SerializeAndCreate()
    {
        Document document = Helper.CreateDocument(Size, Layers, null);

        for (int i = 0; i < Layers; i++)
        {
            ImageEncoder encoder = Encoder == EncoderType.Png ? BuiltInEncoders.Encoders["PNG"] : BuiltInEncoders.Encoders["QOI"];
            byte[] encoded = encoder.Encode(bitmaps[i].Bytes, bitmaps[i].Width, bitmaps[i].Height, true);
           
            document.Graph.AllNodes[i].AdditionalData["Images"] = new System.Collections.Generic.List<System.Collections.Generic.List<byte>>()
            {
                new(encoded)
            };
        }
        
        return ArcParser.V5.Serialize(benchmarkDocument);
    }
}
