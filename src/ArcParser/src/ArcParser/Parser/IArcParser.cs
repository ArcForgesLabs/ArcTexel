using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ArcTexel.Parser;

public interface IArcParser
{
    #if NET5_0_OR_GREATER
    public void Serialize(IArcDocument document, string path, CancellationToken cancellationToken = default)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        Serialize(document, stream, cancellationToken);
    }

    public byte[] Serialize(IArcDocument document, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream();
        Serialize(document, stream, cancellationToken);
        return stream.ToArray();
    }

    public async Task SerializeAsync(IArcDocument document, string path,
        CancellationToken cancellationToken = default)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        await SerializeAsync(document, stream, cancellationToken).ConfigureAwait(false);
    }
    
    public IArcDocument Deserialize(byte[] buffer, CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream(buffer);
        return Deserialize(stream, cancellationToken);
    }

    public IArcDocument Deserialize(string path, CancellationToken cancellationToken = default)
    {
        using var stream = File.OpenRead(path);
        return Deserialize(stream, cancellationToken);
    }

    public async Task<IArcDocument> DeserializeAsync(string path, CancellationToken cancellationToken = default)
    {
        using var stream = File.OpenRead(path);
        return await DeserializeAsync(stream, cancellationToken).ConfigureAwait(false);
    }
    #endif

    public Task SerializeAsync(IArcDocument document, Stream stream, CancellationToken cancellationToken = default);
    public void Serialize(IArcDocument document, Stream stream, CancellationToken cancellationToken = default);
    public IArcDocument Deserialize(Stream stream, CancellationToken cancellationToken = default);
    public Task<IArcDocument> DeserializeAsync(Stream stream, CancellationToken cancellationToken = default);
}