using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace ArcTexel.Parser.Benchmarks.Benchmarks;

public partial class Benchmarks
{
    private byte[] benchmarkDocumentBytes;

    [Benchmark]
    public Document Deserialize()
    {
        return ArcParser.V5.Deserialize(benchmarkDocumentBytes);
    }

    [Benchmark]
    public async Task<Document> DeserializeAsync()
    {
        return await ArcParser.V5.DeserializeAsync("test.arc");
    }
}
