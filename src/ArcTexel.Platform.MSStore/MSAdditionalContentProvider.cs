using ArcTexel.IdentityProvider.ArcAuth;
using ArcTexel.ArcAuth.Exceptions;

namespace ArcTexel.Platform.MSStore;

public sealed class MSAdditionalContentProvider : IAdditionalContentProvider
{
    public string ExtensionsPath { get; }
    public ArcAuthIdentityProvider IdentityProvider { get; }

    public event Action<string, object>? OnError;

    public MSAdditionalContentProvider(string extensionsPath, ArcAuthIdentityProvider identityProvider)
    {
        IdentityProvider = identityProvider;
        ExtensionsPath = extensionsPath;
    }

    public bool IsInstalled(string productId)
    {
        var filePath = Path.Combine(ExtensionsPath, $"{productId}.arcext");
        return File.Exists(filePath);
    }

    public async Task<string?> InstallContent(string productId)
    {
        if (!IdentityProvider.IsValid) return null;

        if (IdentityProvider.User is not { IsLoggedIn: true })
        {
            return null;
        }

        try
        {
            var stream =
                await IdentityProvider.ArcAuthClient.DownloadProduct(IdentityProvider.User.SessionToken, productId);
            if (stream != null)
            {
                var filePath = Path.Combine(ExtensionsPath, $"{productId}.arcext");
                await using (var fileStream = File.Create(filePath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                await stream.DisposeAsync();

                return filePath;
            }
        }
        catch (ArcAuthException authException)
        {
            Error(authException.Message);
        }
        catch (HttpRequestException httpRequestException)
        {
            Error("CONNECTION_ERROR");
        }
        catch (TaskCanceledException timeoutException)
        {
            Error("CONNECTION_TIMEOUT");
        }

        return null;
    }

    public bool IsContentOwned(string product)
    {
        if (!PlatformHasContent(product)) return false;

        if (IdentityProvider.User is not { IsLoggedIn: true })
        {
            return false;
        }

        return IdentityProvider.User.OwnedProducts.Any(x => x.Id.Equals(product, StringComparison.OrdinalIgnoreCase));
    }

    public bool PlatformHasContent(string product)
    {
#if DEBUG
        return true;
#else
        return false;
#endif
    }

    public void Error(string error)
    {
        OnError?.Invoke(error, null);
    }
}
