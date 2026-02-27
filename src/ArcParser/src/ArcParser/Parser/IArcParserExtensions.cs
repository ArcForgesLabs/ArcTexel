using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ArcTexel.Parser;

#if NETSTANDARD
public static class IArcParserExtensions
{
    public static void Serialize(this IArcParser parser, IArcDocument document, string path, CancellationToken cancellationToken = default)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        parser.Serialize(document, stream, cancellationToken);
    }

    public static byte[] Serialize(this IArcParser parser, IArcDocument document, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream();
        parser.Serialize(document, stream, cancellationToken);
        return stream.ToArray();
    }

    public static async Task SerializeAsync(this IArcParser parser, IArcDocument document, string path,
        CancellationToken cancellationToken = default)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        await parser.SerializeAsync(document, stream, cancellationToken).ConfigureAwait(false);
    }
    
    public static IArcDocument Deserialize(this IArcParser parser, byte[] buffer, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream(buffer);
        return parser.Deserialize(stream, cancellationToken);
    }

    public static IArcDocument Deserialize(this IArcParser parser, string path, CancellationToken cancellationToken = default)
    {
        using var stream = File.OpenRead(path);
        return parser.Deserialize(stream, cancellationToken);
    }

    public static async Task<IArcDocument> DeserializeAsync(this IArcParser parser, string path, CancellationToken cancellationToken = default)
    {
        using var stream = File.OpenRead(path);
        return await parser.DeserializeAsync(stream, cancellationToken).ConfigureAwait(false);
    }
}
#endif