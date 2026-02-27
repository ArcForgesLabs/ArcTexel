using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using ArcTexel.Parser.Skia;

namespace ArcTexel.Parser.Benchmarks.Benchmarks;

[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
[HtmlExporter]
[MarkdownExporterAttribute.GitHub]
public partial class Benchmarks
{
    [Params(1920)]
    public int Size;

    [Params(4)]
    public int Layers;

    [Params(EncoderType.Qoi, EncoderType.Png)] 
    public EncoderType Encoder;

    [GlobalSetup]
    public void Setup()
    {
        benchmarkDocument = Helper.CreateDocument(Size, Layers, Encoder == EncoderType.Png ? BuiltInEncoders.Encoders["PNG"] : BuiltInEncoders.Encoders["QOI"]);
        benchmarkDocumentBytes = ArcParser.V5.Serialize(benchmarkDocument);

        bitmaps = new SkiaSharp.SKBitmap[Layers];

        for (int i = 0; i < Layers; i++)
        {
            bitmaps[i] = Helper.CreateSKBitmap(Size);
        }

        ArcParser.V5.Serialize(benchmarkDocument, "./test.arc");
    }
}

public enum EncoderType
{
    Png,
    Qoi
}