using System.Collections.Concurrent;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;

namespace ArcTexel.Models.Clipboard;

public class ArcTexelClipboard : IArcTexelClipboard
{
    private IClipboard avaloniaClipboard;
    private List<IAsyncDataTransferItem>? lastProcessingDataTransfers;

    public ArcTexelClipboard(IClipboard avaloniaClipboard)
    {
        this.avaloniaClipboard = avaloniaClipboard;
    }

    public Task ClearAsync()
    {
        return avaloniaClipboard.ClearAsync();
    }

    public async Task<string> GetTextAsync()
    {
        return await avaloniaClipboard.TryGetTextAsync();
    }

    public async Task SetTextAsync(string text)
    {
        await avaloniaClipboard.SetTextAsync(text);
    }

    public async Task SetDataObjectAsync(IAsyncDataTransfer data)
    {
        avaloniaClipboard.SetDataAsync(data);
    }

    public async Task<T> GetDataAsync<T>(DataFormat<T> id) where T : class
    {
        var data = await avaloniaClipboard.TryGetDataAsync();
        var value = await data.TryGetValueAsync(id);
        data.Dispose();
        return value!;
    }

    public async Task<IReadOnlyList<DataFormat>> GetFormatsAsync()
    {
        return await avaloniaClipboard.GetDataFormatsAsync();
    }

    public async Task<IReadOnlyList<IStorageItem>> GetFilesAsync()
    {
        return await avaloniaClipboard.TryGetFilesAsync();
    }
}
